using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElGamal.DAL.DTOs
{
    public class FilteredProductsDTO
    {
        public List<ProductDTO> Products { get; set; }

        public decimal NumberOfAllItems { get; set; }

        public int NumerOfPages { get; set; }

        public int NumberOfCurrentItems { get; set; }

        public List<CategoryDTO> Brands { get; set; }

    }
}
