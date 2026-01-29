using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace Insurance.Domain.Exceptions
{
    [ExcludeFromCodeCoverage]
    public abstract class BusinessException : Exception
    {
        protected BusinessException(string message) : base(message)
        {
        }
    }

    public class NotFoundException : BusinessException
    {
        public NotFoundException(string message) : base(message) {}
    }

    public class ConflictException : BusinessException
    {
        public ConflictException(string message) : base(message) { }
    }

    public class ForbiddenException : BusinessException
    {
        public ForbiddenException(string message) : base(message) { }
    }
}
