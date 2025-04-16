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
        ISpecialShippingRateService specialShippingRateService;
        IServiceGeneric<WeightPricing> weightService;
        IServiceGeneric<ShippingType> shippingTypeService;
        IServiceGeneric<Merchant> merchantService;
        IServiceGeneric<Setting> settingService;
        IDeliveryService deliveryService;
        public OrderService(IUnitOfWork unitOfWork, ISpecialShippingRateService specialShippingRateService, 
            IServiceGeneric<WeightPricing> weightService, IServiceGeneric<ShippingType> shippingTypeService,
             IServiceGeneric<Merchant> merchantService, IServiceGeneric<Setting> settingService, IDeliveryService deliveryService) : base(unitOfWork)
        {
            this.specialShippingRateService = specialShippingRateService;
            this.weightService = weightService;
            this.shippingTypeService = shippingTypeService;
            this.merchantService = merchantService;
            this.settingService = settingService;
            this.deliveryService = deliveryService;
        }


        public override async Task<IEnumerable<Order>> GetAllAsync()
        {
            var query = unitOfWork.GetRepository<Order>().GetAllAsync();
            var employees = await query;
            return employees
                .Include(e => e.Merchant)
                .Include(e => e.ShippingType)
                .Include(e => e.Delivery)
                .Include(e => e.Government)
                .Include(e => e.City)
                .ToList();
        }

        public override async Task<IEnumerable<Order>> GetAllExistAsync()
        {
            var query = unitOfWork.GetRepository<Order>().GetAllExistAsync();
            var employees = await query;
            return employees
               .Include(e => e.Merchant)
               .Include(e => e.ShippingType)
               .Include(e => e.Delivery)
               .Include(e => e.Government)
               .Include(e => e.City)
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
            var ordersByDeliver = orders.Where(o => o.Delivery_Id== id).ToList();
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


        //----------------------------------------------------------------------------------------------------

        public async Task<decimal> CalculateShippingCost(OrderCreateEditDTO createDTO)
        {
            decimal TotalShippingCost = 0;

            // -----------------------------------------------------------------
            // سعر الشحن لمدينة ولقرية ان طلب
            var specialRate = await specialShippingRateService.GetSpecialRateByMerchant(createDTO.Merchant_Id, createDTO.City_Id);
            var setting = await settingService.GetAllExistAsync();
            var settingFirst = setting.FirstOrDefault(n=>n.Id==1);

            if (specialRate != null)
            {
                TotalShippingCost += specialRate.SpecialPrice;
            }
            else
            {
                TotalShippingCost += (decimal?)specialRate?.City?.StandardShipping ?? 0;

            }

            if (createDTO.DeliverToVillage)
            {

                    TotalShippingCost += settingFirst.ShippingToVillageCost;
               
                
            }
            // -----------------------------------------------------------------

            // -----------------------------------------------------------------
            // سعر الوزن الاضافي
            var weightObj = await weightService.GetAllExistAsync();
            var defaultWeight = weightObj.FirstOrDefault().DefaultWeight;
            var additonalPricingPerKG = weightObj.FirstOrDefault().AdditionalKgPrice;

            if (createDTO.OrderTotalWeight > defaultWeight)
            {
                TotalShippingCost += (decimal)(createDTO.OrderTotalWeight - defaultWeight) * additonalPricingPerKG;
            }
            // -----------------------------------------------------------------

            // -----------------------------------------------------------------
            // نوع الشحن
            var shippingTypeObj = await shippingTypeService.GetByIdAsync(createDTO.ShippingType_Id);
            TotalShippingCost += shippingTypeObj.Cost;
            // -----------------------------------------------------------------

            // -----------------------------------------------------------------
            // نوع الطلب من التاجر
            var orderType = (OrderType)Enum.Parse(typeof(OrderType), (createDTO.OrderType));
            var merchant = await merchantService.GetByIdAsync(createDTO.Merchant_Id);

            if (orderType == OrderType.PickupFromMerchant)
            {
                TotalShippingCost += merchant.PickupCost;
            }

            if (TotalShippingCost == 0) throw new Exception("Shipping cost equal 0");
            // -----------------------------------------------------------------

            return TotalShippingCost;
        }

        public async Task<Order> AssignDeliveryToOrderAndCalculateCompanyAndDeliveryRightsAsync(int orderId, int deliveryId)
        {
            var order = await GetByIdAsync(orderId);
            var delivery = await deliveryService.GetByIdAsync(deliveryId);

            if (delivery == null || order == null) throw new Exception("Not Found.");
            if (order.Delivery_Id != null)  throw new Exception("Order is already assigned to a delivery has id: {order.Delivery_Id}.");

            order.Delivery_Id = deliveryId;

            if (delivery.DiscountType == DiscountType.Fixed)
            {
                order.DeliveryRight = delivery.CompanyPercentage;
            }
            else
            {
                order.DeliveryRight = (delivery.CompanyPercentage/100) * order.ShippingCost;
            }

            order.CompanyRight = order.ShippingCost - order.DeliveryRight;

            return order;
        }
    }
}
