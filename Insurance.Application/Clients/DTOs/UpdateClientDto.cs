using Insurance.Domain.Clients;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Clients.DTOs
{
    public class UpdateClientDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public string IdentificationNumber { get; set; } = string.Empty;
    }
}
