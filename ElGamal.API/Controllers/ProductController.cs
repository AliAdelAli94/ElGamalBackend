using ElGamal.BL.Interfaces;
using ElGamal.DAL.Context;
using ElGamal.DAL.DTOs;
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

        [HttpGet]
        [Route("Product/CheckIfProductExits/{productName}")]
        public IHttpActionResult CheckIfProductExits(string productName)
        {
            try
            {
                return Ok(this.iProductBL.CheckIfProductExits(productName));
            }
            catch (Exception exp)
            {
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
                return null;
            }
        }


        [HttpGet]
        [Route("Product/GetAllProducts/")]
        public IHttpActionResult GetAllProducts()
        {
            try
            {
                return Ok(this.iProductBL.GetAllProducts());
            }
            catch (Exception exp)
            {
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
                return null;
            }
        }

    }
}
