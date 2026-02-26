using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Authentication
{
    public record UserAuthData(
    Guid UserId,
    string Username,
    string PasswordHash,
    string Role,
    Guid? BrokerId
);
}
