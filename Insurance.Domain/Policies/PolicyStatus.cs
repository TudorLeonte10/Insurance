using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Domain.Policies
{
    public enum PolicyStatus
    {
        Draft = 1,
        Active = 2,
        Cancelled = 3,
        Expired = 4
    }
}
