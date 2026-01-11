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

        /// <summary>
        /// Retrieves all products.
        /// </summary>
        /// <returns>
        /// 200 OK with list of products,  
        /// 404 Not Found if no products are found,  
        /// 400 BadRequest if an exception occurs.
        /// </returns>
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

        /// <summary>
        /// Retrieves a specific product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>
        /// 200 OK with product data,  
        /// 404 Not Found if product does not exist,  
        /// 400 BadRequest if an error occurs.
        /// </returns>
        [HttpGet("{id}")]
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

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="productDto">The product data to be created.</param>
        /// <returns>
        /// 200 OK if the product was created successfully,  
        /// 400 BadRequest if the data is invalid or an exception occurs.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreatProductDto productDto)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            try
            {
                Product product = new Product()
                {
                    Order_Id = productDto.OrderId,
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

        /// <summary>
        /// Updates an existing product by ID.
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="productDto">The new product data.</param>
        /// <returns>
        /// 200 OK if updated successfully,  
        /// 404 Not Found if the product does not exist,  
        /// 400 BadRequest if an error occurs.
        /// </returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id ,CreatProductDto productDto)
        {
            try
            {
                Product product = new Product()
                {
                    Id = id,
                    Order_Id = productDto.OrderId,
                    Name = productDto.Name,
                    Quantity = productDto.Quantity,
                    ItemWeight = productDto.ItemWeight,
                    IsDeleted = false,
                };
                if (product == null) { return NotFound("Not Found Product has Id = " + id); }
                await serviceGeneric.UpdateAsync(product);
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
