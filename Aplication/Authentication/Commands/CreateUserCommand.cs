﻿using Application.Models;
using Domain.Entities;
using ErrorOr;
using MediatR;

namespace Application.Authentication.Commands;

public record CreateUserCommand(
    string Email,
    string Name,
    string Password
    ):IRequest<ErrorOr<AuthenticationResult>>;
