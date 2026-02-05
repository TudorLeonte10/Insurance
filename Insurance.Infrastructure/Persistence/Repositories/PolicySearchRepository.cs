using AutoMapper;
using AutoMapper.QueryableExtensions;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Common.Paging;
using Insurance.Application.Policy.DTOs;
using Insurance.Domain.Policies;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Repositories
{
    [ExcludeFromCodeCoverage]
    public class PolicySearchRepository : IPolicySearchRepository
    {
        private readonly InsuranceDbContext _db;
        private readonly IMapper _mapper;

        public PolicySearchRepository(InsuranceDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<PagedResult<PolicyDetailsDto>> SearchAsync(
            Guid? clientId,
            Guid? brokerId,
            PolicyStatus? status,
            DateTime? startDateFrom,
            DateTime? startDateTo,
            int pageNumber,
            int pageSize,
            CancellationToken ct)
        {
            var query = _db.Policies.AsNoTracking();

            if (clientId.HasValue)
                query = query.Where(p => p.ClientId == clientId);

            if (brokerId.HasValue)
                query = query.Where(p => p.BrokerId == brokerId);

            if (status.HasValue)
                query = query.Where(p => p.Status == status);

            if (startDateFrom.HasValue)
                query = query.Where(p => p.StartDate >= startDateFrom);

            if (startDateTo.HasValue)
                query = query.Where(p => p.StartDate <= startDateTo);

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<PolicyDetailsDto>(_mapper.ConfigurationProvider)
                .ToListAsync(ct);

            return new PagedResult<PolicyDetailsDto>(
                items,
                pageNumber,
                pageSize,
                totalCount);
        }
    }

}
