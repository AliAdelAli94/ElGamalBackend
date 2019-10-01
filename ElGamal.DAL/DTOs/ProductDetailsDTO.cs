using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElGamal.DAL.DTOs
{
    public class ProductDetailsDTO
    {
        public ProductDTO CurrentProduct { get; set; }

        public List<ProductDTO> RelatedProducts { get; set; }
    }
}
