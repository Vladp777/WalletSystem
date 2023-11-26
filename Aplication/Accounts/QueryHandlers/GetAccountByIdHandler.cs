using Application.Accounts.Queries;
using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Accounts.QueryHandlers;

public class GetAccountByIdHandler : IRequestHandler<GetAccountById, Account?>
{
    private readonly IAccountRepository _repository;
    //private readonly ICurrentUserService _currentUserService;

    public GetAccountByIdHandler(IAccountRepository repository)//, ICurrentUserService currentUserService)
    {
        _repository = repository;
        //_currentUserService = currentUserService;
    }
    public Task<Account?> Handle(GetAccountById request, CancellationToken cancellationToken)
    {
        return _repository.Get(request.Id);
    }
}
