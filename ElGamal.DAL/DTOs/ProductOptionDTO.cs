using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElGamal.DAL.DTOs
{
    public class ProductOptionDTO
    {
        public Guid ID { get; set; }

        public string optionText { get; set; }

        public Guid? productID { get; set; }

    }
}
