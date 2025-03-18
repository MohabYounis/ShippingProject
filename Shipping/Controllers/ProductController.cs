using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shipping.DTOs.ProductDtos;
using Shipping.Models;
using Shipping.Services;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IServiceGeneric<Product> serviceGeneric;

        public ProductController(IServiceGeneric<Product> serviceGeneric)
        {
            this.serviceGeneric = serviceGeneric;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var products=await serviceGeneric.GetAllAsync();
            if(products==null)
            {
                return NotFound("Not Found Product");
            }
            List<ProductDTO> productDto = new List<ProductDTO>();
            try
            {
                foreach (var product in products)
                {
                    productDto.Add(new ProductDTO()
                    {
                        Id = product.Id,
                        OrderId = product.Id,
                        Name = product.Name,
                        Quantity = product.Quantity,
                        ItemWeight = product.ItemWeight,
                        IsDeleted = product.IsDeleted
                    });
                }

                return Ok(productDto);
            }
            catch (Exception ex) 
            {
                return BadRequest (ex.Message);
            }
           
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetById(int id)
        {
           
            try
            {
                var product = await serviceGeneric.GetByIdAsync(id);
                if (product == null) { return NotFound("Not Found Product has Id = " + id); }
                ProductDTO productDTO = new ProductDTO()
                {
                    Id = product.Id,
                    OrderId = product.Id,
                    Name = product.Name,
                    Quantity = product.Quantity,
                    ItemWeight = product.ItemWeight,
                    IsDeleted = product.IsDeleted
                };
                return Ok(productDTO);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreatProductDto productDto)
        {
            if (ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            try
            {
                Product product = new Product()
                {
                    Product_Id = productDto.OrderId,
                    Name = productDto.Name,
                    Quantity = productDto.Quantity,
                    ItemWeight = productDto.ItemWeight,
                    IsDeleted = false,
                };
                 serviceGeneric.AddAsync(product);
                serviceGeneric.SaveChangesAsync();
                return Ok("Added Succsefully");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateProduct(int id ,CreatProductDto productDto)
        {
            try
            {
                Product product = new Product()
                {
                    Id = id,
                    Product_Id = productDto.OrderId,
                    Name = productDto.Name,
                    Quantity = productDto.Quantity,
                    ItemWeight = productDto.ItemWeight,
                    IsDeleted = false,
                };
                if (product == null) { return NotFound("Not Found Product has Id = " + id); }
                await serviceGeneric.UpdateAsync(id);
                serviceGeneric.SaveChangesAsync();
                return Ok("Updated Succsefully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
