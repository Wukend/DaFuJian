using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Dafujian.Authentication.Controllers
{
    public class AccountController : ApiController
    {
        //[HttpGet]
        //[Route("account/profile")]
        //[Authorize]
        //public async Task<IHttpActionResult> Profile()
        //{
        //    var userId = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Scheme); //decode the token

        //    var user = await _uow.Repository<User>().GetAsync(u => u.Id == userId); //Get the user by id

        //    return Ok(user);
        //}
    }
}
