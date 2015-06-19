# Introduction #

After looking at a few OAuth examples for .Net I was disheartened by their limited implementations, and especially a lack of a good provider implementation.

I started DevDefined.OAuth originally to accompany my REST presentation for the local .Net user group in May - but have decided to continue it's development for a little while, in a hope that it might prove useful to others trying to implementing OAuth for .Net (and because I need it for an upcoming project).

For more information on OAuth itself check out http://www.oauth.net/ and http://wiki.oauth.net/ or the google group at: http://groups.google.com/group/oauth/

# Code #

**Update 4th August 2010**

The code has been moved to github, and can be found here now:

http://github.com/bittercoder/DevDefined.OAuth

All issues/patches should be logged there now, and no further commits will be made to the code on this googlecode project.

# Forks #

This library has been forked a few times, the forks can be found here:

  * http://github.com/XeroAPI
  * http://github.com/buildmaster/oauth-mvc.net/

The XeroAPI is adding some great new features to the DevDefined OAuth library - well worth checking it out.

# Guides #

  * [OAuthConsumer](OAuthConsumer.md) - implementing a consumer with the library.
  * [OAuthProvider](OAuthProvider.md) - implementing a provider with the library.
  * [OAuthSignatureValidation](OAuthSignatureValidation.md) - how to use the library to validate a signature.
  * [GoogleServicesWithCertificate](GoogleServicesWithCertificate.md) - how to use google services requiring a certificate.
  * [OpenSocial Signature Validation](http://blog.bittercoder.com/PermaLink,guid,4f387bde-7ed6-480b-952b-bbc0ead9ebfb.aspx) - how to use the library to validate an [OpenSocial](http://code.google.com/apis/opensocial/) signed request.
  * [OAuthWcfIntegration](OAuthWcfIntegration.md) - using this library with WCF.

# Blog entries referencing this library #

  * http://www.leporelo.eu/blog.aspx?id=how-to-use-oauth-to-connect-to-twitter-in-powershell
  * http://www.markerstudio.com/technical/2009/09/net-oauth-sample-working-with-justintv/
  * http://bgeek.net/blog/2009/3/3/oauth-mvcnet-revisited.html
  * http://weblogs.asp.net/cibrax/archive/2008/11/14/oauth-channel-for-wcf-restful-services.aspx
  * http://oauth.net/code/
  * http://blog.stevienova.com/2008/04/19/oauth-getting-started-with-oauth-in-c-net/

# TODO #

Work with the mono team RE: getting OAuth support into Mono Olive.

  * http://www.mono-project.com/WCF
  * http://groups.google.com/group/mono-olive/browse_thread/thread/591628b267e3f015