using JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;

namespace Dafujian.Authentication
{
    /// <summary>
    /// A class to decode the authentication token 
    /// </summary>
    public class JwtDecoder
    {
        /// <summary>
        /// Get the userid from the token if the token is not expired
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static int? GetUserIdFromToken(string token)
        {
            string key = WebConfigurationManager.AppSettings.Get("jwtKey");
            var jsonSerializer = new JavaScriptSerializer();
            var decodedToken = JsonWebToken.Decode(token, key);
            var data = jsonSerializer.Deserialize<Dictionary<string, object>>(decodedToken);
            data.TryGetValue("userId", out object userId);
            data.TryGetValue("exp", out object exp);
            var validTo = FromUnixTime(long.Parse(exp.ToString()));
            if (DateTime.Compare(validTo, DateTime.UtcNow) <= 0)
            {
                return null;
            }
            return (int)userId;
        }

        private static DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }
    }
}