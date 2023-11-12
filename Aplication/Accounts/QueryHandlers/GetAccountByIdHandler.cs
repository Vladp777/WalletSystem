using Application.Accounts.Queries;
using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Accounts.QueryHandlers;

public class GetAccountByIdHandler : IRequestHandler<GetAccountById, Account>
{
    private readonly IAccountRepository _repository;

    public GetAccountByIdHandler(IAccountRepository repository)
    {
        _repository = repository;
    }
    public Task<Account> Handle(GetAccountById request, CancellationToken cancellationToken)
    {
        return _repository.Get(request.Id);
    }
}
