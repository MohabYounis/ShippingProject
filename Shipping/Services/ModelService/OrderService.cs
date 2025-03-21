using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;
using Shipping.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Shipping.DTOs.OrderDTOs;
using Shipping.DTOs;
namespace Shipping.Services.ModelService
{
    public class OrderService : ServiceGeneric<Order>, IOrderService
    {
        public OrderService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }


        public override async Task<IEnumerable<Order>> GetAllAsync()
        {
            var query = unitOfWork.GetRepository<Order>().GetAllAsync();
            var employees = await query;
            return employees
                .Include(e => e.Merchant)
                .Include(e => e.ShippingCost)
                .Include(e => e.WeightPricing)
                .Include(e => e.Delivery)
                .Include(e => e.Government)
                .Include(e => e.City)
                .Include(e => e.Setting)
                .Include(e => e.RejectReason)
                .ToList();
        }

        public override async Task<IEnumerable<Order>> GetAllExistAsync()
        {
            var query = unitOfWork.GetRepository<Order>().GetAllExistAsync();
            var employees = await query;
            return employees
               .Include(e => e.Merchant)
               .Include(e => e.ShippingCost)
               .Include(e => e.WeightPricing)
               .Include(e => e.Delivery)
               .Include(e => e.Government)
               .Include(e => e.City)
               .Include(e => e.Setting)
               .Include(e => e.RejectReason)
               .ToList();
        }

        //---------------------------------------------------------------------------------------------------------------------------------

        public async Task<IEnumerable<Order>> GetAllOrdersByStatus(string orderStatus)
        {
            var orders = await GetAllAsync();
            var newOrdersList = new List<Order>();

            // convert from string to enum of OrderStatus
            if (Enum.TryParse<OrderStatus>(orderStatus, true, out var orderStatusVariable))
            {
                newOrdersList = orders.Where(o => o.OrderStatus == orderStatusVariable).ToList();
                return newOrdersList;
            }
            else if (orderStatus == "All")
            {
                return orders;
            }
            else
            {
                throw new Exception("parameter is not found");
            }
        }

        public async Task<IEnumerable<Order>> GetAllExistOrdersByStatus(string orderStatus)
        {
            var orders = await GetAllExistAsync();
            var newOrdersList = new List<Order>();

            // convert from string to enum of OrderStatus
            if (Enum.TryParse<OrderStatus>(orderStatus, true, out var orderStatusVariable))
            {
                newOrdersList = orders.Where(o => o.OrderStatus == orderStatusVariable).ToList();
                return newOrdersList;
            }
            else if (orderStatus == "All")
            {
                return orders;
            }
            else
            {
                throw new Exception("parameter is not found");
            }
        }

        //----------------------------------------------------------------------------------------------------
        public async Task<IEnumerable<Order>> GetAllByDeliveryByStatus(int id, string orderStatus)
        {
            var orders = await GetAllAsync();
            var ordersByDeliver = orders.Where(o => o.Delivery.Id == id).ToList();
            var ordersByStatus = new List<Order>();

            if (Enum.TryParse<OrderStatus>(orderStatus, true, out var orderStatusVariable))
            {
                ordersByStatus = ordersByDeliver.Where(o => o.OrderStatus == orderStatusVariable).ToList();
                return ordersByStatus;
            }
            else if (orderStatus == "All")
            {
                return ordersByDeliver;
            }
            else
            {
                throw new Exception("parameter is not found");
            }
        }

        public async Task<IEnumerable<Order>> GetAllExistByDeliveryByStatus(int id, string orderStatus)
        {
            var orders = await GetAllExistAsync();
            var ordersByDeliver = orders.Where(o => o.Delivery.Id == id).ToList();
            var ordersByStatus = new List<Order>();

            if(Enum.TryParse<OrderStatus>(orderStatus, true, out var orderStatusVariable))
            {
                ordersByStatus = ordersByDeliver.Where(o => o.OrderStatus == orderStatusVariable).ToList();
                return ordersByStatus;
            }
            else if (orderStatus == "All")
            {
                return ordersByDeliver;
            }
            else
            {
                throw new Exception("parameter is not found");
            }
        }


        //----------------------------------------------------------------------------------------------------
        public async Task<IEnumerable<Order>> GetAllByMerchantByStatus(int id, string orderStatus)
        {
            var orders = await GetAllAsync();
            var ordersByMerchant = orders.Where(o => o.Merchant.Id == id).ToList();
            var ordersByStatus = new List<Order>();

            if (Enum.TryParse<OrderStatus>(orderStatus, true, out var orderStatusVariable))
            {
                ordersByStatus = ordersByMerchant.Where(o => o.OrderStatus == orderStatusVariable).ToList();
                return ordersByStatus;
            }
            else if (orderStatus == "All")
            {
                return ordersByMerchant;
            }
            else
            {
                throw new Exception("parameter is not found");
            }
        }

        public async Task<IEnumerable<Order>> GetAllExistByMerchantByStatus(int id, string orderStatus)
        {
            var orders = await GetAllExistAsync();
            var ordersByMerchant = orders.Where(o => o.Merchant.Id == id).ToList();
            var ordersByStatus = new List<Order>();

            if (Enum.TryParse<OrderStatus>(orderStatus, true, out var orderStatusVariable))
            {
                ordersByStatus = ordersByMerchant.Where(o => o.OrderStatus == orderStatusVariable).ToList();
                return ordersByStatus;
            }
            else if (orderStatus == "All")
            {
                return ordersByMerchant;
            }
            else
            {
                throw new Exception("parameter is not found");
            }
        }

    }
}
