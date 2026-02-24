using Insurance.Domain.Buildings;
using Insurance.Domain.Clients;
using Insurance.Domain.Exceptions;

namespace Insurance.Domain.Clients
{
    public class Client
    {
        public Guid Id { get; private set; }
        public Guid BrokerId { get; private set; }
        public ClientType Type { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string IdentificationNumber { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;
        public string Address { get; private set; } = string.Empty;

        private readonly List<Building> _buildings = new();
        public IReadOnlyCollection<Building> Buildings => _buildings.AsReadOnly();

        private Client() { }

        public static Client Create(
            ClientType type,
            Guid brokerId,
            string name,
            string identificationNumber,
            string email,
            string phoneNumber,
            string address)
        {

            return new Client
            {
                Id = Guid.NewGuid(),
                BrokerId = brokerId,
                Type = type,
                Name = name,
                IdentificationNumber = identificationNumber,
                Email = email,
                PhoneNumber = phoneNumber,
                Address = address ?? string.Empty
            };
        }

        public void UpdateDetails(
            string name,
            string email,
            string phoneNumber,
            string address,
            string identificationNumber)
        {

            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address ?? string.Empty;
            IdentificationNumber = identificationNumber;
        }

        public static Client Rehydrate(
    Guid id,
    Guid brokerId,
    ClientType type,
    string name,
    string identificationNumber,
    string email,
    string phoneNumber,
    string address)
        {
            return new Client
            {
                Id = id, 
                BrokerId = brokerId,
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
