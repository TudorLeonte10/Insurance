using Insurance.Domain.Abstractions.Repositories;
using Insurance.Domain.Clients;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Repositories
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(InsuranceDbContext dbContext) : base(dbContext) { }

        public async Task<bool> ExistsByIdentifierAsync(string identifier, CancellationToken cancellationToken)
        {
            return await DbContext.Set<Client>().AnyAsync(
                c => c.IdentificationNumber == identifier, cancellationToken);
        }

        public async Task<Client?> GetByIdentificationNumberAsync(string identifier, CancellationToken cancellationToken)
        {
            return await DbContext.Set<Client>().FirstOrDefaultAsync(
                    c => c.IdentificationNumber == identifier, cancellationToken);
        }
    }
}
