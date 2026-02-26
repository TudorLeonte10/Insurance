using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Common.Paging;
using Insurance.Application.Metadata.RiskFactors.DTOs;
using Insurance.Application.Metadata.RiskFactors.Queries;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.RiskFactorConfiguration.Queries
{
    public class GetRiskFactorConfigurationsQueryHandlerTests
    {
        private readonly Mock<IRiskFactorReadRepository> _readRepoMock;
        private readonly GetAllRiskFactorsQueryHandler _handler;

        public GetRiskFactorConfigurationsQueryHandlerTests()
        {
            _readRepoMock = new Mock<IRiskFactorReadRepository>();
            _handler = new GetAllRiskFactorsQueryHandler(
                _readRepoMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_List()
        {
            var list = new List<RiskFactorConfigurationDto>
        {
            new RiskFactorConfigurationDto
            {
                Id = Guid.NewGuid(),
                ReferenceId = "CITY_FIRE",
                AdjustmentPercentage = 10,
                IsActive = true
            }
        };
            var pagedResult = new PagedResult<RiskFactorConfigurationDto>(items: new List<RiskFactorConfigurationDto>
    {
        new RiskFactorConfigurationDto
        {
            Id = Guid.NewGuid(),
            ReferenceId = "CITY_FIRE",
            AdjustmentPercentage = 10,
            IsActive = true
                }
            },
            pageNumber: 1,
            pageSize: 10,
            totalCount: 1);


            _readRepoMock.Setup(x => x.GetPagedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
    .ReturnsAsync(pagedResult);


            var result = await _handler.Handle(
                new GetAllRiskFactorsQuery(1,10),
                CancellationToken.None);

            Assert.Single(result.Items);
        }
    }
}
