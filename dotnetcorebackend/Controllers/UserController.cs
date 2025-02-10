using dotnetcorebackend.Application.Services.UserService.Commands;
using dotnetcorebackend.Application.Services.UserService.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Npgsql.Replication.PgOutput.Messages;

namespace dotnetcorebackend.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserCommand command)
        {

            var existingUser = await _mediator.Send(new GetUserByEmailQuery(command.Email));
            if (existingUser != null)
            {
                return BadRequest(new { success = false, message = "Email is already registered!" });
            }
            await _mediator.Send(command);
            return Ok(new { success = true, message = "Registered Sucessfully!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserCommand command)
        {
            var existingUser = await _mediator.Send(new GetUserByEmailQuery(command.Email));
            if (existingUser == null)
            {
                return BadRequest(new { success = false, message = "User not found!..Kindly register" });
            }
            var response= await _mediator.Send(command);
            return Ok(new { success = true, message = response });
        }
    }
}
