using Application.Authentication.Commands;
using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
