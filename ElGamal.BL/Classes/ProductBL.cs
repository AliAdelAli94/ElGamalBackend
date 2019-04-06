using ElGamal.BL.Interfaces;
using ElGamal.DAL.DTOs;
using ElGamal.DAL.Entities;
using ElGamal.DAL.UOF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
                foreach (var val in oldItem.Images.ToList())
                {

                    if (item.images.Where(h => h.ID == val.ID).Count() == 0)
                    {
                        this.iUnitOfWork.ImageRepository.Delete(val);
                    }

                }

                foreach (var val in item.images)
                {
                    if (oldItem.Images.Where(x => x.ID == val.ID).Count() == 0)
                    {
                        this.iUnitOfWork.ImageRepository.Insert(new Image() { ID = Guid.NewGuid(), imageUrl = val.imageUrl, productID = oldItem.ID });
                    }
                }
                item.images.Clear();

                // end edit images

                // product options edit

                var tempProductOption = new List<ProductOptionDTO>();
                foreach (var val in oldItem.ProductOptions.ToList())
                {
                    iUnitOfWork.ProductOptionRepository.DetachEntity(val);
                    tempProductOption = item.ProductOptions.Where(x => x.ID == val.ID).ToList();
                    if (tempProductOption.Count() == 0)
                    {
                        this.iUnitOfWork.ProductOptionRepository.Delete(val.ID);
                    }
                    else
                    {
                        this.iUnitOfWork.ProductOptionRepository.Update(new ProductOption() { ID = tempProductOption.FirstOrDefault().ID, optionText = tempProductOption.FirstOrDefault().optionText, productID = val.productID });
                    }
                }

                foreach (var val in item.ProductOptions)
                {
                    if (oldItem.ProductOptions.Where(x => x.ID == val.ID).Count() == 0)
                    {
                        this.iUnitOfWork.ProductOptionRepository.Insert(new ProductOption() { ID = Guid.NewGuid(), optionText = val.optionText, productID = oldItem.ID });
                    }
                }
                item.ProductOptions.Clear();

                // end product options edit

                foreach (var val in oldItem.Comments.ToList())
                {

                    if (item.Comments.Where(x => x.ID == val.ID).Count() > 0)
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

                List<ProductOfferDTO> offers = this.iUnitOfWork.ProductRepository.Get(y => y.priceBefore != null && y.priceAfter != null).Select(x => new ProductOfferDTO()
                {
                    ID = x.ID,
                    description = x.description,
                    categoryID = x.categoryID,
                    parentCategoryName = x.Category.name,
                    name = x.name,
                    priceAfter = x.priceAfter,
                    priceBefore = x.priceBefore,
                    discountPercentage = String.Format("{0:0.00}", (100 - ((x.priceAfter * 100) / x.priceBefore))) + " %" ,
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

        public int deleteProduct(DeleteProductDTO data)
        {
            try
            {
                try
                {
                    List<string> imagesUrls = data.images;
                    if (imagesUrls != null)
                    {
                        string oldProp = string.Empty;
                        foreach (var item in imagesUrls)
                        {
                            var url = new Uri(item);
                            oldProp = '~' + url.LocalPath;
                            oldProp = HttpContext.Current.Server.MapPath(oldProp);
                            if (File.Exists(oldProp))
                                File.Delete(oldProp);
                        }
                    }
                }
                catch(Exception ex)
                {

                }
                this.iUnitOfWork.ProductRepository.Delete(data.productID);
                this.iUnitOfWork.Save();
                return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public ProductDTO GetProductById(Guid productID)
        {
            try
            {
                Product currentProduct = this.iUnitOfWork.ProductRepository.GetByID(productID);
                if(currentProduct != null)
                {
                    return new ProductDTO()
                    {
                        ID = currentProduct.ID,
                        description = currentProduct.description,
                        categoryID = currentProduct.categoryID,
                        parentCategoryName = currentProduct.Category.name,
                        name = currentProduct.name,
                        priceAfter = currentProduct.priceAfter,
                        priceBefore = currentProduct.priceBefore,
                        discountPercentage = (currentProduct.priceAfter != null && currentProduct.priceBefore != null)?String.Format("{0:0.00}", (100 - ((currentProduct.priceAfter * 100) / currentProduct.priceBefore))) + " %" : 0 + " %",
                        ProductOptions = currentProduct.ProductOptions.Select(p => new ProductOptionDTO()
                        {
                            ID = p.ID,
                            optionText = p.optionText,
                            productID = currentProduct.ID

                        }).ToList(),
                        rate = currentProduct.rate,
                        images = currentProduct.Images.Select(i => new ImageDTO()
                        {
                            ID = i.ID,
                            imageUrl = WebConfigurationManager.AppSettings["WebApiUrl"].ToString() + i.imageUrl,
                            productID = currentProduct.ID

                        }).ToList(),

                        Comments = currentProduct.Comments.Select(c => new CommentDTO()
                        {
                            ID = c.ID,
                            commentText = c.commentText,
                            ratingValue = c.ratingValue,
                            userID = c.userID,
                            userName = c.User.userName,
                            productID = currentProduct.ID
                        }).ToList()
                    };
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }



    }
}
