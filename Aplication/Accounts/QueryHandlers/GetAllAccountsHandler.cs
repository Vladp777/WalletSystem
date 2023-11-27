using Application.Accounts.Queries;
using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Domain.Common.Errors;
using MediatR;
using ErrorOr;

namespace Application.Accounts.QueryHandlers;

public class GetAllAccountHandler : IRequestHandler<GetAllAccounts, ErrorOr<List<Account>>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetAllAccountHandler(IAccountRepository repository, ICurrentUserService currentUserService)
    {
        _accountRepository = repository;
        _currentUserService = currentUserService;
    }
    public async Task<ErrorOr<List<Account>>> Handle(GetAllAccounts request, CancellationToken cancellationToken)
    {
        if (request.UserId.ToString() != _currentUserService.UserId)
        {
            return Errors.User.Unauthorized;
        }

        return await _accountRepository.GetAll(request.UserId);
    }
}
