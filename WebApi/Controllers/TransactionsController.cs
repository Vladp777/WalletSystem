using Application.Transactions.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class TransactionsController : ApiController
    {
        private readonly ISender _mediator;

        public TransactionsController(ISender mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateTransaction command)
        {
            var result = await _mediator.Send(command);

            return result.Match(
                result => Created($"api/transactions/{result.Id}", result),
                errors => Problem(errors));
        }
    }
}
