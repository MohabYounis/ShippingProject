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

            
            [HttpGet("{id:int}")]
            public async Task<IActionResult> GetById(int id)
            {
                try
                {
                    var weightPricingObj = await _weigtService.GetByIdAsync(id);
                    if(weightPricingObj == null) return NotFound();

                    var weightPricingDTO = mapper.Map<WeightPricingDTO>(weightPricingObj);
                    return Ok(weightPricingDTO);
                }
                catch (Exception ex) 
                {
                    return StatusCode(500, ex.Message);
                }
            }
            
            
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
                    return BadRequest(ex.Message);
                }
                catch (RequestFailedException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
