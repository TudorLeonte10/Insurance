using Insurance.Domain.Buildings;
using Insurance.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Domain.Clients
{
    public class Client
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public ClientType Type { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string IdentificationNumber { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;
        public string Address { get; private set; } = string.Empty;
        public ICollection<Building> Buildings { get; private set; } = new List<Building>();


        private Client() { }

        public void UpdateDetails(string name, string email, string phoneNumber, string address, string identificationNumber)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ConflictException("Client name cannot be empty");

            if (string.IsNullOrWhiteSpace(identificationNumber))
                throw new ConflictException("Identification number cannot be empty");

            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address ?? string.Empty;

            IdentificationNumber = identificationNumber;
        }

        public static Client Create(ClientType type, string name, string identificationNumber, string email, string phoneNumber, string address)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ConflictException("Client name cannot be empty");

            if (string.IsNullOrWhiteSpace(identificationNumber))
                throw new ConflictException("Identification number cannot be empty");

            return new Client
            {
                Id = Guid.NewGuid(),
                Type = type,
                Name = name,
                IdentificationNumber = identificationNumber,
                Email = email,
                PhoneNumber = phoneNumber,
                Address = address
            };
        }

    }
}
