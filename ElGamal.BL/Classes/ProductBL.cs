using ElGamal.BL.Interfaces;
using ElGamal.DAL.DTOs;
using ElGamal.DAL.Entities;
using ElGamal.DAL.UOF;
using LawFirm.CommonUtilitis.Logging;
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
                ErrorLogger.LogDebug(ex.Message);
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
                    rate = 0,
                    priceAfter = item.priceAfter,
                    Images = item.images.Select(x => new Image() { imageUrl = x.imageUrl, ID = Guid.NewGuid() }).ToList(),
                    productOptions = item.productOptions
                };

                this.iUnitOfWork.ProductRepository.Insert(currentProduct);
                this.iUnitOfWork.Save();
                return 0;

            }
            catch (Exception ex)
            {
                ErrorLogger.LogDebug(ex.Message);
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
                    productOptions = x.productOptions,
                    rate = (x.rate == null) ? 0 : (int?)x.rate,
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
                ErrorLogger.LogDebug(ex.Message);
                return null;
            }
        }

        public int EditProduct(ProductDTO item)
        {
            try
            {
                ErrorLogger.LogDebug(item.productOptions);
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
                oldItem.productOptions = item.productOptions;

                this.iUnitOfWork.ProductRepository.Update(oldItem);
                this.iUnitOfWork.Save();

                return 0;
            }
            catch (Exception ex)
            {
                ErrorLogger.LogDebug(ex.Message);
                return -1;
            }
        }

        public List<ProductDTO> GetAllOffers()
        {
            try
            {

                List<ProductDTO> offers = this.iUnitOfWork.ProductRepository.Get(y => y.priceBefore != null && y.priceAfter != null).Select(x => new ProductDTO()
                {
                    ID = x.ID,
                    description = x.description,
                    categoryID = x.categoryID,
                    parentCategoryName = x.Category.name,
                    name = x.name,
                    priceAfter = x.priceAfter,
                    priceBefore = x.priceBefore,
                    discountPercentage = String.Format("{0:0.00}", (100 - ((x.priceAfter * 100) / x.priceBefore))) + " %",
                    rate = (x.rate == null) ? 0 : (int?)x.rate,
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
                ErrorLogger.LogDebug(ex.Message);
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
                catch (Exception ex)
                {
                    ErrorLogger.LogDebug(ex.Message);
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

        public ProductDetailsDTO GetProductById(Guid productID)
        {
            ProductDetailsDTO item = new ProductDetailsDTO();
            try
            {
                Product currentProduct = this.iUnitOfWork.ProductRepository.GetByID(productID);
                if (currentProduct != null)
                {
                    item.CurrentProduct = new ProductDTO()
                    {
                        ID = currentProduct.ID,
                        description = currentProduct.description,
                        categoryID = currentProduct.categoryID,
                        parentCategoryName = currentProduct.Category.name,
                        name = currentProduct.name,
                        priceAfter = currentProduct.priceAfter,
                        priceBefore = currentProduct.priceBefore,
                        discountPercentage = (currentProduct.priceAfter != null && currentProduct.priceBefore != null) ? String.Format("{0:0.00}", (100 - ((currentProduct.priceAfter * 100) / currentProduct.priceBefore))) + " %" : 0 + " %",
                        productOptions = currentProduct.productOptions,
                        rate = (int)currentProduct.rate,
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

                    List<ProductDTO> relatedProducts = new List<ProductDTO>();
                    relatedProducts = this.GetProductsOFCategory(currentProduct.Category.parentCategoryID.ToString());
                    if(relatedProducts.Count > 0)
                    {
                        item.RelatedProducts = relatedProducts;
                    }

                }           
            }
            catch (Exception ex)
            {
                ErrorLogger.LogDebug(ex.Message);
            }

            return item;
        }

        public int AddComment(CommentDTO item)
        {
            try
            {
                if (item != null)
                {
                    Comment temp = new Comment()
                    {
                        ID = Guid.NewGuid(),
                        commentText = item.commentText,
                        productID = item.productID,
                        ratingValue = item.ratingValue,
                        userID = item.userID
                    };

                    this.iUnitOfWork.CommentRepository.Insert(temp);
                    var allComments = this.iUnitOfWork.CommentRepository.Get(x => x.productID == item.productID);
                    if (allComments != null)
                    {
                        var rate = (allComments.Sum(x => x.ratingValue) + item.ratingValue) / (allComments.Count() + 1);
                        rate = decimal.Ceiling((decimal)rate);
                        var currentProduct = this.iUnitOfWork.ProductRepository.GetByID(item.productID);
                        if (currentProduct != null)
                        {
                            currentProduct.rate = rate;
                            this.iUnitOfWork.ProductRepository.Update(currentProduct);
                        }
                    }

                    this.iUnitOfWork.Save();
                }
                return 0;
            }
            catch (Exception ex)
            {
                ErrorLogger.LogDebug(ex.Message);
                return -1;
            }
        }

        public FilteredProductsDTO GetFilteredProducts(ProductFilterDTO filter)
        {
            try
            {
                FilteredProductsDTO fPDTO = new FilteredProductsDTO();
                List<ProductDTO> items = new List<ProductDTO>();

                if (filter.CategoryID != null)
                {
                    items = mapObjectProducts(this.iUnitOfWork.ProductRepository.Get(x => x.categoryID.ToString() == filter.CategoryID).ToList());
                    var productsOfChildCategories = GetProductsOFCategory(filter.CategoryID);
                    items.AddRange(productsOfChildCategories);
                }

                if (filter.CategoriesIDs != null)
                {
                    if (filter.CategoriesIDs.Count > 0)
                    {
                        if (items.Count == 0)
                        {
                            items = mapObjectProducts(this.iUnitOfWork.ProductRepository.Get(x => filter.CategoriesIDs.Contains(x.categoryID.ToString())).ToList());
                        }
                        else
                        {
                            items = items.Where(x => filter.CategoriesIDs.Contains(x.categoryID.ToString())).ToList();
                        }
                    }

                }

                if (filter.NamePart != null && filter.NamePart != string.Empty)
                {
                    if (items.Count == 0)
                    {
                        items = mapObjectProducts(this.iUnitOfWork.ProductRepository.Get(x => x.name.ToLower().Contains(filter.NamePart.ToLower())).ToList());
                    }
                    else
                    {
                        items = items.Where(x => x.name.ToLower().Contains(filter.NamePart.ToLower())).ToList();
                    }
                }


                if (filter.PriceFrom != null && filter.PriceTO != null)
                {
                    if (items.Count == 0)
                    {
                        items = mapObjectProducts(this.iUnitOfWork.ProductRepository.Get(x => x.priceAfter <= filter.PriceTO && x.priceAfter >= filter.PriceFrom).ToList());
                    }
                    else
                    {
                        items = items.Where(x => x.priceAfter <= filter.PriceTO && x.priceAfter >= filter.PriceFrom).ToList();
                    }
                }


                // this condition executed if there is no filteration
                if (items.Count == 0 && filter.PriceFrom == null && filter.PriceTO == null && filter.NamePart == null && filter.CategoryID == null && filter.CategoriesIDs == null)
                {
                    items = mapObjectProducts(this.iUnitOfWork.ProductRepository.Get().ToList().Take(20).ToList());
                }

                if (filter.SortingType != null)
                {
                    if (items.Count != 0)
                    {
                        if (filter.SortingType == 1) // price from high to low
                        {
                            ProductPriceFromHighToLow pPFHTL = new ProductPriceFromHighToLow();
                            items.Sort(pPFHTL);
                        }

                        if (filter.SortingType == 2) // price from low to high
                        {
                            ProductPriceFromLowToHigh pPFLTH = new ProductPriceFromLowToHigh();
                            items.Sort(pPFLTH);
                        }

                        if (filter.SortingType == 3) // rate from high to low
                        {
                            ProductRateFromHighToLow pRHTL = new ProductRateFromHighToLow();
                            items.Sort(pRHTL);
                        }
                    }
                }


                if (items.Count > 0)
                {
                    fPDTO.Brands = items.Select(x => new CategoryDTO()
                    {
                        ID = Guid.Parse(x.categoryID.ToString()),
                        name = x.parentCategoryName
                    }).Distinct(new CategoryEqualityComparer()).ToList();

                    fPDTO.NumberOfAllItems = items.Count();
                    fPDTO.NumerOfPages = (int)(decimal.Ceiling((decimal)fPDTO.NumberOfAllItems / 20));
                    fPDTO.MaxPriceValue = (int)items.Max(x => x.priceAfter).Value + 100;

                    // this filteration by page number
                    if (filter.PageNumber == null || filter.PageNumber == 1)
                    {
                        items = items.Take(20).ToList();
                    }
                    else
                    {
                        items = items.Skip((int)filter.PageNumber * 20).Take(20).ToList();
                    }
                }

                fPDTO.NumberOfCurrentItems = items.Count();
                fPDTO.Products = items;
                return fPDTO;
            }
            catch (Exception ex)
            {
                ErrorLogger.LogDebug(ex.Message);
                return null;
            }
        }

        public int MakeProductFavourite(Guid productID, Guid userID)
        {
            try
            {
                if (productID != null && userID != null)
                {
                    var item = this.iUnitOfWork.FavouriteRepository.Get(x => x.productID == productID && x.userID == userID);

                    if (item.Count() > 0)
                    {
                        return 2;
                    }
                    else
                    {
                        this.iUnitOfWork.FavouriteRepository.Insert(new Favourite()
                        {
                            ID = Guid.NewGuid(),
                            productID = productID,
                            userID = userID
                        });
                        this.iUnitOfWork.Save();
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogDebug(ex.Message);
                return -1;
            }
            return 0;
        }

        public List<ProductDTO> GetAllFavourites(Guid userID)
        {
            try
            {
                var items = this.iUnitOfWork.FavouriteRepository.Get(x => x.userID == userID);
                List<ProductDTO> favourites = items.Join(this.iUnitOfWork.ProductRepository.Get(), y => y.productID, x => x.ID, (y, x) => new ProductDTO()
                {
                    ID = x.ID,
                    description = x.description,
                    categoryID = x.categoryID,
                    parentCategoryName = x.Category.name,
                    name = x.name,
                    priceAfter = x.priceAfter,
                    priceBefore = x.priceBefore,
                    discountPercentage = String.Format("{0:0.00}", (100 - ((x.priceAfter * 100) / x.priceBefore))) + " %",
                    rate = (x.rate == null) ? 0 : (int?)x.rate,
                    images = x.Images.Select(i => new ImageDTO()
                    {
                        ID = i.ID,
                        imageUrl = WebConfigurationManager.AppSettings["WebApiUrl"].ToString() + i.imageUrl,
                        productID = x.ID

                    }).ToList(),
                }).ToList();


                return favourites;
            }
            catch (Exception ex)
            {
                ErrorLogger.LogDebug(ex.Message);
                return null;
            }
        }

        private List<ProductDTO> GetProductsOFCategory(string CategoryID)
        {
            List<string> Categories = new List<string>();
            List<ProductDTO> products = new List<ProductDTO>();
            try
            {
                Categories = this.iUnitOfWork.CategoryRepository.Get(x => x.parentCategoryID.ToString() == CategoryID).Select(z => z.ID.ToString()).ToList();
                if (Categories.Count != 0)
                {
                    foreach (var item in Categories)
                    {
                        var temp = (this.iUnitOfWork.ProductRepository.Get(x => x.categoryID.ToString() == item) != null) ? this.iUnitOfWork.ProductRepository.Get(x => x.categoryID.ToString() == item).ToList() : new List<Product>();
                        if (temp.Count > 0)
                        {
                            products.AddRange(mapObjectProducts(temp));
                        }
                        GetProductsOFCategory(item);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogDebug(ex.Message);
            }
            return products;
        }

        private List<ProductDTO> mapObjectProducts(List<Product> items)
        {
            try
            {
                if (items != null)
                {
                    if (items.Count != 0)
                    {
                        List<ProductDTO> products = items.Select(x => new ProductDTO()
                        {
                            ID = x.ID,
                            description = x.description,
                            categoryID = x.categoryID,
                            parentCategoryName = x.Category.name,
                            name = x.name,
                            priceAfter = x.priceAfter,
                            priceBefore = x.priceBefore,
                            productOptions = x.productOptions,
                            rate = (int?)x.rate,
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
                    else
                    {
                        return new List<ProductDTO>();
                    }
                }
                else
                {
                    return new List<ProductDTO>();
                }

            }
            catch (Exception ex)
            {
                ErrorLogger.LogDebug(ex.Message);
                return new List<ProductDTO>();
            }
        }


        private class ProductPriceFromHighToLow : IComparer<ProductDTO>
        {
            public int Compare(ProductDTO x, ProductDTO y)
            {
                return -1 * ((decimal)x.priceAfter).CompareTo((decimal)y.priceAfter);
            }
        }
        private class ProductPriceFromLowToHigh : IComparer<ProductDTO>
        {
            public int Compare(ProductDTO x, ProductDTO y)
            {
                return ((decimal)x.priceAfter).CompareTo((decimal)y.priceAfter);
            }
        }
        private class ProductRateFromHighToLow : IComparer<ProductDTO>
        {
            public int Compare(ProductDTO x, ProductDTO y)
            {
                return -1 * ((decimal)x.rate).CompareTo((decimal)y.rate);
            }
        }
        private class CategoryEqualityComparer : IEqualityComparer<CategoryDTO>
        {
            public bool Equals(CategoryDTO x, CategoryDTO y)
            {
                return x.ID == y.ID;
            }

            public int GetHashCode(CategoryDTO obj)
            {
                return obj.ID.GetHashCode();
            }
        }

    }



}
