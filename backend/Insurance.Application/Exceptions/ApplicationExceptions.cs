using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Application.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NotFoundException : Exception
    {
        public NotFoundException(string message)
            : base(message)
        {
        }
    }

    [ExcludeFromCodeCoverage]
    public class ConflictException : Exception
    {
        public ConflictException(string message)
            : base(message)
        {
        }
    }

    [ExcludeFromCodeCoverage]
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message)
            : base(message)
        {
        }
    }

    [ExcludeFromCodeCoverage]
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message)
            : base(message)
        {
        }
    }
}
