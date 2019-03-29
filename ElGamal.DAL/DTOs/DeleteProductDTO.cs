using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElGamal.DAL.DTOs
{
    public class DeleteProductDTO
    {
        public Guid productID { get; set; }

        public List<String> images { get; set; }
    }
}
