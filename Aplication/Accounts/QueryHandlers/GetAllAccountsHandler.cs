using Application.Accounts.Queries;
using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Accounts.QueryHandlers;

//public class GetAllAccountHandler : IRequestHandler<GetAllAccounts, IEnumerable<Account>>
//{
//    private readonly IAccountRepository _repository;

//    public GetAllAccountHandler(IAccountRepository repository)
//    {
//        _repository = repository;
//    }
//    public Task<IEnumerable<Account>> Handle(GetAllAccounts request, CancellationToken cancellationToken)
//    {
//        return _repository.GetAll(request.UserId);
//    }
//}
