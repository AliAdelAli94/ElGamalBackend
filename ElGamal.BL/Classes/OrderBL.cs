using ElGamal.BL.Interfaces;
using ElGamal.DAL.DTOs;
using ElGamal.DAL.Entities;
using ElGamal.DAL.UOF;
using LawFirm.CommonUtilitis.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElGamal.BL.Classes
{
    public class OrderBL : IOrderBL
    {
        private IUnitOfWork iUnitOfWork;
        public OrderBL(IUnitOfWork iUOF)
        {
            this.iUnitOfWork = iUOF;
        }

        public MakeOrderResultDTO MakeOrder(OrderDTO item)
        {
            MakeOrderResultDTO result = new MakeOrderResultDTO();
            try
            {
                if (item != null)
                {
                    var currentUser = this.iUnitOfWork.UserRepository.GetByID(Guid.Parse(item.userID));
                    if (currentUser != null)
                    {
                        // update user info with new data
                        currentUser.phoneNumber = item.phone;
                        currentUser.address = item.address;
                        this.iUnitOfWork.UserRepository.Update(currentUser);

                        // create order object 
                        Order currentOrder = new Order()
                        {
                            ID = Guid.NewGuid(),
                            shippingAmount = item.cartData.shipingPrice,
                            total = item.cartData.productsPrice,
                            userID = Guid.Parse(item.userID),
                        };


                        List<OrderDetail> OrderDetails = new List<OrderDetail>();
                        foreach (var val in item.cartData.selectedProducts)
                        {
                            OrderDetails.Add(new OrderDetail()
                            {
                                ID = Guid.NewGuid(),
                                productID = val.ID,
                                quantity = val.NumberOfItems,
                                buyingPrice = val.priceAfter
                            });
                        }

                        currentOrder.OrderDetails = OrderDetails;

                        this.iUnitOfWork.OrderRepository.Insert(currentOrder);
                        result.StatusCode = 0;
                        result.OrderID = currentOrder.ID.ToString();
                    }
                    else
                    {
                        result.StatusCode = -2;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogDebug(ex.Message);
                result.StatusCode = -1;
            }
            this.iUnitOfWork.Save();
            return result;
        }

        public List<GetOrderDTO> GetOrdersByStatus(bool status)
        {
            List<GetOrderDTO> result = new List<GetOrderDTO>();
            try
            {
                result = this.iUnitOfWork.OrderRepository.Get(x => x.status == status).Select(x => new GetOrderDTO()
                {
                    ID = x.ID,
                    status = x.status,
                    shippingAmount = x.shippingAmount,
                    userName = x.User.userName,
                    total = x.total
                }).ToList();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogDebug(ex.Message);
            }
            return result;
        }

        public OrderDetailsDTO GetOrderDetailsByID(Guid id)
        {
            OrderDetailsDTO item = new OrderDetailsDTO();
            try
            {
                var temp = this.iUnitOfWork.OrderRepository.GetByID(id);
                if(temp != null)
                {
                    item.ID = temp.ID;
                    item.userName = temp.User.userName;
                    item.userID = temp.userID;
                    item.total = temp.total;
                    item.status = temp.status;
                    item.shippingAmount = temp.shippingAmount;
                    item.OrderDetails = temp.OrderDetails.Select(x => new OrderProductDTO()
                    {
                        buyingPrice = x.buyingPrice,
                        description = x.Product.description,
                        name = x.Product.name,
                        productID = x.productID,
                        quantity = x.quantity
                    }).ToList();
                }
            }
            catch(Exception ex)
            {
                ErrorLogger.LogDebug(ex.Message);
            }
            return item;
        }

    }
}
