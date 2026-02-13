using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Authentication;
using Insurance.Application.Common.Paging;
using Insurance.Application.Policy.DTOs;
using Insurance.Application.Policy.Queries;
using Insurance.Domain.Policies;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.Policies.Queries
{
    public class SearchPoliciesQueryHandlerTests
    {
        private readonly Mock<IPolicySearchRepository> _searchRepositoryMock;
        private readonly Mock<ICurrentUserContext> _currentUserContextMock;
        private readonly SearchPoliciesQueryHandler _handler;
        

        public SearchPoliciesQueryHandlerTests()
        {
            _searchRepositoryMock = new Mock<IPolicySearchRepository>();
            _currentUserContextMock = new Mock<ICurrentUserContext>();
            _handler = new SearchPoliciesQueryHandler(_searchRepositoryMock.Object, _currentUserContextMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnPagedResultFromRepository()
        {
            var expectedResult = new PagedResult<PolicyDetailsDto>(
                new[]
                {
                new PolicyDetailsDto { Id = Guid.NewGuid() }
                },
                1,
                10,
                1);

            _currentUserContextMock
                .SetupGet(x => x.BrokerId)
                .Returns(Guid.NewGuid());

            _searchRepositoryMock
                .Setup(r => r.SearchAsync(
                    It.IsAny<Guid?>(),
                    It.IsAny<Guid?>(),
                    It.IsAny<PolicyStatus?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            var query = new SearchPoliciesQuery
            {
                PageNumber = 1,
                PageSize = 10
            };

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(expectedResult.TotalCount, result.TotalCount);
            Assert.Single(result.Items);
        }

        [Fact]
        public async Task Handle_ShouldCallSearchRepositoryWithCorrectParameters()
        {
            var query = new SearchPoliciesQuery
            {
                ClientId = Guid.NewGuid(),
                Status = PolicyStatus.Active,
                StartDateFrom = DateTime.UtcNow.AddDays(-10),
                StartDateTo = DateTime.UtcNow,
                PageNumber = 2,
                PageSize = 5
            };

            var brokerId = Guid.NewGuid();
            _currentUserContextMock
                .SetupGet(x => x.BrokerId)
                .Returns(brokerId);

            _searchRepositoryMock
                .Setup(r => r.SearchAsync(
                    query.ClientId,
                    brokerId,
                    query.Status,
                    query.StartDateFrom,
                    query.StartDateTo,
                    query.PageNumber,
                    query.PageSize,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PagedResult<PolicyDetailsDto>(
                    Array.Empty<PolicyDetailsDto>(),
                    query.PageNumber,
                    query.PageSize,
                    0));

            await _handler.Handle(query, CancellationToken.None);

            _searchRepositoryMock.Verify(r => r.SearchAsync(
                query.ClientId,
                brokerId,
                query.Status,
                query.StartDateFrom,
                query.StartDateTo,
                query.PageNumber,
                query.PageSize,
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }

}
