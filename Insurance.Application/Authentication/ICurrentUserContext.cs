using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Authentication
{
    public interface ICurrentUserContext
    {
        Guid UserId { get; }
        string Role { get; }
        Guid? BrokerId { get; }

    }
}
