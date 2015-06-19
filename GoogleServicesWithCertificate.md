# Introduction #

This article briefly discusses what you need to do to:

  * Registering your domain for use with google apps.
  * Create a certificate using makecert.
  * Convert it to a pfx using pvk2pfx.
  * Upload the certificate you wish to use for your consumer.
  * Configure an OAuth session to use the certificate.

# Details #

Before anything else you need to register your domain (which will effectively be your consumer key) with google apps - see this article for more details:

http://code.google.com/apis/accounts/docs/RegistrationForWebAppsAuto.html

As part of the verification process (to ensure you "own" or at least control the content found at the domain) you must either insert some meta tags into the homepage for the domain, or upload a file into the root of the domain which google can access during the verification process - this only needs to be done until verification has completed, at which point it can be removed again.

It's worth starting this process first as it may take a little while to complete verification in some cases.

Once done we next want to generate the certificates - to do so fire up a command shell, then to continue to read - we'll need to issue a couple of commands.

Microsoft Visual Studio 2005 or later provides utilities (in the Common7\Tools\Bin directory) which can be used to generate a certificate for use with Google Apps.

Follow the steps below to create the public and private key pair and certificate in .NET:

  1. makecert -r -pe -n "CN=Test Certificate" -sky exchange -sv testcert.pvk testcert.cer
  1. pvk2pfx -pvk testcert.pvk -spc testcert.cer -pfx testcert.pfx

By default the RSA algorithm is used in the commands above. Step 1 uses the Certificate Creation Tool (makecert.exe) to create a self signed X.509 certificate called testcert.cer and the corresponding private key. Step 2 uses the pvk2pfx Tool (pvk2pfx.exe) to create a Personal Information Exchange (PFX) file from a CER and PVK file. The PFX contains both your public and private key.

See this article for more details on the above process

http://code.google.com/support/bin/answer.py?answer=71864&topic=12142

Now that we have a **testcert.cer** it can be uploaded to google via:

http://www.google.com/a/yourdomainname.com

Log in as an administrator. Select Advanced Tools, then Set up single sign-on (SSO), and fill in all the fields on the page.

Once done you are ready to start using the OAuth library with the various google services, you then just need to associate the certificate and RSA-SHA1 signature method with the oauth session via a consumer context.

```
var certificate= new X509Certificate2("c:\\temp\\testcert.pfx");

var consumerContext = new OAuthConsumerContext
{
    ConsumerKey = "yourdomainname.com",
    SignatureMethod = SignatureMethod.RsaSha1,
    Key = certificate.PrivateKey
};

var session = new OAuthSession(consumerContext, 
   "https://www.google.com/accounts/OAuthGetRequestToken",
   "https://www.google.com/accounts/accounts/OAuthAuthorizeToken",
   "https://www.google.com/accounts/OAuthGetAccessToken ");
```

Also in most cases you need to set a scope parameter when using google services which is passed along with all requests to the google services - identifying the service you wish to use - this can be done with the following call:

```
session.WithQueryParameters(new {scope = "https://www.google.com/m8/feeds"});
```