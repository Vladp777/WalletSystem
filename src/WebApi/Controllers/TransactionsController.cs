using Application.Transactions.Commands;
using Application.Transactions.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class TransactionsController : ApiController
    {
        private readonly ISender _mediator;

        public TransactionsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTransaction command)
        {
            var result = await _mediator.Send(command);

            return result.Match(
                result => Created($"api/transactions/{result.Id}", result),
                errors => Problem(errors));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = new GetTransactionById(id);

            var result = await _mediator.Send(query);

            return result.Match(
                result => Ok(result),
                errors => Problem(errors));
        }

        [HttpGet("getAll/{userId}")]
        public async Task<IActionResult> GetAll(Guid userId)
        {
            var query = new GetAllAccountTransaction(userId);

            var result = await _mediator.Send(query);

            return result.Match(
                result => Ok(result),
                errors => Problem(errors));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteTransaction(id);

            var result = await _mediator.Send(command);

            return result.Match(
                result => Ok(result),
                errors => Problem(errors));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateTransaction command)
        {
            var result = await _mediator.Send(command);

            return result.Match(
                result => Ok(result),
                errors => Problem(errors));
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Match(
                result => Ok(result),
                errors => Problem(errors));
        }
    } 
}
