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
}
