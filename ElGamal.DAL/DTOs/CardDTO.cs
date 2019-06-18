using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElGamal.DAL.DTOs
{
    public class CardDTO
    {
        public List<ProductDTO> selectedProducts { get; set; }

        public decimal productsPrice { get; set; }

        public decimal shipingPrice { get; set; }

        public decimal totalPrice { get; set; }
    }
}
