using ElGamal.DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElGamal.BL.Interfaces
{
    public interface ICategoryBL
    {
        int AddCategory(CategoryDTO item);

        List<CategoryDTO> GetAllCategories();

        List<ParentCategoryDto> GetParentCategories();

        List<CategoryDTO2> GetAllCategoriesDashboard();

        int DeleteCategory(Guid id);
        int EditCategory(CategoryDTO item);


    }
}
