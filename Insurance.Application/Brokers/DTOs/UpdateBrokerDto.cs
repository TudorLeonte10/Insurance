using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Brokers.DTOs
{
    public class UpdateBrokerDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
