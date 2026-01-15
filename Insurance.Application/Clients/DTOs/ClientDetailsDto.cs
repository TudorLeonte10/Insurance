using Insurance.Domain.Clients;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Clients.DTOs
{
    public class ClientDetailsDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public string IdentificationNumber { get; init; } = null!;
        public ClientType Type { get; init; }
        public string ContactInfo { get; init; } = null!;
    }

}
