using Application.Models;
using ErrorOr;
using MediatR;

namespace Application.Authentication.Queries;

public record LoginUserQuery(
    string Email, 
    string Password): IRequest<ErrorOr<AuthenticationResult>>;
