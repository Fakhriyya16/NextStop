using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Cities;
using Service.Interfaces;

namespace NextStop.Controllers.Admin
{
    public class CityController : BaseAdminController
    {
        private readonly ICityService _cityService;

        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CityCreateDto request)
        {
            await _cityService.CreateAsync(request);
            return Ok(); 
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromQuery] int id, [FromForm] CityEditDto request)
        {
            await _cityService.EditAsync(id, request);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            await _cityService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _cityService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return Ok(await _cityService.GetByIdAsync(id));
        }
    }
}
