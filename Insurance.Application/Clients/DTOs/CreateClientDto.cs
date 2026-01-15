using Insurance.Domain.Clients;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Clients.DTOs
{
    public class CreateClientDto
    {
        public string Name { get; set; } = string.Empty;
        public string IdentificationNumber { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
        public ClientType Type { get; set; }

    }
}
