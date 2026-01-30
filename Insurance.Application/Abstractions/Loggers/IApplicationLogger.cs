using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Abstractions.Loggers
{
    public interface IApplicationLogger
    {
        void LogInformation(string message);
        void LogError(Exception ex, string message);
    }
}
