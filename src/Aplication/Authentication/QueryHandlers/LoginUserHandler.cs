using Application.Authentication.Queries;
using Application.Interfaces;
using Application.Models;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authentication.QueryHandlers
{
    internal class LoginUserHandler : IRequestHandler<LoginUserQuery, ErrorOr<AuthenticationResult>>
    {
        private readonly IIdentityService _identityService;

        public LoginUserHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public async Task<ErrorOr<AuthenticationResult>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            return await _identityService.LoginUser(request.Email, request.Password);
        }
    }
}
