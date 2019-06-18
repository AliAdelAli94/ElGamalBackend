using ElGamal.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElGamal.DAL.DTOs
{
    public class OrderDetailsDTO
    {
        public Guid ID { get; set; }

        public decimal? total { get; set; }

        public decimal? shippingAmount { get; set; }

        public Guid? userID { get; set; }

        public string userName { get; set; }

        public bool status { get; set; }

        public List<OrderProductDTO> OrderDetails { get; set; }

    }
}
