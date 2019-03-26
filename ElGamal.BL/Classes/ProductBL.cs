using ElGamal.BL.Interfaces;
using ElGamal.DAL.DTOs;
using ElGamal.DAL.Entities;
using ElGamal.DAL.UOF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

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
                    Images = item.images.Select(x => new Image() { imageUrl = x.imageUrl, ID = Guid.NewGuid() }).ToList(),
                    ProductOptions = item.ProductOptions.Select(x => new ProductOption() { optionText = x.optionText, ID = Guid.NewGuid() }).ToList()
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

        public List<ProductDTO> GetAllProducts()
        {
            try
            {
                List<ProductDTO> products = this.iUnitOfWork.ProductRepository.Get().Select(x => new ProductDTO()
                {
                    ID = x.ID,
                    description = x.description,
                    categoryID = x.categoryID,
                    parentCategoryName = x.Category.name,
                    name = x.name,
                    priceAfter = x.priceAfter,
                    priceBefore = x.priceBefore,
                    ProductOptions = x.ProductOptions.Select(p => new ProductOptionDTO()
                    {
                        ID = p.ID,
                        optionText = p.optionText,
                        productID = x.ID

                    }).ToList(),
                    rate = x.rate,
                    images = x.Images.Select(i => new ImageDTO()
                    {
                        ID = i.ID,
                        imageUrl = WebConfigurationManager.AppSettings["WebApiUrl"].ToString() + i.imageUrl,
                        productID = x.ID

                    }).ToList(),

                    Comments = x.Comments.Select(c => new CommentDTO()
                    {
                        ID = c.ID,
                        commentText = c.commentText,
                        ratingValue = c.ratingValue,
                        userID = c.userID,
                        userName = c.User.userName,
                        productID = x.ID
                    }).ToList()
                }).ToList();

                return products;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int EditProduct(ProductDTO item)
        {
            try
            {
                Product oldItem = this.iUnitOfWork.ProductRepository.GetByID(item.ID);

                // start edit images
                foreach (var val in item.images)
                {
                    if (val.ID == Guid.Empty)
                    {
                        this.iUnitOfWork.ImageRepository.Insert(new Image() { ID = Guid.NewGuid(), imageUrl = val.imageUrl, productID = oldItem.ID });
                    }
                }

                foreach (var val in oldItem.Images.ToList())
                {

                    if (!(item.images.Contains(new ImageDTO() { ID = val.ID, imageUrl = val.imageUrl })))
                    {
                        this.iUnitOfWork.ImageRepository.Delete(val);
                    }

                }
                item.images.Clear();

                // end edit images

                // product options edit
                foreach (var val in item.ProductOptions)
                {
                    if (val.ID == Guid.Empty)
                    {
                        this.iUnitOfWork.ProductOptionRepository.Insert(new ProductOption() { ID = Guid.NewGuid(), optionText = val.optionText, productID = oldItem.ID });
                    }
                }

                foreach (var val in oldItem.ProductOptions.ToList())
                {
                    if (!(item.ProductOptions.Where(x => x.ID == val.ID && x.optionText == val.optionText).Count() > 0))
                    {
                        this.iUnitOfWork.ProductOptionRepository.Delete(val.ID);
                    }
                    else
                    {
                        this.iUnitOfWork.ProductOptionRepository.Update(new ProductOption() { ID = val.ID, optionText = val.optionText, productID = val.productID });
                    }
                }
                item.ProductOptions.Clear();

                // end product options edit

                foreach (var val in oldItem.Comments.ToList())
                {

                    if (!(item.Comments.Contains(new CommentDTO() { ID = val.ID, commentText = val.commentText })))
                    {
                        this.iUnitOfWork.CommentRepository.Delete(val.ID);
                    }

                }

                item.Comments.Clear();

                oldItem.ID = item.ID;
                oldItem.categoryID = item.categoryID;
                oldItem.description = item.description;
                oldItem.name = item.name;
                oldItem.priceAfter = item.priceAfter;
                oldItem.priceBefore = item.priceBefore;

                this.iUnitOfWork.ProductRepository.Update(oldItem);
                this.iUnitOfWork.Save();

                return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<ProductOfferDTO> GetAllOffers()
        {
            try
            {

                List<ProductOfferDTO> offers = this.iUnitOfWork.ProductRepository.Get( y => y.priceBefore != null && y.priceAfter != null).Select(x => new ProductOfferDTO()
                {
                    ID = x.ID,
                    description = x.description,
                    categoryID = x.categoryID,
                    parentCategoryName = x.Category.name,
                    name = x.name,
                    priceAfter = x.priceAfter,
                    priceBefore = x.priceBefore,

                    rate = x.rate,
                    images = x.Images.Select(i => new ImageDTO()
                    {
                        ID = i.ID,
                        imageUrl = WebConfigurationManager.AppSettings["WebApiUrl"].ToString() + i.imageUrl,
                        productID = x.ID

                    }).ToList()

                }).ToList();

                return offers;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
