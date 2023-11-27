using Application.Authentication.Commands;
using Application.Authentication.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ApiController
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

            return result.Match(
                result => Ok(result),
                errors => Problem(errors));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserQuery query)
        {
            var result = await _mediatr.Send(query);

            return result.Match(
             result => Ok(result),
             errors => Problem(errors));
        }
    }
}
