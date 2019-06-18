using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElGamal.DAL.DTOs
{
    public class OrderDTO
    {
        public CardDTO cartData { get; set; }

        public string userID { get; set; }

        public string phone { get; set; }

        public string address { get; set; }
    }
}
