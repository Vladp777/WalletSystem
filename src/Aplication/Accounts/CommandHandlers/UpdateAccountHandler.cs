using Application.Accounts.Commands;
using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace Application.Accounts.CommandHandlers;

public class UpdateAccountHandler : IRequestHandler<UpdateAccount, ErrorOr<Account>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public UpdateAccountHandler(IAccountRepository repository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _accountRepository = repository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }
    public async Task<ErrorOr<Account>> Handle(UpdateAccount request, CancellationToken cancellationToken)
    {
        var accountToUpdate = await _accountRepository.NoTrackingGet(request.Id);

        if (accountToUpdate == null)
        {
            return Errors.Account.AccountNotFound;
        }

        if (accountToUpdate.UserId != _currentUserService.UserId)
        {
            return Errors.User.Unauthorized;
        }
        var entity = new Account
        {
            Id = request.Id,
            Name = request.Name,
            Balance = request.Balance
        };

        var result = await _accountRepository.Update(entity);

        await _unitOfWork.SaveAsync();

        return result;
    }
}
