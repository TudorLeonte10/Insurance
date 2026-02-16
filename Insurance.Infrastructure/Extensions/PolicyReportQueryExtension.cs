using Insurance.Application.Policy.DTOs;
using Insurance.Application.Policy.Queries;
using Insurance.Domain.Buildings;
using Insurance.Domain.Policies;
using Insurance.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Extensions
{
    public static class PolicyReportQueryExtension
    {
        public static IQueryable<PolicyEntity> ApplyFilters(this IQueryable<PolicyEntity> query, GetPoliciesReportQuery request)
        {
            return query
                .FilterByDateRange(request.Dto.From, request.Dto.To)
                .FilterByStatus(request.Dto.Status)
                .FilterByCurrency(request.Dto.Currency)
                .FilterByBuildingType(request.Dto.BuildingType);
        }
        

        public static IQueryable<PolicyEntity> FilterByDateRange(this IQueryable<PolicyEntity> query, DateTime from, DateTime to)
        {
            query = query.Where(p =>
            p.StartDate <= to &&
            p.EndDate >= from);

            return query;
        }

        public static IQueryable<PolicyEntity> FilterByStatus(this IQueryable<PolicyEntity> query, PolicyStatus? status)
        {
            if (status.HasValue)
            {
                query = query.Where(p => p.Status == status.Value);
            }
    
            return query;
        }

        public static IQueryable<PolicyEntity> FilterByCurrency(this IQueryable<PolicyEntity> query, string? currencyCode)
        {
            if (!string.IsNullOrEmpty(currencyCode))
            {
                query = query.Where(p => p.Currency.Code == currencyCode);
            }
    
            return query;
        }

        public static IQueryable<PolicyEntity> FilterByBuildingType(this IQueryable<PolicyEntity> query, BuildingType? buildingType)
        {
            if (buildingType.HasValue)
            {
                query = query.Where(p => p.Building.Type == buildingType.Value);
            }
    
            return query;
        }
    }
}
