using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Authentication;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly InsuranceDbContext _db;

        public UserRepository(InsuranceDbContext db)
        {
            _db = db;
        }

        public async Task<UserAuthData?> GetByUsernameAsync(
            string username,
            CancellationToken ct)
        {
            return await _db.Users
                .AsNoTracking()
                .Where(u => u.Username == username)
                .Select(u => new UserAuthData(
                    u.Id,
                    u.Username,
                    u.PasswordHash,
                    u.Roles.ToString(),
                    u.BrokerId))
                .FirstOrDefaultAsync(ct);
        }
    }
}
