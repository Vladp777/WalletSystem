using Domain.Entities;
using MediatR;

namespace Application.Authentication.Commands;

public record DeleteUserCommand(
    Guid Id): IRequest<ApplicationUser>;
