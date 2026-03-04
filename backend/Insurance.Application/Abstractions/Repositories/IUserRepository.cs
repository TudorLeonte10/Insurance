using Insurance.Application.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task<UserAuthData?> GetByUsernameAsync(
            string username,
            CancellationToken ct);
    }
}
