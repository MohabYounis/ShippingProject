    using AutoMapper;
    using Azure;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Shipping.DTOs.NewFolder1;
    using Shipping.DTOs;
    using Shipping.Models;
    using Shipping.Services;
    using Shipping.Services.IModelService;

    namespace Shipping.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class WeightPricingController : ControllerBase
        {
            private readonly IWeightPricingService _weigtService;
            IMapper mapper;
            public WeightPricingController(IWeightPricingService weigtService, IMapper mapper)
            {
                _weigtService = weigtService;
                this.mapper = mapper;
            }
        /// <summary>
        /// Creates a new weight pricing record.
        /// </summary>
        /// <param name="weighReq">The weight pricing data transfer object containing DefaultWeight and AdditionalKgPrice.</param>
        /// <returns>
        /// Returns 200 OK with the created record if successful, 
        /// 400 Bad Request with validation errors or a message if the pricing already exists or if a request failure occurs.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WeightPricingDTO weighReq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                // Attempt to add the weight pricing
                var result = await _weigtService.AddWeightAsync(weighReq);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                // If it already exists, return a specific message
                return BadRequest(ex.Message);
            }
            catch (RequestFailedException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing weight pricing record.
        /// </summary>
        /// <param name="weighReq">The weight pricing data transfer object containing updated values.</param>
        /// <returns>
        /// Returns 200 OK with the updated record if successful, 
        /// 400 Bad Request with validation errors or a message if no matching record exists or if a request failure occurs.
        /// </returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] WeightPricingDTO weighReq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var weigh = mapper.Map<WeightPricing>(weighReq);
                var myWeighAfterUpdate = await _weigtService.UpdateWeightAsync(weigh);
                return Ok(mapper.Map(myWeighAfterUpdate, weighReq));
            }
            catch (InvalidOperationException ex)
            {
                // Handle case where update is attempted but no record exists
                return BadRequest(ex.Message);
            }
            catch (RequestFailedException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
    }
