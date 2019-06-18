using ElGamal.BL.Interfaces;
using ElGamal.DAL.DTOs;
using ElGamal.DAL.Entities;
using ElGamal.DAL.UOF;
using LawFirm.CommonUtilitis.Logging;
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
                ErrorLogger.LogDebug(ex.Message);
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
                ErrorLogger.LogDebug(ex.Message);
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
                ErrorLogger.LogDebug(ex.Message);
                return null;
                throw;
            }
        }

        public List<CategoryDTO2> GetAllCategoriesDashboard()
        {
            try
            {
                return this.iUnitOfWork.CategoryRepository.Get().Select(x => new CategoryDTO2()
                {

                    ID = x.ID,
                    name = x.name,
                    parentCategoryID = x.parentCategoryID,
                    parentCategoryName = (x.Category1 != null) ? x.Category1.name : string.Empty,

                }).ToList();
            }
            catch(Exception ex)
            {
                ErrorLogger.LogDebug(ex.Message);
                return null;
            }
        }

        public int DeleteCategory(Guid id)
        {
            try
            {
                DeleteEntityRecursive(id);
                this.iUnitOfWork.Save();
                return 0;
            }
            catch (Exception ex)    
            {
                ErrorLogger.LogDebug(ex.Message);
                return -1;
            }
        }

        private void DeleteEntityRecursive(Guid id)
        {
            try
            {
                List<Category> childCategories = this.iUnitOfWork.CategoryRepository.Get(x => x.parentCategoryID == id).ToList();
                if (childCategories.Count() == 0)
                {
                    this.iUnitOfWork.CategoryRepository.Delete(id);
                }
                else
                {
                    foreach (var item in childCategories)
                    {
                        this.iUnitOfWork.CategoryRepository.DetachEntity(item);
                        DeleteEntityRecursive(item.ID);
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorLogger.LogDebug(ex.Message);
            }
        }

        public int EditCategory(CategoryDTO item)
        {
            try
            {
                Category currentItem = new Category()
                {
                    ID = item.ID,
                    name = item.name,
                    parentCategoryID = item.parentCategoryID
                };
                this.iUnitOfWork.CategoryRepository.Update(currentItem);
                this.iUnitOfWork.Save();
                return 0;

            }
            catch (Exception ex)
            {
                ErrorLogger.LogDebug(ex.Message);
                return -1;
            }
        }

    }

}
