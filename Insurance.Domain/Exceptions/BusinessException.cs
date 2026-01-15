using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Insurance.Domain.Exceptions
{
    public abstract class BusinessException : Exception
    {
        public BusinessException(string message) : base(message)
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

    public class ForbiddenBusinessException : BusinessException
    {
        public ForbiddenBusinessException(string message) : base(message) { }
    }
}
