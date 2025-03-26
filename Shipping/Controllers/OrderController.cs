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

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        IMapper mapper;
        IOrderService orderService;
        IDeliveryService deliveryService;
        GeneralResponse response;
        UserManager<ApplicationUser> userManager;

        public OrderController(IMapper mapper, GeneralResponse response, IOrderService orderService, UserManager<ApplicationUser> userManager, IDeliveryService deliveryService)
        {
            this.mapper = mapper;
            this.response = response;
            this.orderService = orderService;
            this.userManager = userManager;
            this.deliveryService = deliveryService;
        }


        //[HttpGet("{orderStatus}/All")]
        //[EndpointSummary("Get all orders -deleted or not- with specific status to shipping employee")]
        //public async Task<ActionResult<GeneralResponse>> GetAllByStatus(string orderStatus = "New")
        //{
        //    try
        //    {
        //        var orders = await orderService.GetAllOrdersByStatus(orderStatus);
        //        if (orders == null)
        //        {
        //            response.IsSuccess = false;
        //            response.Data = "No Found";
        //            return NotFound(response);
        //        }
        //        else
        //        {
        //            var ordersDTO = mapper.Map<OrderGetDTO>(orders);
        //            response.IsSuccess = true;
        //            response.Data = ordersDTO;
        //            return Ok(response);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.IsSuccess = false;
        //        response.Data = ex.Message;
        //        return StatusCode(500, response);
        //    }
        //}   

        [HttpGet("{orderStatus:alpha}/{all:alpha}")]
        [EndpointSummary("Get orders with specific status to shipping employee")]
        public async Task<ActionResult<GeneralResponse>> GetWithPaginationAndSearch(
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
                else
                {
                    response.IsSuccess = false;
                    response.Data = "Parameter Not Exist";
                    return BadRequest(response);
                }

                if (orders == null || !orders.Any())
                {
                    response.IsSuccess = false;
                    response.Data = "No Found";
                    return NotFound(response);
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

                        if (!orders.Any())
                        {
                            response.IsSuccess = false;
                            response.Data = "No Found";
                            return NotFound(response);
                        }
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

                    response.IsSuccess = true;
                    response.Data = result;
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
                return StatusCode(500, response);
            }
        }

        //[HttpGet("{orderStatus}/AllExist")]
        //[EndpointSummary("Get all Exist orders with specific status to shipping employee")]
        //public async Task<ActionResult<GeneralResponse>> GetAllExistByStatus(string orderStatus = "New")
        //{
        //    try
        //    {
        //        var orders = await orderService.GetAllExistOrdersByStatus(orderStatus);
        //        if (orders == null)
        //        {
        //            response.IsSuccess = false;
        //            response.Data = "No Found";
        //            return NotFound(response);
        //        }
        //        else
        //        {
        //            var ordersDTO = mapper.Map<List<OrderGetDTO>>(orders);
        //            response.IsSuccess = true;
        //            response.Data = ordersDTO;
        //            return NotFound(response);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.IsSuccess = false;
        //        response.Data = ex.Message;
        //        return StatusCode(500, response);
        //    }
        //}

        //-------------------------------------------------------------------------------------------------------------------------------------------------------

        [HttpGet("Delivery/{id:int}/All")]
        [EndpointSummary("Get all orders or orders with specific status -deleted or not- to delivery")]
        public async Task<ActionResult<GeneralResponse>> GetAllByDeliveryByStatus(int id, string orderStatus = "DeliveredToAgent") //orderStatus = All ===> receive all orders, orderStatus == any status ===> recieve all orders that have this status.
        {
            try
            {
                var orders = await orderService.GetAllByDeliveryByStatus(id, orderStatus);
                if (orders == null)
                {
                    response.IsSuccess = false;
                    response.Data = "No Found";
                    return NotFound(response);
                }
                else
                {
                    var ordersDTO = mapper.Map<List<OrderGetDTO>>(orders);
                    response.IsSuccess = true;
                    response.Data = ordersDTO;
                    return NotFound(response);
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
                return StatusCode(500, response);
            }
        }
        

        [HttpGet("Delivery/{id:int}/{orderStatus}/AllExist")]
        [EndpointSummary("Get all exist orders or exist orders with specific status to delivery")]
        public async Task<ActionResult<GeneralResponse>> GetAllExistByDeliveryByStatus(int id, string orderStatus = "DeliveredToAgent")
        {
            try
            {
                var orders = await orderService.GetAllExistByDeliveryByStatus(id, orderStatus);
                if (orders == null)
                {
                    response.IsSuccess = false;
                    response.Data = "No Found";
                    return NotFound(response);
                }
                else
                {
                    var ordersDTO = mapper.Map<List<OrderGetDTO>>(orders);
                    response.IsSuccess = true;
                    response.Data = ordersDTO;
                    return NotFound(response);
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
                return StatusCode(500, response);
            }
        }

        //------------------------------------------------------------------------------------------------------------------------------------

        [HttpGet("Merchant/{id:int}/All")]
        [EndpointSummary("Get all orders -deleted or not- to merchant")]
        public async Task<ActionResult<GeneralResponse>> GetAllByMerchantByStatus(int id, string orderStatus = "New")
        {
            try
            {
                var orders = await orderService.GetAllByMerchantByStatus(id, orderStatus);
                if (orders == null)
                {
                    response.IsSuccess = false;
                    response.Data = "No Found";
                    return NotFound(response);
                }
                else
                {
                    var ordersDTO = mapper.Map<List<OrderGetDTO>>(orders);
                    response.IsSuccess = true;
                    response.Data = ordersDTO;
                    return NotFound(response);
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
                return StatusCode(500, response);
            }
        }


        [HttpGet("Merchant/{id:int}/{orderStatus}/AllExist")]
        [EndpointSummary("Get all exist orders with specific status to merchant")]
        public async Task<ActionResult<GeneralResponse>> GetAllExistByMerchantByStatus(int id, string orderStatus = "New")
        {
            try
            {
                var orders = await orderService.GetAllExistByMerchantByStatus(id, orderStatus);
                if (orders == null)
                {
                    response.IsSuccess = false;
                    response.Data = "No Found";
                    return NotFound(response);
                }
                else
                {
                    var ordersDTO = mapper.Map<List<OrderGetDTO>>(orders);
                    response.IsSuccess = true;
                    response.Data = ordersDTO;
                    return NotFound(response);
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
                return StatusCode(500, response);
            }
        }

        //------------------------------------------------------------------------------------------------------------------------------------

        [HttpPost]
        [EndpointSummary("Create order and calculate its shipping cost")]
        public async Task<ActionResult<GeneralResponse>> CreateOrder(OrderCreateEditDTO orderFromReq)
        {
            if (!ModelState.IsValid)
            {
                response.IsSuccess = false;
                response.Data = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()
                );
                return BadRequest(response);
            }
            try
            {
                var order = mapper.Map<Order>(orderFromReq);
                if (order.Products.Count == 0)
                {
                    response.IsSuccess = false;
                    response.Data = "No Products Found";
                    return BadRequest(response);
                }

                decimal totalShippingCost = await orderService.CalculateShippingCost(orderFromReq);
                order.ShippingCost = totalShippingCost;

                await orderService.AddAsync(order);
                await orderService.SaveChangesAsync();

                response.IsSuccess = true;
                response.Data = "Order Created Successfully.";
                return CreatedAtAction("Create", response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
                return StatusCode(500, response);
            }
        }

        //------------------------------------------------------------------------------------------------------------------------------------

        [HttpPut("{id:int}")]
        [EndpointSummary("Edit the order's information.")]
        public async Task<ActionResult<GeneralResponse>> EditById(int id, [FromBody] OrderCreateEditDTO orderEditDTO)
        {
            if (!ModelState.IsValid)
            {
                response.IsSuccess = false;
                response.Data = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()
                );
                return BadRequest(response);
            }
            try
            {
                var existingOrder = await orderService.GetByIdAsync(id);
                if (existingOrder == null)
                {
                    response.IsSuccess = false;
                    response.Data = "Not Found.";
                    return NotFound();
                }

                existingOrder.Merchant_Id = orderEditDTO.Merchant_Id;
                existingOrder.Branch_Id = orderEditDTO.Branch_Id;
                existingOrder.Government_Id = orderEditDTO.Government_Id;
                existingOrder.ShippingType_Id = orderEditDTO.ShippingType_Id;
                existingOrder.City_Id = orderEditDTO.City_Id;
                existingOrder.OrderType = Enum.Parse<OrderType>(orderEditDTO.OrderType);
                existingOrder.ClientName = orderEditDTO.ClientName;
                existingOrder.ClientPhone1 = orderEditDTO.ClientPhone1;
                existingOrder.ClientPhone2 = orderEditDTO.ClientPhone2;
                existingOrder.ClientEmail = orderEditDTO.ClientEmail;
                existingOrder.ClientAddress = orderEditDTO.ClientAddress;
                existingOrder.DeliverToVillage = orderEditDTO.DeliverToVillage;
                existingOrder.PaymentType = Enum.Parse<PaymentTypee>(orderEditDTO.PaymentType);
                existingOrder.OrderCost = orderEditDTO.OrderCost;
                existingOrder.OrderTotalWeight = orderEditDTO.OrderTotalWeight;
                existingOrder.MerchantNotes = orderEditDTO.MerchantNotes;
                existingOrder.EmployeeNotes = orderEditDTO.EmployeeNotes;
                existingOrder.DeliveryNotes = orderEditDTO.DeliveryNotes;
                existingOrder.Products = orderEditDTO.Products;

                await orderService.UpdateAsync(existingOrder);
                await orderService.SaveChangesAsync();

                response.IsSuccess = true;
                response.Data = "Order updated successfully.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
                return StatusCode(500, response);
            }
        }

        //------------------------------------------------------------------------------------------------------------------------------------

        [HttpDelete("{orderId:int}")]
        [EndpointSummary("Delete order by employee or merchant, or reject it by delivery.")]
        public async Task<ActionResult<GeneralResponse>> DeleteOrReject([FromQuery] string userId, [FromRoute] int orderId)
        {
            try
            {
                var order = await orderService.GetByIdAsync(orderId);
                var user = await userManager.FindByIdAsync(userId);

                if (user == null || order == null)
                {
                    response.IsSuccess = false;
                    response.Data = "User or order is Not Found";
                    return NotFound(response);
                }

                var userRole = await userManager.IsInRoleAsync(user, "Deliver");
                if (userRole)
                {
                    order.Delivery_Id = null; // لغيت اسنادة لدلفري
                    order.OrderStatus = OrderStatus.Pending; // هعدل حالته علشان اعرف ارجع اسنده لدلفري تاني

                    await orderService.UpdateAsync(order);
                    await orderService.SaveChangesAsync();

                    string message = $"Delivery [{(user.UserName.ToUpper())}] has canceled the assignment of the order with serial number [{order.SerialNumber}]";
                    response.IsSuccess = true;
                    response.Data = message;
                    return Ok(response);
                }

                await orderService.DeleteAsync(orderId);
                await orderService.SaveChangesAsync();

                response.IsSuccess = true;
                response.Data = "Order deleted successfully.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
                return StatusCode(500, response);
            }
        }

        //------------------------------------------------------------------------------------------------------------------------------------

        [HttpPut("{orderId:int}/{userId:alpha}/{newStatus:alpha}")]
        [EndpointSummary("Change order status.")]
        public async Task<ActionResult<GeneralResponse>> ChangeStatusById(int orderId, string userId,string newStatus, string note = "")
        {
            try
            {
                var order = await orderService.GetByIdAsync(orderId);
                var user = await userManager.FindByIdAsync(userId);
                if (order == null || user == null)
                {
                    response.IsSuccess = false;
                    response.Data = "Order is Not Found";
                    return NotFound(response);
                }
                var lastStatus = order.OrderStatus;
                order.OrderStatus = (OrderStatus)Enum.Parse(typeof(OrderStatus), newStatus);

                bool isInDeliveryRole = await userManager.IsInRoleAsync(user, "Delivery");
                if (isInDeliveryRole) order.DeliveryNotes = note;
                else order.EmployeeNotes = note;

                await orderService.UpdateAsync(order);
                await orderService.SaveChangesAsync();

                response.IsSuccess = true;
                response.Data = $"Status updated from {lastStatus.ToString()} to {newStatus.ToUpper()} successfully.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
                return StatusCode(500, response);
            }
        }

        //------------------------------------------------------------------------------------------------------------------------------------

        [HttpPut("{orderId:int}/{deliveryId:int}")]
        [EndpointSummary("Assign order to delivery.")]
        public async Task<ActionResult<GeneralResponse>> AssignOrderToDelivery (int orderId, int deliveryId)
        {
            try
            {
                var order = await orderService.GetByIdAsync(orderId);
                var delivery = await deliveryService.GetByIdAsync (deliveryId);
                
                if(delivery == null || order == null)
                {
                    response.IsSuccess = false;
                    response.Data = "Not Found.";
                    return NotFound(response);
                }

                if(order.Delivery_Id != null)
                {
                    response.IsSuccess = false;
                    response.Data = $"Order is already assigned to a delivery has id: {order.Delivery_Id}.";
                    return BadRequest(response);
                }

                order.Delivery_Id = deliveryId;

                await orderService.UpdateAsync(order);
                await orderService.SaveChangesAsync();

                response.IsSuccess = true;
                response.Data = $"Order assigned to delivery has id: {orderId}.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
                return StatusCode(500, response);
            }
        }

        //------------------------------------------------------------------------------------------------------------------------------------
    }
}
