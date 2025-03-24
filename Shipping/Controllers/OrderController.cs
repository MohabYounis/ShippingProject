using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shipping.DTOs;
using Shipping.DTOs.OrderDTOs;
using Shipping.Models;
using Shipping.Services;
using Shipping.Services.IModelService;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        IMapper mapper;
        IServiceGeneric<Order> serviceGeneric;
        IOrderService orderService;
        GeneralResponse response;
        UserManager<ApplicationUser> userManager;

        public OrderController(IMapper mapper, IServiceGeneric<Order> serviceGeneric, GeneralResponse response, IOrderService orderService, UserManager<ApplicationUser> userManager)
        {
            this.mapper = mapper;
            this.serviceGeneric = serviceGeneric;
            this.response = response;
            this.orderService = orderService;
            this.userManager = userManager;
        }


        [HttpGet("{orderStatus}/All")]
        [EndpointSummary("Get all orders -deleted or not- with specific status to shipping employee")]
        public async Task<ActionResult<GeneralResponse>> GetAllByStatus(string orderStatus = "New")
        {
            try
            {
                var orders = await orderService.GetAllOrdersByStatus(orderStatus);
                if (orders == null)
                {
                    response.IsSuccess = false;
                    response.Data = "No Found";
                    return NotFound(response);
                }
                else
                {
                    var ordersDTO = mapper.Map<OrderGetDTO>(orders);
                    response.IsSuccess = true;
                    response.Data = ordersDTO;
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
        

        [HttpGet("{orderStatus}/AllExist")]
        [EndpointSummary("Get all Exist orders with specific status to shipping employee")]
        public async Task<ActionResult<GeneralResponse>> GetAllExistByStatus(string orderStatus = "New")
        {
            try
            {
                var orders = await orderService.GetAllExistOrdersByStatus(orderStatus);
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
        public async Task<ActionResult<GeneralResponse>> CreateOrder(OrderCreateDTO orderFromReq)
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

        [HttpDelete("{orderId:int}")]
        [EndpointSummary("Delete order by employee or merchant, or reject it by delivery.")]
        public async Task<ActionResult<GeneralResponse>> DeleteOrReject([FromQuery] string userId, [FromRoute] int orderId)
        {
            try
            {
                var order = await serviceGeneric.GetByIdAsync(orderId);
                var user = await userManager.FindByIdAsync(userId);

                if (user == null || order == null)
                {
                    response.IsSuccess = false;
                    response.Data = "User or order is Not Found";
                    return BadRequest(response);
                }

                var userRole = await userManager.IsInRoleAsync(user, "Deliver");
                if (userRole)
                {
                    order.Delivery_Id = null; // لغيت اسنادة لدلفري
                    order.OrderStatus = OrderStatus.Pending; // هعدل حالته علشان اعرف ارجع اسنده لدلفري تاني

                    await serviceGeneric.UpdateAsync(order);
                    await serviceGeneric.SaveChangesAsync();

                    string message = $"Delivery [{(user.UserName.ToUpper())}] has canceled the assignment of the order with serial number [{order.SerialNumber}]";
                    response.IsSuccess = true;
                    response.Data = message;
                    return Ok(response);
                }

                await serviceGeneric.DeleteAsync(orderId);
                await serviceGeneric.SaveChangesAsync();

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

    }
}
