using ElGamal.BL.Interfaces;
using ElGamal.DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ElGamal.API.Controllers
{
    public class UserController : ApiController
    {
        private IUserBL iUserBL;
        public UserController(IUserBL iUBL)
        {
            this.iUserBL = iUBL;
        }

        [HttpPost]
        [Route("User/Login")]
        public IHttpActionResult Login(LoginDTO item)
        {
            try
            {
                return Ok(this.iUserBL.Login(item));
            }
            catch (Exception exp)
            {
                return null;
            }
        }

    }
}