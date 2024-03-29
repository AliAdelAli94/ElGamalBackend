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
        
        List<ProductDTO> GetAllProducts();

        int EditProduct(ProductDTO item);

        List<ProductDTO> GetAllOffers();

        int deleteProduct(DeleteProductDTO data);

        ProductDetailsDTO GetProductById(Guid productID);

        FilteredProductsDTO GetFilteredProducts(ProductFilterDTO filter);

        int AddComment(CommentDTO item);

        int MakeProductFavourite(Guid productID, Guid userID);

        List<ProductDTO> GetAllFavourites(Guid userID);
               


    }
}
