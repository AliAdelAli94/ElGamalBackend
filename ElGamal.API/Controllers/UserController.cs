using ElGamal.BL.Interfaces;
using ElGamal.DAL.DTOs;
using LawFirm.CommonUtilitis.Logging;
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
                ErrorLogger.LogDebug(exp.Message);
                return null;
            }
        }

        [HttpPost]
        [Route("User/RegisterUser")]
        public IHttpActionResult RegisterUser(RegisterDTO item)
        {
            try
            {
                return Ok(this.iUserBL.RegisterUser(item));
            }
            catch (Exception exp)
            {
                ErrorLogger.LogDebug(exp.Message);
                return null;
            }
        }

        [HttpGet]
        [Route("User/GetAllUsers/{role}")]
        public IHttpActionResult GetAllUsers(string role)
        {
            try
            {
                return Ok(this.iUserBL.GetAllUsers(role));
            }
            catch (Exception exp)
            {
                ErrorLogger.LogDebug(exp.Message);
                return null;
            }
        }


        [HttpPost]
        [Route("User/DeleteUser/{item}")]
        public IHttpActionResult DeleteUser(Guid item)
        {
            try
            {
                return Ok(this.iUserBL.DeleteUser(item));
            }
            catch (Exception exp)
            {
                ErrorLogger.LogDebug(exp.Message);
                return null;
            }
        }

        [HttpPost]
        [Route("User/EditUser/")]
        public IHttpActionResult EditUser([FromBody] UserDTO data)
        {
            try
            {
                return Ok(this.iUserBL.EditUser(data));
            }
            catch (Exception exp)
            {
                ErrorLogger.LogDebug(exp.Message);
                return null;
            }
        }



    }
}