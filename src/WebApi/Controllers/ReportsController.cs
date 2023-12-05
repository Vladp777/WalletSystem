using Application.Reports.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ReportsController : ApiController
    {
        private readonly ISender _mediator;
        public ReportsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{accountId}/{typeId}")]
        public async Task<IActionResult> GetByType([FromRoute]Guid accountId, int typeId, [FromQuery] DateOnly fromDate, DateOnly toDate)
        {
            if (toDate == DateOnly.MinValue)
            {
                toDate = DateOnly.FromDateTime(DateTime.Now);
            }

            var query = new GetReportByType(accountId, typeId, fromDate, toDate);
            var result = await _mediator.Send(query);

            return result.Match(
                result => Ok(result),
                errors => Problem(errors));
        }

        [HttpGet("{accountId}/{typeId}/{tagId}")]
        public async Task<IActionResult> GetByTypeAndTag([FromRoute] Guid accountId, int typeId, int tagId, [FromQuery] DateOnly fromDate, DateOnly toDate)
        {
            if (toDate == DateOnly.MinValue)
            {
                toDate = DateOnly.FromDateTime(DateTime.Now);
            }

            var query = new GetReportByTypeAndTag(accountId, typeId, tagId, fromDate, toDate);
            var result = await _mediator.Send(query);

            return result.Match(
                result => Ok(result),
                errors => Problem(errors));
        }

    }
}
