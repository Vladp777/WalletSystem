using Application.Authentication.Commands;
using Application.Authentication.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISender _mediatr;

        public AuthController(ISender mediatr)
        {
            _mediatr = mediatr;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]CreateUserCommand command)
        {
            var result = await _mediatr.Send(command);

            if (!result.Succeeded)
            {
                return BadRequest(new AuthFailResponse
                {
                    Errors = result.Errors
                });
            }

            return Ok(new AuthSuccesResponse
            {
                Id = result.UserId,
                Token = result.Token
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserQuery query)
        {
            var result = await _mediatr.Send(query);

            if (!result.Succeeded)
            {
                return BadRequest(new AuthFailResponse
                {
                    Errors = result.Errors
                });
            }

            return Ok(new AuthSuccesResponse
            {
                Id = result.UserId,
                Token = result.Token
            });
        }
    }
}
