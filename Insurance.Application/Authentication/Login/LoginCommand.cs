using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Authentication.Login
{
    public record LoginCommand(
    string Username,
    string Password
) : IRequest<LoginResponseDto>;

}
