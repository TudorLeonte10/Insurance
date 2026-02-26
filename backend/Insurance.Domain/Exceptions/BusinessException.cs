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

    public class InvalidBasePremiumException : BusinessException
    {
        public InvalidBasePremiumException(string message)
            : base(message)
        {
        }
    }

    public class InvalidFinalPremiumException : BusinessException
    {
        public InvalidFinalPremiumException(string message)
            : base(message)
        {
        }
    }  

    public class InvalidPolicyTermException : BusinessException
    {
        public InvalidPolicyTermException(string message)
            : base(message)
        {
        }
    }

    public class InvalidPolicyTransitionException : BusinessException
    {
        public InvalidPolicyTransitionException(string message)
            : base(message)
        {
        }
    }

    public class InactiveBrokerException : BusinessException
    {
        public InactiveBrokerException(string message)
            : base(message)
        {
        }
    }

    public class InactiveClientException : BusinessException
    {
        public InactiveClientException(string message)
            : base(message)
        {
        }
    }

    public class InactiveCurrencyException : BusinessException
    {
        public InactiveCurrencyException(string message)
            : base(message)
        {
        }
    }

    public class BuildingNotOwnedByClientException : BusinessException
    {
        public BuildingNotOwnedByClientException(string message)
            : base(message)
        {
        }
    }
}
