using ElGamal.BL.Interfaces;
using ElGamal.DAL.DTOs;
using ElGamal.DAL.Entities;
using ElGamal.DAL.UOF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElGamal.BL.Classes
{
    public class ProductBL : IProductBL
    {
        private IUnitOfWork iUnitOfWork;
        public ProductBL(IUnitOfWork iUOF)
        {
            this.iUnitOfWork = iUOF;
        }

        public int CheckIfProductExits(string name)
        {
            try
            {
                List<Product> products = this.iUnitOfWork.ProductRepository.Get(x => x.name == name).ToList();
                return products.Count > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public int AddProduct(ProductDTO item)
        {
            try
            {
                Product currentProduct = new Product()
                {
                    ID = Guid.NewGuid(),
                    categoryID = item.categoryID,
                    description = item.description,
                    name = item.name,
                    priceBefore = item.priceBefore,
                    priceAfter = item.priceAfter,
                    Images = item.images.Select(x => new Image() { imageUrl = x.imageUrl,ID = Guid.NewGuid() }).ToList(),
                    ProductOptions = item.ProductOptions.Select(x => new ProductOption() { optionText = x.optionText, ID = Guid.NewGuid()}).ToList()
                };

                this.iUnitOfWork.ProductRepository.Insert(currentProduct);
                this.iUnitOfWork.Save();
                return 0;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}
