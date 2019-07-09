using ElGamal.BL.Interfaces;
using ElGamal.DAL.Context;
using ElGamal.DAL.DTOs;
using LawFirm.CommonUtilitis.Logging;
using System;
using System.Web.Http;

namespace ElGamal.API.Controllers
{
    public class ProductController : ApiController
    {
        private IProductBL iProductBL;
        private ElGamalContext db = new ElGamalContext();

        public ProductController(IProductBL iPBL)
        {
            this.iProductBL = iPBL;
        }

        [HttpPost]
        [Route("Product/CheckIfProductExits/")]
        public IHttpActionResult CheckIfProductExits(CheckProductExistDTO item)
        {
            try
            {
                return Ok(this.iProductBL.CheckIfProductExits(item.ProductName));
            }
            catch (Exception exp)
            {
                ErrorLogger.LogDebug(exp.Message);
                return null; 
            }

        }

        [HttpPost]
        [Route("Product/AddProduct")]
        public IHttpActionResult AddProduct(ProductDTO item)
        {
            try
            {
                return Ok(this.iProductBL.AddProduct(item));
            }
            catch (Exception exp)
            {
                ErrorLogger.LogDebug(exp.Message);
                return null;
            }
        }


        [HttpGet]
        [Route("Product/GetAllProducts/")]
        public IHttpActionResult GetAllProducts()
        {
            try
            {
                ErrorLogger.LogDebug("ssssss");
                return Ok(this.iProductBL.GetAllProducts());
            }
            catch (Exception exp)
            {
                ErrorLogger.LogDebug(exp.Message);
                return null;
            }

        }

        [HttpGet]
        [Route("Product/GetProductById/{item}")]
        public IHttpActionResult GetProductById(Guid item)
        {
            try
            {
                return Ok(this.iProductBL.GetProductById(item));
            }
            catch (Exception exp)
            {
                ErrorLogger.LogDebug(exp.Message);
                return null;
            }

        }


        [HttpGet]
        [Route("Product/GetAllOffers/")]
        public IHttpActionResult GetAllOffers()
        {
            try
            {
                return Ok(this.iProductBL.GetAllOffers());
            }
            catch (Exception exp)
            {
                ErrorLogger.LogDebug(exp.Message);
                return null;
            }

        }


        [HttpPost]
        [Route("Product/EditProduct")]
        public IHttpActionResult EditProduct(ProductDTO item)
        {
            try
            {
                return Ok(this.iProductBL.EditProduct(item));
            }
            catch (Exception exp)
            {
                ErrorLogger.LogDebug(exp.Message);
                return null;
            }
        }


        [HttpPost]
        [Route("Product/DeleteProduct")]
        public IHttpActionResult deleteProduct(DeleteProductDTO item)
        {
            try
            {
                return Ok(this.iProductBL.deleteProduct(item));
            }
            catch (Exception exp)
            {
                ErrorLogger.LogDebug(exp.Message);
                return null;
            }
        }

        [HttpPost]
        [Route("Product/GetFilteredProducts")]
        public IHttpActionResult CheckIfProductExits(ProductFilterDTO item)
        {
            try
            {
                return Ok(this.iProductBL.GetFilteredProducts(item));
            }
            catch (Exception exp)
            {
                ErrorLogger.LogDebug(exp.Message);
                return null;
            }

        }

        [HttpPost]
        [Route("Product/AddComment")]
        public IHttpActionResult AddComment(CommentDTO item)
        {
            try
            {
                return Ok(this.iProductBL.AddComment(item));
            }
            catch (Exception exp)
            {
                ErrorLogger.LogDebug(exp.Message);
                return null;
            }
        }


    }
}
