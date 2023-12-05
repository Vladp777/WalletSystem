using Application.Models;
using ErrorOr;
using MediatR;

namespace Application.Reports.Queries;

public record GetReportByType(Guid AccountId,
    int TypeId,
    DateOnly FromDate,
    DateOnly ToDate) : IRequest<ErrorOr<Report>>;