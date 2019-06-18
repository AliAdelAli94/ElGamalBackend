using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElGamal.DAL.DTOs
{
    public class OrderProductDTO
    {
        public decimal? buyingPrice { get; set; }

        public int? quantity { get; set; }

        public Guid? productID { get; set; }

        public string name { get; set; }

        public string description { get; set; }
    }
}
