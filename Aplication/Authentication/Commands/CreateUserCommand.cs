using Domain.Entities;
using MediatR;

namespace Application.Authentication.Commands;

public record CreateUserCommand(
    string Email,
    string UserName,
    string Password
    ):IRequest<ApplicationUser>;
