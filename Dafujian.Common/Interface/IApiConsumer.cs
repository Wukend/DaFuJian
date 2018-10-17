using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Dafujian.Common.Interface
{
    public interface IApiConsumer
    {
        /// <summary>
        /// The base uri of the resource
        /// </summary>
        Uri BaseUri { get; set; }

        /// <summary>
        /// The request uri of the resource
        /// </summary>
        string ResourcePath { get; set; }

        /// <summary>
        /// The resource content type
        /// </summary>
        string ContentType { get; set; }

        /// <summary>
        /// Send a Get request. Pass header information in the parameter if any
        /// </summary>
        /// <returns></returns>
        Task<HttpResponseMessage> GetAsync(IEnumerable<KeyValuePair<string, string>> headers = null);

        /// <summary>
        /// Send a post request
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> PostAsync(HttpContent content);
    }
}
