using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shipping.DTOs.OrderReportDTOs;
using Shipping.DTOs;
using Shipping.Models;
using Shipping.Services.IModelService;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderReportController : ControllerBase
    {
        IMapper mapper;
        IOrderService orderService;
        IDeliveryService deliveryService;
        UserManager<ApplicationUser> userManager;

        public OrderReportController(IMapper mapper, IOrderService orderService, UserManager<ApplicationUser> userManager, IDeliveryService deliveryService)
        {
            this.mapper = mapper;
            this.orderService = orderService;
            this.userManager = userManager;
            this.deliveryService = deliveryService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll(
                                string? searchTxt,
                                DateTime? startDate, DateTime? endDate,
                                string orderStatus = "New",
                                int page = 1, int pageSize = 10)
        {
            try
            {
                var orders = await orderService.GetAllExistOrdersByStatus(orderStatus);

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
                                (item.ClientEmail?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false) ||
                                (item.Merchant.ApplicationUser.UserName?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false) ||
                                (item.Government.Name?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false) ||
                                (item.City.Name?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false) ||
                                (item.SerialNumber?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false)
                            )
                            .ToList();

                        if (!orders.Any()) return NotFound(GeneralResponse.Failure("Not Found."));
                    }

                    // Other filters
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

                    var orderDTO = mapper.Map<List<OrderReportGetDTO>>(paginatedOrders);

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
    }
}
