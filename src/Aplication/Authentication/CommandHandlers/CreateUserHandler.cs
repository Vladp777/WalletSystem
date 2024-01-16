using Application.Authentication.Commands;
using Application.Interfaces;
using Application.Models;
using ErrorOr;
using MediatR;

namespace Application.Authentication.CommandHandlers;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IIdentityService _identityService;

    public CreateUserHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var response = await _identityService.RegisterUser(request.Email, request.Name, request.Password);

        return response;
    }
}
