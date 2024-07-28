using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Places;
using Service.Interfaces;

namespace NextStop.Controllers.Admin
{
    public class PlaceController : BaseAdminController
    {
        private readonly IPlaceService _placeService;

        public PlaceController(IPlaceService placeService)
        {
            _placeService = placeService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PlaceCreateDto request)
        {
            await _placeService.CreateAsync(request);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromQuery] int id, [FromForm] PlaceEditDto request)
        {
            await _placeService.EditAsync(id, request);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            await _placeService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _placeService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return Ok(await _placeService.GetByIdAsync(id));
        }
    }
}
