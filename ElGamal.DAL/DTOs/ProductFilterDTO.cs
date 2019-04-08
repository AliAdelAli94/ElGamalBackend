using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElGamal.DAL.DTOs
{
    public class ProductFilterDTO
    {
        public string CategoryID { get; set; }

        public decimal? PriceFrom { get; set; }

        public decimal? PriceTO { get; set; }

        public List<string> CategoriesIDs { get; set; }

        public int? SortingType { get; set; }

        public string NamePart { get; set; }

        public int? PageNumber { get; set; }

    }
}
