﻿using ElGamal.DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElGamal.BL.Interfaces
{
    public interface IProductBL
    {
        int CheckIfProductExits(string name);
        int AddProduct(ProductDTO item);


    }
}
