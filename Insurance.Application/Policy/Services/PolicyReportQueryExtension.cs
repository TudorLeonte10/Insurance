using Insurance.Application.Policy.DTOs;
using Insurance.Application.Policy.Queries;
using Insurance.Domain.Buildings;
using Insurance.Domain.Policies;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Services
{
    public static class PolicyReportQueryExtension
    {
        public static IQueryable<PolicyReportReadModel> ApplyFilters(this IQueryable<PolicyReportReadModel> query, GetPoliciesReportQuery request)
        {
            return query
                .FilterByDateRange(request.From, request.To)
                .FilterByStatus(request.Status)
                .FilterByCurrency(request.Currency)
                .FilterByBuildingType(request.BuildingType);
        }
        

        public static IQueryable<PolicyReportReadModel> FilterByDateRange(this IQueryable<PolicyReportReadModel> query, DateTime? from, DateTime? to)
        {
            if (from.HasValue)
            {
                query = query.Where(p => p.PolicyStartDate >= from.Value);
            }

            if (to.HasValue)
            {
                query = query.Where(p => p.PolicyStartDate <= to.Value);
            }

            return query;
        }

        public static IQueryable<PolicyReportReadModel> FilterByStatus(this IQueryable<PolicyReportReadModel> query, PolicyStatus? status)
        {
            if (status.HasValue)
            {
                query = query.Where(p => p.Status == status.Value);
            }
    
            return query;
        }

        public static IQueryable<PolicyReportReadModel> FilterByCurrency(this IQueryable<PolicyReportReadModel> query, string? currencyCode)
        {
            if (!string.IsNullOrEmpty(currencyCode))
            {
                query = query.Where(p => p.CurrencyCode == currencyCode);
            }
    
            return query;
        }

        public static IQueryable<PolicyReportReadModel> FilterByBuildingType(this IQueryable<PolicyReportReadModel> query, BuildingType? buildingType)
        {
            if (buildingType.HasValue)
            {
                query = query.Where(p => p.BuildingType == buildingType.Value);
            }
    
            return query;
        }
    }
}
