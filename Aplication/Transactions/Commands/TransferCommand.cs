using Domain.Entities;
using ErrorOr;
using MediatR;

namespace Application.Transactions.Commands;

public record TransferCommand(Guid FromId,
    Guid ToId,
    double Count): IRequest<ErrorOr<Transaction>>;

