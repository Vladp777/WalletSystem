using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Accounts.Queries;
using Application.Accounts.Commands;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
public class AccountsController : ApiController
{
    private readonly ISender _mediatr;

    public AccountsController(ISender mediatr)
    {
        _mediatr = mediatr;
    }

    [HttpGet("getAll/{userId}")]
    public async Task<IActionResult> GetAll(Guid userId)
    {
        var request = new GetAllAccounts(userId);

        var result = await _mediatr.Send(request);
        if (result == null)
        {
            return NoContent();
        }
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var request = new GetAccountById(id);

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
