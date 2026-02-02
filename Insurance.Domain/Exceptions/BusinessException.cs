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

    public class BuildingConstructionYearNotAllowedException : BusinessException
    {
        public BuildingConstructionYearNotAllowedException(string message)
            : base(message)
        {
        }
    }

    public class BrokerAlreadyActiveException : BusinessException
    {
        public BrokerAlreadyActiveException(string message)
            : base(message)
        {
        }
    }

    public class BrokerAlreadyInactiveException : BusinessException
    {
        public BrokerAlreadyInactiveException(string message)
            : base(message)
        {
        }
    }
}
