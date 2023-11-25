using Application.Authentication.Queries;
using Application.Interfaces;
using Application.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authentication.QueryHandlers
{
    internal class LoginUserHandler : IRequestHandler<LoginUserQuery, AuthenticationResult>
    {
        private readonly IIdentityService _identityService;

        public LoginUserHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public Task<AuthenticationResult> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            return  _identityService.LoginUser(request.Email, request.Password);
        }
    }
}
