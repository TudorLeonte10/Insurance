using Insurance.Application.Policy.DTOs;
using Insurance.Application.Policy.Queries;
using Insurance.Domain.Buildings;
using Insurance.Domain.Policies;
using Insurance.Reporting.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Reporting.Infrastructure.Extensions
{
    public static class PolicyReportQueryExtension
    {
        public static IQueryable<PolicyReportAggregate> ApplyFilters(this IQueryable<PolicyReportAggregate> query, GetPoliciesReportQuery request)
        {
            return query
                .FilterByDateRange(request.Dto.From, request.Dto.To)
                .FilterByStatus(request.Dto.Status)
                .FilterByCurrency(request.Dto.Currency)
                .FilterByBuildingType(request.Dto.BuildingType);
        }
        

        public static IQueryable<PolicyReportAggregate> FilterByDateRange(this IQueryable<PolicyReportAggregate> query, DateTime from, DateTime to)
        {
            query = query.Where(p =>
            p.CreatedAt <= to &&
            p.CreatedAt >= from);

            return query;
        }

        public static IQueryable<PolicyReportAggregate> FilterByStatus(this IQueryable<PolicyReportAggregate> query, PolicyStatus? status)
        {
            if (status.HasValue)
            {
                query = query.Where(p => p.Status == status.Value.ToString());
            }
    
            return query;
        }

        public static IQueryable<PolicyReportAggregate> FilterByCurrency(this IQueryable<PolicyReportAggregate> query, string? currencyCode)
        {
            if (!string.IsNullOrEmpty(currencyCode))
            {
                query = query.Where(p => p.Currency == currencyCode);
            }
    
            return query;
        }

        public static IQueryable<PolicyReportAggregate> FilterByBuildingType(this IQueryable<PolicyReportAggregate> query, BuildingType? buildingType)
        {
            if (buildingType.HasValue)
            {
                query = query.Where(p => p.BuildingType == buildingType.Value.ToString());
            }
    
            return query;
        }
    }
}
