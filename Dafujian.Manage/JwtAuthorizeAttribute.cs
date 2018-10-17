using Dafujian.Common;
using Dafujian.Common.Interface;
using JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Dafujian.Manage
{
    /// <summary>
    /// Performs validation of tokens on the client side and other security functions like logout and validation of user credentials
    /// </summary>
    public class JwtAuthorizeAttribute : AuthorizeAttribute
    {

        private readonly IApiConsumer _apiConsumer;

        public JwtAuthorizeAttribute()
        {
            _apiConsumer = new ApiConsumer();
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (base.AuthorizeCore(httpContext))
            {
                return false;
            }
            return IsTokenValid();
            //return base.AuthorizeCore(httpContext);
        }

        /// <summary>
        /// If a token is present on the client side, the the request is authenticated
        /// </summary>
        /// <returns></returns>
        private bool IsAuthenticated()
        {
            try
            {
                var token = Utils.GetCookie("lc_token");
                return token != null;
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if the token is valid
        /// </summary>
        /// <returns></returns>
        private bool IsTokenValid()
        {
            try
            {
                if (IsAuthenticated()) //If token is found in cookie
                {
                    //check expiry date
                    var jsonSerializer = new JavaScriptSerializer();
                    var payloadJson = JsonWebToken.Decode(Utils.GetCookie("lc_token"), "token");

                    var payloadData = jsonSerializer.Deserialize<Dictionary<string, object>>(payloadJson);
                    payloadData.TryGetValue("exp", out object expiration);
                    var validTo = FromUnixTime(long.Parse(expiration.ToString()));
                    if (DateTime.Compare(validTo, DateTime.UtcNow) <= 0)
                    {
                        return false;
                    }
                    return true;
                }
                return false;
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }

        private static DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }
    }
}