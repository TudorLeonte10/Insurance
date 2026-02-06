using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Clients.DTOs
{
    public class ClientSummaryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string IdentificationNumber { get; set; } = string.Empty;
    }
}
