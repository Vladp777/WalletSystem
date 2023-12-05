using Application.Accounts.Queries;
using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace Application.Accounts.QueryHandlers;

public class GetAccountByIdHandler : IRequestHandler<GetAccountById, ErrorOr<Account>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetAccountByIdHandler(IAccountRepository repository, ICurrentUserService currentUserService)
    {
        _accountRepository = repository;
        _currentUserService = currentUserService;
    }
    public async Task<ErrorOr<Account>> Handle(GetAccountById request, CancellationToken cancellationToken)
    {
        var accountToGet = await _accountRepository.NoTrackingGet(request.Id);

        if (accountToGet == null)
        {
            return Errors.Account.AccountNotFound;
        }

        if (accountToGet.UserId != _currentUserService.UserId)
        {
            return Errors.User.Unauthorized;
        }

        return await _accountRepository.Get(request.Id);
    }
}
