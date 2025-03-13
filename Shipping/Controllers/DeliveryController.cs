using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shipping.Models;
using Shipping.Repository;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly IRepositoryGeneric<Delivery> deliveryRepo;

        public DeliveryController(IRepositoryGeneric<Delivery> DeliveryRepo)
        {
            deliveryRepo = DeliveryRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDelivery() 
        {
          var deliveries= await deliveryRepo.GetAllAsync();
            return Ok(deliveries);
        }
    }
}
