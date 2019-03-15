using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElGamal.DAL.DTOs
{
    public class ImageDTO
    {
        public Guid ID { get; set; }

        public string imageUrl { get; set; }

        public Guid? productID { get; set; }

    }
}
