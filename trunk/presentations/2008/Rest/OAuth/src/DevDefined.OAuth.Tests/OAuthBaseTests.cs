namespace DevDefined.OAuth.Tests
{
    /*[TestFixture]
    public class OAuthBaseTests
    {
        private readonly OAuthBase authBase = new OAuthBase();
        private NameValueCollection rsaSha1Parameters;
        private Uri rsaSha1Uri;
        private X509Certificate2 certificate = TestCertificates.OAuthTestCertificate();

        [SetUp]
        public void SetUp()
        {
            rsaSha1Parameters = new NameValueCollection
                                    {
                                        {Parameters.OAuth_Signature_Method, SignatureMethod.RsaSha1},
                                        {Parameters.OAuth_Version, "1.0"},
                                        {Parameters.OAuth_Consumer_Key, "dpf43f3p2l4k3l03"},
                                        {Parameters.OAuth_Timestamp, "1196666512"},
                                        {Parameters.OAuth_Nonce, "13917289812797014437"},
                                        {"file", "vacaction.jpg"},
                                        {"size", "original"}
                                    };

            rsaSha1Uri = BuildUri("http://photos.example.net/photos", rsaSha1Parameters);
        }


        [Test]
        public void GenerateRsaSha1SignatureBase()
        {
            string result = authBase.GenerateSignatureBase(rsaSha1Uri, "dpf43f3p2l4k3l03", null, null, "GET",
                                                           "1196666512", "13917289812797014437", "RSA-SHA1");

            Assert.AreEqual(
                "GET&http%3A%2F%2Fphotos.example.net%2Fphotos&file%3Dvacaction.jpg%26oauth_consumer_key%3Ddpf43f3p2l4k3l03%26oauth_nonce%3D13917289812797014437%26oauth_signature_method%3DRSA-SHA1%26oauth_timestamp%3D1196666512%26oauth_version%3D1.0%26size%3Doriginal",
                result);
        }

        [Test]
        public void GenerateRsaSha1Signature()
        {
            string expected =
                @"jvTp/wX1TYtByB1m+Pbyo0lnCOLIsyGCH7wke8AUs3BpnwZJtAuEJkvQL2/9n4s5wUmUl4aCI4BwpraNx4RtEXMe5qg5T1LVTGliMRpKasKsW//e+RinhejgCuzoH26dyF8iY2ZZ/5D1ilgeijhV/vBka5twt399mXwaYdCwFYE=";

            
            string result = authBase.GenerateSignature(rsaSha1Uri, "dpf43f3p2l4k3l03", null, null, null, "GET",
                                                       "1196666512", "13917289812797014437", "RSA-SHA1", certificate.PrivateKey);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void CreateRsaSha1SignedUrl()
        {
            var expected =
                new Uri(
                    @"http://photos.example.net/photos?oauth_signature_method=RSA-SHA1&oauth_version=1.0&oauth_consumer_key=dpf43f3p2l4k3l03&oauth_timestamp=1196666512&oauth_nonce=13917289812797014437&file=vacaction.jpg&size=original&oauth_signature=jvTp%2FwX1TYtByB1m%2BPbyo0lnCOLIsyGCH7wke8AUs3BpnwZJtAuEJkvQL2%2F9n4s5wUmUl4aCI4BwpraNx4RtEXMe5qg5T1LVTGliMRpKasKsW%2F%2Fe%2BRinhejgCuzoH26dyF8iY2ZZ%2F5D1ilgeijhV%2FvBka5twt399mXwaYdCwFYE%3D");
            string result = authBase.GenerateSignature(rsaSha1Uri, "dpf43f3p2l4k3l03", null, null, null, "GET",
                                                       "1196666512", "13917289812797014437", "RSA-SHA1", certificate.PrivateKey);

            rsaSha1Parameters.Add(Parameters.OAuth_Signature, result);

            Uri actualUri = BuildUri("http://photos.example.net/photos", rsaSha1Parameters);

            Assert.AreEqual(expected.ToString(), actualUri.ToString());
        }

        [Test]
        public void GenerateHmacSha1Signature()
        {
            Assert.AreEqual("egQqG5AJep5sJ7anhXju1unge2I=", authBase.GenerateHmacSha1Signature("bs", "cs", null));
            Assert.AreEqual("VZVjXceV7JgPq/dOTnNmEfO0Fv8=", authBase.GenerateHmacSha1Signature("bs", "cs", "ts"));

            string baseString =
                "GET&http%3A%2F%2Fphotos.example.net%2Fphotos&file%3Dvacation.jpg%26oauth_consumer_key%3Ddpf43f3p2l4k3l03%26oauth_nonce%3Dkllo9940pd9333jh%26oauth_signature_method%3DHMAC-SHA1%26oauth_timestamp%3D1191242096%26oauth_token%3Dnnch734d00sl2jdk%26oauth_version%3D1.0%26size%3Doriginal";

            Assert.AreEqual("tR3+Ty81lMeYAr/Fid0kMTYa/WM=",
                            authBase.GenerateHmacSha1Signature(baseString, "kd94hf93k423kf44", "pfkkdhi9sl3r4s00"));
        }

        [Test]
        public void ConcatenateRequestElementsRow2()
        {
            Assert.AreEqual(
                "POST&https%3A%2F%2Fphotos.example.net%2Frequest_token&oauth_consumer_key%3Ddpf43f3p2l4k3l03%26oauth_nonce%3Dhsu94j3884jdopsl%26oauth_signature_method%3DPLAINTEXT%26oauth_timestamp%3D1191242090%26oauth_version%3D1.0",
                authBase.GenerateSignatureBase(new Uri("https://photos.example.net/request_token"), "dpf43f3p2l4k3l03",
                                               null, null, "POST", "1191242090", "hsu94j3884jdopsl", "PLAINTEXT"));
        }

        [Test]
        public void ConcatenateRequestElementsRow3()
        {
            Assert.AreEqual(
                "GET&http%3A%2F%2Fphotos.example.net%2Fphotos&file%3Dvacation.jpg%26oauth_consumer_key%3Ddpf43f3p2l4k3l03%26oauth_nonce%3Dkllo9940pd9333jh%26oauth_signature_method%3DHMAC-SHA1%26oauth_timestamp%3D1191242096%26oauth_token%3Dnnch734d00sl2jdk%26oauth_version%3D1.0%26size%3Doriginal",
                authBase.GenerateSignatureBase(
                    new Uri("http://photos.example.net/photos?file=vacation.jpg&size=original"), "dpf43f3p2l4k3l03",
                    "nnch734d00sl2jdk", null, "GET", "1191242096", "kllo9940pd9333jh", "HMAC-SHA1"));
        }

        [Test]
        public void EnsureOurUrlTranslatesToGooglesBaseUrl()        
        {
            string input = "https://www.google.com/accounts/OAuthGetRequestToken?scope=http%25253a%25252f%25252fwww.google.com%25252fm8%25252ffeeds";
            string expected = "GET&https%3A%2F%2Fwww.google.com%2Faccounts%2FOAuthGetRequestToken&oauth_consumer_key%3Dweitu.googlepages.com%26oauth_nonce%3D4810839%26oauth_signature_method%3DRSA-SHA1%26oauth_timestamp%3D1210977911%26oauth_version%3D1.0%26scope%3Dhttp%25253a%25252f%25252fwww.google.com%25252fm8%25252ffeeds";

            Assert.AreEqual(expected, authBase.GenerateSignatureBase(new Uri(input), "weitu.googlepages.com", null, null, "GET", "1210977911", "4810839", SignatureMethod.RsaSha1));
        }

        private Uri BuildUri(string url, NameValueCollection parameters)
        {
            var wholeUri = new UriBuilder(url);

            var builder = new StringBuilder();
            foreach (string parameter in parameters)
            {
                if (builder.Length > 0) builder.Append("&");
                builder.Append(parameter).Append("=").Append(HttpUtility.UrlEncode(parameters[parameter]));
            }

            wholeUri.Query = builder.ToString();

            return wholeUri.Uri;
        }
    }*/

    // TODO: implement test
    /*[Test]
    public void ConcatenateRequestElementsRow1()
    {
        Assert.AreEqual("GET&http%3A%2F%2Fexample.com&n%3Dv",
                        authBase.FormatParameters("GET", new Uri("http://example.com?n=v")));
    }*/
}