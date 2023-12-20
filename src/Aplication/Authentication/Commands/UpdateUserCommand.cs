using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authentication.Commands
{
    public record UpdateUserCommand(
        Guid Id,
        string Name
        ): IRequest<ApplicationUser>;
}
