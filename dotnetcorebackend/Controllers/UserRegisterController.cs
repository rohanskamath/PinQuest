using dotnetcorebackend.Application.UserService.Commands;
using dotnetcorebackend.Application.UserService.Queries;
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
    public class UserRegisterController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserRegisterController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserCommand command)
        {
         
            bool userExists = await _mediator.Send(new GetUserByEmailQuery(command.Email));
            if (userExists)
            {
                return BadRequest(new { sucess = false, message = "Email is already registered!" });
            }
            var response = await _mediator.Send(command);
            return Ok(new { sucess = true, message = "Registered Sucessfully!", userData = response });
        }
    }
}
