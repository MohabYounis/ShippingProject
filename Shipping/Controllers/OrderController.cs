using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shipping.DTOs;
using Shipping.DTOs.OrderDTOs;
using Shipping.Models;
using Shipping.Services;
using Shipping.Services.IModelService;
using SHIPPING.Services;
using System.ComponentModel;

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

        public OrderController(IMapper mapper, IServiceGeneric<Order> serviceGeneric, GeneralResponse response, IOrderService orderService)
        {
            this.mapper = mapper;
            this.serviceGeneric = serviceGeneric;
            this.response = response;
            this.orderService = orderService;
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

    }
}
