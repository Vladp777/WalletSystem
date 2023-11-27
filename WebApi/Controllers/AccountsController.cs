using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Accounts.Queries;
using Application.Accounts.Commands;
using Microsoft.AspNetCore.Authorization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

        return result.Match(
                result => Ok(result),
                errors => Problem(errors));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var request = new GetAccountById(id);

        var result = await _mediatr.Send(request);

        return result.Match(
                result => Ok(result),
                errors => Problem(errors));
    }

    // POST api/<AccountsController>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAccount command)
    {
        var result = await _mediatr.Send(command);

        return result.Match(
                result => Created($"api/accounts/{result.Id}", result),
                errors => Problem(errors));
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] UpdateAccount command)
    {
        var result = await _mediatr.Send(command);

        return result.Match(
                result => Ok(result),
                errors => Problem(errors));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteAccount(id);
        var result = await _mediatr.Send(command);

        return result.Match(
                result => Ok(result),
                errors => Problem(errors));
    }
}
