using Application.Authentication.Commands;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authentication.CommandHandlers;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, ApplicationUser>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public CreateUserHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<ApplicationUser> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        
    }
}
