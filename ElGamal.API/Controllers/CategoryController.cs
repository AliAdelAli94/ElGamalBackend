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
    public class CategoryController : ApiController
    {
        private ICategoryBL iCategoryBL;
        public CategoryController(ICategoryBL iCB)
        {
            this.iCategoryBL = iCB;
        }


        [HttpGet]
        [Route("Category/GetAllCategories/")]
        public IHttpActionResult GetAllCategories()
        {
            try
            {
                return Ok(this.iCategoryBL.GetAllCategories());
            }
            catch (Exception exp)
            {
                return null;
            }
        }


        [HttpGet]
        [Route("Category/GetParentCategories/")]
        public IHttpActionResult GetParentCategories()
        {
            try
            {
                return Ok(this.iCategoryBL.GetParentCategories());
            }
            catch (Exception exp)
            {
                return null;
            }
        }


        [HttpPost]
        [Route("Category/AddCategory/")]
        public IHttpActionResult AddCategory(CategoryDTO category)
        {
            try
            {
                return Ok(this.iCategoryBL.AddCategory(category));
            }
            catch (Exception exp)
            {
                return Content(HttpStatusCode.InternalServerError,-1);
            }
        }


        [HttpGet]
        [Route("Category/GetAllCategoriesDashboard/")]
        public IHttpActionResult GetAllCategoriesDashboard()
        {
            try
            {
                return Ok(this.iCategoryBL.GetAllCategoriesDashboard());
            }
            catch (Exception exp)
            {
                return null;
            }
        }


        [HttpPost]
        [Route("Category/DeleteCategory/{id}")]
        public IHttpActionResult DeleteCategory(Guid id)
        {
            try
            {
                return Ok(this.iCategoryBL.DeleteCategory(id));
            }
            catch (Exception exp)
            {
                return Content(HttpStatusCode.InternalServerError, -1);
            }
        }


        [HttpPost]
        [Route("Category/EditCategory")]
        public IHttpActionResult EditCategory(CategoryDTO item)
        {
            try
            {
                return Ok(this.iCategoryBL.EditCategory(item));
            }
            catch (Exception exp)
            {
                return Content(HttpStatusCode.InternalServerError, -1);
            }
        }


        

    }
}
