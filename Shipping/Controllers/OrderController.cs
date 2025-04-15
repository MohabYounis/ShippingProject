using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shipping.DTOs;
using Shipping.DTOs.MerchantDTOs;
using Shipping.DTOs.OrderDTOs;
using Shipping.Models;
using Shipping.Services;
using Shipping.Services.IModelService;
using Shipping.Services.ModelService;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        IMapper mapper;
        IOrderService orderService;
        IDeliveryService deliveryService;
        UserManager<ApplicationUser> userManager;

        public OrderController(IMapper mapper, IOrderService orderService, UserManager<ApplicationUser> userManager, IDeliveryService deliveryService)
        {
            this.mapper = mapper;
            this.orderService = orderService;
            this.userManager = userManager;
            this.deliveryService = deliveryService;
        }

        //------------------------------------------------------------------------------------------------------------------------------------

        [HttpGet("{orderStatus:alpha}/{all:alpha}")]
        [EndpointSummary("Get orders by searching or by specific status to shipping employee")]
        public async Task<ActionResult<GeneralResponse>> GetWithPaginationAndSearchByEmployee(
                                string? searchTxt, int? branchId, int? merchantId,
                                int? governId, int? cityId, int? deliveryId, string? serialNum,
                                DateTime? startDate, DateTime? endDate,
                                string all = "all", string orderStatus = "New",
                                int page = 1, int pageSize = 10)
        {
            try
            {
                IEnumerable<Order> orders;
                if (all == "all") orders = await orderService.GetAllOrdersByStatus(orderStatus);
                else if (all == "exist") orders = await orderService.GetAllExistOrdersByStatus(orderStatus);
                else return BadRequest(GeneralResponse.Failure("Parameter Not Exist."));

                if (orders == null || !orders.Any())
                {
                    return NotFound(GeneralResponse.Failure("Not Found."));
                }
                else
                {
                    if (!string.IsNullOrEmpty(searchTxt))
                    {
                        // Search by ClientName or Phone or Email
                        orders = orders
                            .Where(item =>
                                (item.ClientName?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false) ||
                                (item.ClientPhone1?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false) ||
                                (item.ClientPhone2?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false)
                            )
                            .ToList();

                        if (!orders.Any()) return NotFound(GeneralResponse.Failure("Not Found."));
                    }

                    // Other filters
                    if (branchId.HasValue)
                        orders = orders.Where(o => o.Branch_Id == branchId.Value);

                    if (merchantId.HasValue)
                        orders = orders.Where(o => o.Merchant_Id == merchantId.Value);

                    if (governId.HasValue)
                        orders = orders.Where(o => o.Government_Id == governId.Value);

                    if (cityId.HasValue)
                        orders = orders.Where(o => o.City_Id == cityId.Value);

                    if (deliveryId.HasValue)
                        orders = orders.Where(o => o.Delivery_Id == deliveryId.Value);

                    if (!string.IsNullOrEmpty(serialNum))
                        orders = orders.Where(o => o.SerialNumber.Contains(serialNum, StringComparison.OrdinalIgnoreCase));

                    if (startDate.HasValue)
                        orders = orders.Where(o => o.CreatedDate >= startDate.Value);

                    if (endDate.HasValue)
                        orders = orders.Where(o => o.CreatedDate <= endDate.Value);

                    var totalOrders = orders.Count();

                    // Pagination
                    var paginatedOrders = orders
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    var orderDTO = mapper.Map<List<OrderGetDTO>>(paginatedOrders);

                    var result = new
                    {
                        TotalOrders = totalOrders,          // العدد الإجمالي للعناصر
                        Page = page,                        // الصفحة الحالية
                        PageSize = pageSize,                // عدد العناصر في الصفحة
                        Orders = orderDTO                   // العناصر الحالية
                    };

                    return Ok(GeneralResponse.Success(result));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------------

        [HttpGet("Delivery/{id:int}/{orderStatus:alpha}/{all:alpha}")]
        [EndpointSummary("Get orders by searching or by specific status to delivery")]
        public async Task<ActionResult> GetWithPaginationAndSearchByDelivery(
                                int id, string? searchTxt,
                                int? governId, int? cityId, string? serialNum,
                                DateTime? startDate, DateTime? endDate,
                                string all = "all", string orderStatus = "DeliveredToAgent",
                                int page = 1, int pageSize = 10)
        {
            try
            {
                IEnumerable<Order> orders;
                if (all == "all") orders = await orderService.GetAllByDeliveryByStatus(id, orderStatus);
                else if (all == "exist") orders = await orderService.GetAllExistByDeliveryByStatus(id, orderStatus);
                else return BadRequest(GeneralResponse.Failure("Parameter Not Exist."));

                if (orders == null || !orders.Any())
                {
                    return NotFound(GeneralResponse.Failure("Not Found."));
                }
                else
                {
                    if (!string.IsNullOrEmpty(searchTxt))
                    {
                        // Search by ClientName or Phone or Email
                        orders = orders
                            .Where(item =>
                                (item.ClientName?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false) ||
                                (item.ClientPhone1?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false) ||
                                (item.ClientPhone2?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false) ||
                                (item.ClientEmail?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false)
                            )
                            .ToList();

                        if (!orders.Any()) return NotFound(GeneralResponse.Failure("Not Found."));
                    }

                    // Other filters
                    if (governId.HasValue)
                        orders = orders.Where(o => o.Government_Id == governId.Value);

                    if (cityId.HasValue)
                        orders = orders.Where(o => o.City_Id == cityId.Value);

                    if (!string.IsNullOrEmpty(serialNum))
                        orders = orders.Where(o => o.SerialNumber.Contains(serialNum, StringComparison.OrdinalIgnoreCase));

                    if (startDate.HasValue)
                        orders = orders.Where(o => o.CreatedDate >= startDate.Value);

                    if (endDate.HasValue)
                        orders = orders.Where(o => o.CreatedDate <= endDate.Value);

                    var totalOrders = orders.Count();

                    // Pagination
                    var paginatedOrders = orders
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    var orderDTO = mapper.Map<List<OrderGetDTO>>(paginatedOrders);

                    var result = new
                    {
                        TotalOrders = totalOrders,          // العدد الإجمالي للعناصر
                        Page = page,                        // الصفحة الحالية
                        PageSize = pageSize,                // عدد العناصر في الصفحة
                        Orders = orderDTO                   // العناصر الحالية
                    };

                    return Ok(GeneralResponse.Success(result));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }

        //------------------------------------------------------------------------------------------------------------------------------------

        [HttpGet("Merchant/{id:int}/{orderStatus:alpha}/{all:alpha}")]
        [EndpointSummary("Get orders by searching or by specific status to merchant")]
        public async Task<ActionResult> GetWithPaginationAndSearchByMerchant(
                                int id, string? searchTxt,
                                int? governId, int? cityId, int? branchId,
                                DateTime? startDate, DateTime? endDate,
                                string all = "all", string orderStatus = "New",
                                int page = 1, int pageSize = 10)
        {
            try
            {
                IEnumerable<Order> orders;
                if (all == "all") orders = await orderService.GetAllByMerchantByStatus(id, orderStatus);
                else if (all == "exist") orders = await orderService.GetAllExistByMerchantByStatus(id, orderStatus);
                else return BadRequest(GeneralResponse.Failure("Parameter Not Exist."));

                if (orders == null || !orders.Any()) return NotFound(GeneralResponse.Failure("Not Found."));
                else
                {
                    if (!string.IsNullOrEmpty(searchTxt))
                    {
                        // Search by ClientName or Phone or Email
                        orders = orders
                            .Where(item =>
                                (item.ClientName?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false) ||
                                (item.ClientPhone1?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false) ||
                                (item.ClientPhone2?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false) ||
                                (item.ClientEmail?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false)
                            )
                            .ToList();

                        if (!orders.Any()) return NotFound(GeneralResponse.Failure("Not Found."));
                    }

                    // Other filters
                    if (governId.HasValue)
                        orders = orders.Where(o => o.Government_Id == governId.Value);

                    if (cityId.HasValue)
                        orders = orders.Where(o => o.City_Id == cityId.Value);

                    if (branchId.HasValue)
                        orders = orders.Where(o => o.Branch_Id == branchId.Value);

                    if (startDate.HasValue)
                        orders = orders.Where(o => o.CreatedDate >= startDate.Value);

                    if (endDate.HasValue)
                        orders = orders.Where(o => o.CreatedDate <= endDate.Value);

                    var totalOrders = orders.Count();

                    // Pagination
                    var paginatedOrders = orders
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    var orderDTO = mapper.Map<List<OrderGetDTO>>(paginatedOrders);

                    var result = new
                    {
                        TotalOrders = totalOrders,          // العدد الإجمالي للعناصر
                        Page = page,                        // الصفحة الحالية
                        PageSize = pageSize,                // عدد العناصر في الصفحة
                        Orders = orderDTO                   // العناصر الحالية
                    };

                    return Ok(GeneralResponse.Success(result));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }

        //------------------------------------------------------------------------------------------------------------------------------------

        [HttpPost]
        [EndpointSummary("Create order and calculate its shipping cost")]
        public async Task<ActionResult> CreateOrder(OrderCreateEditDTO orderFromReq)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return BadRequest(GeneralResponse.Failure(errors));
            }
            try
            {
                var order = mapper.Map<Order>(orderFromReq);
                if (order.Products.Count() == 0) return NotFound(GeneralResponse.Failure("Products Not Found."));

                decimal totalShippingCost = await orderService.CalculateShippingCost(orderFromReq);
                order.ShippingCost = totalShippingCost;

                await orderService.AddAsync(order);
                await orderService.SaveChangesAsync();

                return Ok(GeneralResponse.Success("Order created successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }

        //------------------------------------------------------------------------------------------------------------------------------------

        [HttpPut("{id:int}")]
        [EndpointSummary("Edit the order's information.")]
        public async Task<ActionResult> EditById(int id, [FromBody] OrderCreateEditDTO orderEditDTO)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return BadRequest(GeneralResponse.Failure(errors));
            }
            try
            {
                var existingOrder = await orderService.GetByIdAsync(id);
                if (existingOrder == null)
                    return NotFound(GeneralResponse.Failure("Not Found."));

                // AutoMapper: Update existing entity with values from DTO
                mapper.Map(orderEditDTO, existingOrder);

                await orderService.UpdateAsync(existingOrder);
                await orderService.SaveChangesAsync();

                return Ok(GeneralResponse.Success("Order updated successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }

        //------------------------------------------------------------------------------------------------------------------------------------

        [HttpDelete("{orderId:int}")]
        [EndpointSummary("Delete order by employee or merchant, or reject it by delivery.")]
        public async Task<ActionResult> DeleteOrReject([FromQuery] string userId, [FromRoute] int orderId)
        {
            try
            {
                var order = await orderService.GetByIdAsync(orderId);
                var user = await userManager.FindByIdAsync(userId);

                if (user == null || order == null) return NotFound(GeneralResponse.Failure("User or order is Not Found"));

                var userRole = await userManager.IsInRoleAsync(user, "Deliver");
                if (userRole)
                {
                    order.Delivery_Id = null; // لغيت اسنادة لدلفري
                    order.DeliveryRight = 0;
                    order.CompanyRight = 0;
                    order.DeliveryNotes = "";
                    order.OrderStatus = OrderStatus.Pending; // هعدل حالته علشان اعرف ارجع اسنده لدلفري تاني

                    await orderService.UpdateAsync(order);
                    await orderService.SaveChangesAsync();

                    string message = $"Delivery [{(user.UserName.ToUpper())}] has canceled the assignment of the order with serial number [{order.SerialNumber}]";
                    return Ok(GeneralResponse.Success(message));
                }

                await orderService.DeleteAsync(orderId);
                await orderService.SaveChangesAsync();

                return Ok(GeneralResponse.Success("Order deleted successfully by employee."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }

        //------------------------------------------------------------------------------------------------------------------------------------

        [HttpPut("{orderId:int}/{userId:alpha}/{newStatus:alpha}")]
        [EndpointSummary("Change order status.")]
        public async Task<ActionResult> ChangeStatusById(int orderId, string userId, string newStatus, string note = "")
        {
            try
            {
                var order = await orderService.GetByIdAsync(orderId);
                var user = await userManager.FindByIdAsync(userId);
                if (order == null || user == null)  return NotFound(GeneralResponse.Failure("Not Found."));
                
                var lastStatus = order.OrderStatus;
                order.OrderStatus = Enum.Parse<OrderStatus>(newStatus);

                bool isInDeliveryRole = await userManager.IsInRoleAsync(user, "Delivery");
                if (isInDeliveryRole) order.DeliveryNotes = note;
                else order.EmployeeNotes = note;

                await orderService.UpdateAsync(order);
                await orderService.SaveChangesAsync();

                return Ok(GeneralResponse.Success($"Status updated from {lastStatus.ToString()} to {newStatus.ToUpper()} successfully."));

            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }

        //------------------------------------------------------------------------------------------------------------------------------------

        [HttpPut("{orderId:int}/{deliveryId:int}")]
        [EndpointSummary("Assign order to delivery.")]
        public async Task<ActionResult> AssignOrderToDelivery(int orderId, int deliveryId)
        {
            try
            {
                var order = await orderService.AssignDeliveryToOrderAndCalculateCompanyAndDeliveryRightsAsync(orderId, deliveryId);

                await orderService.UpdateAsync(order);
                await orderService.SaveChangesAsync();

                return Ok(GeneralResponse.Success($"Order assigned to delivery has id: {orderId}."));

            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }

        //------------------------------------------------------------------------------------------------------------------------------------
    }
}
