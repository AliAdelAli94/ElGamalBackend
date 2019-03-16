using ElGamal.BL.Interfaces;
using ElGamal.DAL.DTOs;
using ElGamal.DAL.Entities;
using ElGamal.DAL.UOF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElGamal.BL.Classes
{
    public class CategoryBL : ICategoryBL
    {
        private IUnitOfWork iUnitOfWork;
        public CategoryBL(IUnitOfWork iUOF)
        {
            this.iUnitOfWork = iUOF;
        }

        public int AddCategory(CategoryDTO item)
        {
            try
            {
                if (this.iUnitOfWork.CategoryRepository.Get(x => x.name == item.name && x.parentCategoryID == item.parentCategoryID).Count() > 0)
                    return 1;
                Category category = new Category();
                category.ID = Guid.NewGuid();
                category.name = item.name;
                category.parentCategoryID = item.parentCategoryID;
                this.iUnitOfWork.CategoryRepository.Insert(category);
                this.iUnitOfWork.Save();
                return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<CategoryDTO> GetAllCategories()
        {
            try
            {
                return this.iUnitOfWork.CategoryRepository.Get().Select(x => new CategoryDTO()
                {
                    ID = x.ID,
                    name = x.name,
                    parentCategoryID = x.parentCategoryID
                }).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ParentCategoryDto> GetParentCategories()
        {
            try
            {
                return this.iUnitOfWork.CategoryRepository.Get().Select(x => new ParentCategoryDto()
                {
                    ID = x.ID,
                    name = x.name,
                    parentCategoryID = x.parentCategoryID,
                    childCategories = x.Categories1.Select(z => new CategoryDTO()
                    {
                        ID = z.ID,
                        name = z.name,
                        parentCategoryID = z.parentCategoryID
                    }).ToList()
                }).Where(x => x.parentCategoryID == null).ToList();
            }
            catch (Exception ex)
            {
                return null;
                throw;
            }
        }
    }


}
