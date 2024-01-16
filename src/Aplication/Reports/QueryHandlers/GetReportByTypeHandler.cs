using Application.Interfaces;
using Application.Models;
using Application.Reports.Queries;
using Application.Repositories;
using Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace Application.Reports.QueryHandlers;

public class GetReportByTypeHandler : IRequestHandler<GetReportByType, ErrorOr<Report>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetReportByTypeHandler(IAccountRepository accountRepository,
        ITransactionRepository transactionRepository,
        ICurrentUserService currentUserService)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        _currentUserService = currentUserService;
    }
    public async Task<ErrorOr<Report>> Handle(GetReportByType request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.Get(request.AccountId);

        if (account == null)
        {
            return Errors.Account.AccountNotFound;
        }

        if (account.UserId != _currentUserService.UserId)
        {
            return Errors.User.Unauthorized;
        }
        
        var transactions = await _transactionRepository.GetTransactionsByTypeAndPeriodDate(request.AccountId,
            request.TypeId,
            request.FromDate,
            request.ToDate);

        if (transactions == null)
        {
            return Errors.Transaction.TransactionNotFound;
        }

        var report = new Report
        {
            Amount = transactions.Sum(t => t.Count),
            Transactions = transactions,
            FromDate = request.FromDate,
            ToDate = request.ToDate
        };

        return report;
    }
}