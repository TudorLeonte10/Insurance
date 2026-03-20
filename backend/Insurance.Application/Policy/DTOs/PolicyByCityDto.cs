using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.DTOs
{
    public class PolicyByCityDto
    {
        public string City { get; set; } = string.Empty;

        public int PolicyCount { get; set; }
    }
}
