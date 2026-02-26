using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Authentication
{
    public record AuthUserContext(
    Guid UserId,
    string Username,
    string Role,
    Guid? BrokerId
);
}
