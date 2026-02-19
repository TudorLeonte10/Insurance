using FluentValidation.Results;
using Insurance.Application.Policy.DTOs;
using Insurance.Application.Policy.Enums;
using Insurance.Application.Policy.Queries;
using Insurance.Application.Policy.Validators;
using Insurance.Domain.Buildings;
using Insurance.Domain.Policies;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.Reporting.Validators
{
    public class GetPoliciesReportValidatorsTests
    {
        private readonly GetPoliciesReportRequestValidator _requestValidator = new();
        private readonly GetPolicyReportQueryValidator _queryValidator = new();

        private static GetPoliciesReportRequestDto CreateValidDto()
        {
            var now = DateTime.UtcNow;
            return new GetPoliciesReportRequestDto(
                From: now.AddDays(-7),
                To: now,
                Status: PolicyStatus.Active,
                Currency: "RON",
                BuildingType: BuildingType.Residential);
        }

        [Fact]
        public void RequestValidator_ValidDto_IsValid()
        {
            var dto = CreateValidDto();

            ValidationResult result = _requestValidator.Validate(dto);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void RequestValidator_FromDefault_IsInvalid()
        {
            var now = DateTime.UtcNow;
            var dto = new GetPoliciesReportRequestDto(
                From: default,
                To: now,
                Status: PolicyStatus.Active,
                Currency: "RON",
                BuildingType: BuildingType.Residential);

            ValidationResult result = _requestValidator.Validate(dto);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "From");
        }

        [Fact]
        public void RequestValidator_ToBeforeFrom_IsInvalid()
        {
            var now = DateTime.UtcNow;
            var dto = new GetPoliciesReportRequestDto(
                From: now,
                To: now.AddDays(-1),
                Status: PolicyStatus.Active,
                Currency: "RON",
                BuildingType: BuildingType.Residential);

            ValidationResult result = _requestValidator.Validate(dto);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "To");
        }

        [Fact]
        public void RequestValidator_IntervalExceedsOneYear_IsInvalid()
        {
            var now = DateTime.UtcNow;
            var dto = new GetPoliciesReportRequestDto(
                From: now.AddYears(-2),
                To: now,
                Status: PolicyStatus.Active,
                Currency: "RON",
                BuildingType: BuildingType.Residential);

            ValidationResult result = _requestValidator.Validate(dto);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("Maximum allowed interval"));
        }

        [Fact]
        public void RequestValidator_InvalidCurrencyLength_IsInvalid()
        {
            var dto = CreateValidDto() with { Currency = "RO" };

            ValidationResult result = _request_validator_or_throw(_requestValidator, dto);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Currency");
        }


        [Fact]
        public void RequestValidator_InvalidBuildingType_IsInvalid()
        {
            var dto = CreateValidDto() with { BuildingType = (BuildingType)999 };

            ValidationResult result = _request_validator_or_throw(_requestValidator, dto);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "BuildingType");
        }

        [Fact]
        public void QueryValidator_ValidQuery_IsValid()
        {
            var dto = CreateValidDto();
            var query = new GetPoliciesReportQuery(dto, ReportGroupingType.City);

            ValidationResult result = _queryValidator.Validate(query);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void QueryValidator_InvalidGroupingType_IsInvalid()
        {
            var dto = CreateValidDto();
            var query = new GetPoliciesReportQuery(dto, (ReportGroupingType)999);

            ValidationResult result = _query_validator_or_throw(_queryValidator, query);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "GroupingType" && e.ErrorMessage.Contains("Invalid grouping type"));
        }

  
        private static ValidationResult _request_validator_or_throw(GetPoliciesReportRequestValidator validator, GetPoliciesReportRequestDto dto)
        {
            return validator.Validate(dto);
        }

        private static ValidationResult _query_validator_or_throw(GetPolicyReportQueryValidator validator, GetPoliciesReportQuery query)
        {
            return validator.Validate(query);
        }
    }
}
