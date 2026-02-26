using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Brokers.DTOs;
using Insurance.Application.Brokers.Queries;
using Insurance.Application.Common.Paging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.BrokersManagement.Queries
{
    public class GetBrokersQueryHandlerTests
    {
        private readonly Mock<IBrokerReadRepository> _readRepositoryMock;
        private readonly GetBrokersQueryHandler _handler;

        public GetBrokersQueryHandlerTests()
        {
            _readRepositoryMock = new Mock<IBrokerReadRepository>();
            _handler = new GetBrokersQueryHandler(_readRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_Paged_Brokers()
        {
            var brokers = new List<BrokerDetailsDto>
        {
            new BrokerDetailsDto
            {
                Id = Guid.NewGuid(),
                Name = "Broker 1",
                Email = "b1@test.com",
                IsActive = true
            }
        };

            var pagedResult = new PagedResult<BrokerDetailsDto>(
                brokers,
                pageNumber: 1,
                pageSize: 10,
                totalCount: 1);

            _readRepositoryMock
                .Setup(x => x.GetPagedAsync(1, 10, It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedResult);

            var query = new GetBrokersQuery(1, 10);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(1, result.TotalCount);
            Assert.Single(result.Items);

            _readRepositoryMock.Verify(
                x => x.GetPagedAsync(1, 10, It.IsAny<CancellationToken>()),
                Times.Once);
        }


    }

}
