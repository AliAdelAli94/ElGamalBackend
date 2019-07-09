using ElGamal.BL.Interfaces;
using ElGamal.BL.Classes;
using ElGamal.DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LawFirm.CommonUtilitis.Logging;

namespace ElGamal.API.Controllers
{
    public class OrderController : ApiController
    {
        private IOrderBL iOrderBL;
        public OrderController(IOrderBL iOBL)
        {
            this.iOrderBL = iOBL;
        }

        [HttpPost]
        [Route("Order/MakeOrder")]
        public IHttpActionResult MakeOrder(OrderDTO item)
        {
            try
            {
                return Ok(this.iOrderBL.MakeOrder(item));
            }
            catch (Exception exp)
            {
                ErrorLogger.LogDebug(exp.Message);
                return null;
            }
        }

        [HttpGet]
        [Route("Order/GetOrdersByStatus")]
        public IHttpActionResult GetOrdersByStatus()
        {
            try
            {
                return Ok(this.iOrderBL.GetOrdersByStatus());
            }
            catch (Exception exp)
            {
                ErrorLogger.LogDebug(exp.Message);
                return null;
            }
        }

        [HttpGet]
        [Route("Order/GetOrderDetailsByID")]
        public IHttpActionResult GetOrderDetailsByID(Guid id)
        {
            try
            {
                return Ok(this.iOrderBL.GetOrderDetailsByID(id));
            }
            catch (Exception exp)
            {
                ErrorLogger.LogDebug(exp.Message);
                return null;
            }
        }

        [HttpPost]
        [Route("Order/ConfirmOrder")]
        public IHttpActionResult ConfirmOrder(Guid id)
        {
            try
            {
                return Ok(this.iOrderBL.ConfirmOrder(id));
            }
            catch (Exception exp)
            {
                ErrorLogger.LogDebug(exp.Message);
                return null;
            }
        }
    }
}
