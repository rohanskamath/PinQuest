using dotnetcorebackend.Application.Services.EmailService;
using dotnetcorebackend.Application.Services.OTPService;
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
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            try
            {
                var existingUser = await _mediator.Send(new GetUserByEmailQuery(command.Email));
                if (existingUser != null)
                {
                    return BadRequest(new { success = false, message = "Email is already registered!" });
                }
                await _mediator.Send(command);
                return Ok(new { success = true, message = "Registered sucessfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            try
            {
                var response = await _mediator.Send(command);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] UpdateUserPasswordCommand command)
        {
            try
            {
                var response = await _mediator.Send(command);
                return Ok(new { success = response, message = "Password changed sucessfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOTP([FromBody] SendOtpCommnd command)
        {
            try
            {
                var response = await _mediator.Send(command);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOTP([FromBody] VerifyOtpCommand command)
        {
            try
            {
                var response = await _mediator.Send(command);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, messge = ex.Message });
            }
        }
    }
}
