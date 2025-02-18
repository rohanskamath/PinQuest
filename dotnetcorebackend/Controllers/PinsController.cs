using dotnetcorebackend.Application.Services.Pinservice.Commands;
using dotnetcorebackend.Application.Services.Pinservice.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dotnetcorebackend.Controllers
{
    [Route("api")]
    [ApiController]
    public class PinsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PinsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("pins")]
        public async Task<IActionResult> CreatePins([FromBody] CreateNewPinCommand command)
        {
            try
            {
                var newPin = await _mediator.Send(command);
                return Ok(new { success = true, message = "Pin created sucessfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("pins")]
        public async Task<IActionResult> GetAllPins()
        {
            try
            {
                var pins = await _mediator.Send(new GetAllPinsQuery());
                return Ok(new { success = true, message = "Pins fetched successfully!", data = pins });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("pins/{userId}")]
        public async Task<IActionResult> GetPinsByUserId([FromRoute] Guid userId)
        {
            try
            {
                var pins = await _mediator.Send(new GetPinsByIdQuery(userId));
                return Ok(new { success = true, message = "Your pins fetched successfully!", data = pins });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });

            }
        }
    }
}
