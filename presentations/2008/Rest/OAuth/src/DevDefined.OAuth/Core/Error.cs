using System;
using System.IO;
using System.Net;
using DevDefined.OAuth.Provider;

namespace DevDefined.OAuth.Core
{
    public static class Error
    {
        public static Exception MissingRequiredOAuthParameter(string parameterName)
        {
            return new Exception(string.Format("Missing required parameter : {0}", parameterName));
        }

        public static Exception OAuthAuthenticationFailure(string errorMessage)
        {
            return new Exception(string.Format("OAuth authentication failed, message was: {0}", errorMessage));
        }

        public static Exception TokenCanNoLongerBeUsed(string token)
        {
            return new Exception(string.Format("Token \"{0}\" is no longer valid", token));
        }

        public static Exception FailedToParseResponse(string parameters)
        {
            return new Exception(string.Format("Failed to parse response string \"{0}\"", parameters));
        }

        public static Exception UnknownSignatureMethod(string signatureMethod)
        {
            return new Exception(string.Format("Unknown signature method \"{0}\"", signatureMethod));
        }

        public static Exception ForRsaSha1SignatureMethodYouMustSupplyAssymetricKeyParameter()
        {
            return
                new Exception(
                    "For the RSASSA-PKCS1-v1_5 signature method you must use the constructor which takes an additional AssymetricAlgorithm \"key\" parameter");
        }

        public static Exception RequestFailed(WebException innerException)
        {
            var response = innerException.Response as HttpWebResponse;

            if (response != null)
            {
                using (var reader = new StreamReader(innerException.Response.GetResponseStream()))
                {
                    string body = reader.ReadToEnd();

                    return
                        new Exception(
                            string.Format(
                                "Request for uri: {0} failed.\r\nstatus code: {1}\r\nheaders: {2}\r\nbody:\r\n{3}",
                                response.ResponseUri, response.StatusCode, response.Headers, body));
                }
            }

            return innerException;
        }

        public static Exception RequestMethodHasNotBeenAssigned(string parameter)
        {
            return new Exception(string.Format("The RequestMethod parameter \"{0}\" is null or empty.", parameter));
        }

        public static Exception FailedToValidateSignature()
        {
            return new Exception("Failed to validate signature");
        }

        public static Exception InvalidRequestToken(string token)
        {
            return new Exception(string.Format("Invalid Request Token: {0}", token));
        }

        public static Exception InvalidConsumerKey(string consumerKey)
        {
            return new Exception(string.Format("Invalid Consumer Key: {0}", consumerKey));
        }

        public static Exception AlgorithmPropertyNotSetOnSigningContext()
        {
            return
                new Exception(
                    "Algorithm Property must be set on SingingContext when using an Assymetric encryption method such as RSA-SHA1");
        }

        public static Exception SuppliedTokenWasNotIssuedToThisConsumer(string expectedConsumerKey,
                                                                        string actualConsumerKey)
        {
            return
                new Exception(
                    string.Format("Supplied token was not issued to this consumer, expected key: {0}, actual key: {1}",
                                  expectedConsumerKey, actualConsumerKey));
        }

        public static Exception AccessDeniedToProtectedResource(AccessOutcome outcome)
        {
            Uri uri = outcome.Context.GenerateUri();

            if (string.IsNullOrEmpty(outcome.AdditionalInfo))
            {
                return new AccessDeniedException(outcome, string.Format("Access to resource \"{0}\" was denied", uri));
            }

            return new AccessDeniedException(outcome,
                                             string.Format("Access to resource: {0} was denied, additional info: {1}",
                                                           uri, outcome.AdditionalInfo));
        }
    }
}