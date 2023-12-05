using Application.Models;
using ErrorOr;
using MediatR;

namespace Application.Reports.Queries;

public record GetReportByTypeAndTag(Guid AccountId, 
    int TypeId,
    int TagId,
    DateOnly FromDate,
    DateOnly ToDate) : IRequest<ErrorOr<Report>>;


