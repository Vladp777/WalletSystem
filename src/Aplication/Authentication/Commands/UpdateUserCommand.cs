using Domain.Entities;
using MediatR;

namespace Application.Authentication.Commands
{
    public record UpdateUserCommand(
        Guid Id,
        string Name
        ): IRequest<ApplicationUser>;
}
