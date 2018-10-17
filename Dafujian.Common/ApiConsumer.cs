using Dafujian.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Dafujian.Common
{
    /// <summary>
    /// A class for consuming web api from a.net client
    /// </summary>
    public class ApiConsumer : IApiConsumer
    {
        /// <summary>
        /// The base uri of the resource
        /// </summary>
        public Uri BaseUri { get; set; }

        /// <summary>
        /// The request uri of the resource
        /// </summary>
        public string ResourcePath { get; set; }

        /// <summary>
        /// The resource content type
        /// </summary>
        public string ContentType { get; set; }

        public static string ApplicationXml = "application/xml";
        public static string TextXml = "text/xml";
        public static string ApplicationJson = "application/json";
        public static string TextJson = "text/json";

        /// <summary>
        /// Configure the client to consume the Api
        /// </summary>
        /// <returns></returns>
        private HttpClient ConfigureClient(IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(BaseUri.AbsoluteUri)

            };
            if (headers != null)
                foreach (var items in headers)
                {
                    client.DefaultRequestHeaders.Add(items.Key, items.Value);

                }
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType));
            return client;
        }

        /// <summary>
        /// Send a Get request. Pass header information in the parameter if any
        /// </summary>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetAsync(IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var client = ConfigureClient(headers);
            var response = await client.GetAsync(ResourcePath);
            return response;
        }

        /// <summary>
        /// Send a post request
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostAsync(HttpContent content)
        {
            var client = ConfigureClient();
            var response = await client.PostAsync(ResourcePath, content);
            return response;
        }
    }
}
