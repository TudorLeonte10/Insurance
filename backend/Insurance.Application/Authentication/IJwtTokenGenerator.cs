using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Authentication
{
    public interface IJwtTokenGenerator
    {
        string Generate(AuthUserContext userContext);
    }
}
