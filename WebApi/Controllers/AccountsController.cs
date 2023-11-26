using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Accounts.Queries;
using Application.Accounts.Commands;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly ISender _mediatr;

    public AccountsController(ISender mediatr)
    {
        _mediatr = mediatr;
    }

    //[HttpGet("{userId}")]
    //public async Task<IActionResult> GetAll(GetAllAccounts request)
    //{
    //    var result =  await _mediatr.Send(request);
    //    if (result == null)
    //    {
    //        return NoContent();
    //    }
    //    return Ok(result);
    //}

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(GetAccountById request)
    {
        var result = await _mediatr.Send(request);
        if (result == null)
        {
            return NoContent();
        }
        return Ok(result);
    }

    // POST api/<AccountsController>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAccount command)
    {
        var result = await _mediatr.Send(command);

        if (result == null)
        {
            return BadRequest();
        }

        return Created($"api/accounts/{result.Id}", result);
    }

    // PUT api/<AccountsController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<AccountsController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
