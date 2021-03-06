﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElGamal.DAL.DTOs
{
    public class RegisterDTO
    {
        public Guid ID { get; set; }

        public string userName { get; set; }

        public string email { get; set; }

        public string password { get; set; }

        public string phoneNumber { get; set; }

        public string address { get; set; }

        public string role { get; set; }

        public DateTime? birthDate { get; set; }

    }
}
