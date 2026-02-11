using Insurance.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Entities
{
    [ExcludeFromCodeCoverage]
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRoles Roles { get; set; }
        public Guid? BrokerId { get; set; }

    }
  
}
