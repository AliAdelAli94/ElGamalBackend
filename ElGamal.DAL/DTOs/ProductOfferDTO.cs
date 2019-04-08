﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElGamal.DAL.DTOs
{
    public class ProductOfferDTO
    {
        public Guid ID { get; set; }

        public string name { get; set; }

        public decimal? priceBefore { get; set; }

        public decimal? priceAfter { get; set; }

        public string description { get; set; }

        public decimal? rate { get; set; }

        public Guid? categoryID { get; set; }

        public string parentCategoryName { get; set; }

        public string discountPercentage { get; set; }

        public List<ImageDTO> images { get; set; }

    }
}