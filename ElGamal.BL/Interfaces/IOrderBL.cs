using ElGamal.DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElGamal.BL.Interfaces
{
    public interface IOrderBL
    {
        MakeOrderResultDTO MakeOrder(OrderDTO item);

        List<GetOrderDTO> GetOrdersByStatus(bool status);

        OrderDetailsDTO GetOrderDetailsByID(Guid id);

    }
}
