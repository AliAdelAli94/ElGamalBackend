using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElGamal.DAL.DTOs
{
    public class LoginDTO
    {
        public string email { get; set; }
        public string password { get; set; }
        public string role { get; set; }
    }
}
