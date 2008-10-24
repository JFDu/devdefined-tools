using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services.Client;
using System.IO;
using System.Net;

namespace AdoDsDemo.FormsClient.Utility
{
    public static class DataServiceUtility
    {
        public static string FormatRequest(this System.Net.WebRequest request)
        {            
            StringBuilder builder = new StringBuilder();            

            builder.Append("---------------------------------").AppendLine()
                .Append("Http Method: ").Append(request.Method).Append(" (or not... see the comments)")
                .AppendLine().AppendLine()
                .Append("Uri: ").Append(request.RequestUri)
                .AppendLine().AppendLine()
                .Append("Headers")
                .AppendLine();

            foreach (string key in request.Headers)
            {
                builder.AppendLine().Append(key).Append(": ").Append(request.Headers[key]);
            }

            if (request.Method == "PUT" || request.Method == "POST")
            {
                builder.AppendLine().AppendLine();

                using (var reader = new StreamReader(request.GetRequestStream()))
                {
                    builder.Append(reader.ReadToEnd());
                }
            }

            builder.AppendLine();
            string output = builder.ToString();
            return output;
        }

        public static string FormatResponse(this DataServiceResponse response)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("---------------------------------").AppendLine()
                .Append("Data Service Response").AppendLine()
                .Append("Status Code: ").Append(response.StatusCode).AppendLine()
                .Append("Has Errors: ").Append(response.HasErrors).AppendLine()
                .AppendLine();

            builder.Append(FormatHeaders(response.Headers));

            int operationCount = 0;

            foreach (var operation in response)
            {
                builder.AppendLine().Append("Operation Response #").Append(++operationCount)
                    .AppendLine().Append(FormatOperationResponse(operation))
                    .AppendLine();
            }

            return builder.ToString();
        }

        private static string FormatHeaders(IDictionary<string, string> headers)
        {
            if (headers.Count <= 0) return "No Headers Present\r\n";
            
            StringBuilder builder = new StringBuilder();
            
            builder.Append("Headers").AppendLine();

            foreach (var pair in headers)
            {
                builder.Append(pair.Key).Append(": ").Append(pair.Value).AppendLine();
            }

            return builder.ToString();
        }

        public static string FormatEntityDescriptors(IEnumerable<EntityDescriptor> descriptors)
        {
            StringBuilder builder = new StringBuilder();

            int index = 0;

            foreach (var descriptor in descriptors)
            {
                builder.Append("EntityDescriptor #").Append(++index).AppendLine()
                    .Append("State: ").Append(descriptor.State).AppendLine();

                var exception = descriptor.Error;
                
                if (exception != null) builder.Append("Exception: ")
                    .Append(exception)
                    .AppendLine();

                builder.Append("Entity: ").Append(descriptor.Entity).AppendLine().AppendLine();
            }

            return builder.ToString();
        }

        private static string FormatOperationResponse(OperationResponse response)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("---------------------------------").AppendLine()
                .Append("Type: ").Append(response.GetType().Name).AppendLine()
                .Append("Status Code: ").Append(response.StatusCode).AppendLine()
                .Append("Has Errors: ").Append(response.HasErrors).AppendLine()
                .AppendLine();

            builder.Append(FormatHeaders(response.Headers));

            if (response is ChangesetResponse)
            {
                builder.Append(FormatEntityDescriptors(response.Cast<EntityDescriptor>()));
            }

            return builder.ToString();
        }
    }
}
