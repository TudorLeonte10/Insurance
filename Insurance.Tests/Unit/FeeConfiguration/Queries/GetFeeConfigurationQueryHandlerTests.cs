using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Common.Paging;
using Insurance.Application.FeeConfiguration.DTOs;
using Insurance.Application.FeeConfiguration.Queries;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.FeeConfiguration.Queries
{
    public class GetFeeConfigurationQueryHandlerTests
    {
        private readonly Mock<IFeeConfigurationReadRepository> _readRepositoryMock;
        private readonly GetFeeConfigurationQueryHandler _handler;

        public GetFeeConfigurationQueryHandlerTests()
        {
            _readRepositoryMock = new Mock<IFeeConfigurationReadRepository>();
            _handler = new GetFeeConfigurationQueryHandler(_readRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_PagedResult()
        {
            var expected = new PagedResult<FeeConfigurationDto>(new List<FeeConfigurationDto>(), 0, 1, 10);

            _readRepositoryMock
                .Setup(x => x.GetPagedAsync(1, 10, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            var query = new GetFeeConfigurationQuery(1, 10);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(expected, result);

            _readRepositoryMock.Verify(x =>
                x.GetPagedAsync(1, 10, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Pass_Correct_Parameters()
        {
            var expected = new PagedResult<FeeConfigurationDto>(new List<FeeConfigurationDto>(), 0, 2, 5);
            _readRepositoryMock
                .Setup(x => x.GetPagedAsync(2, 5, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);
            var query = new GetFeeConfigurationQuery(2, 5);
            var result = await _handler.Handle(query, CancellationToken.None);
            Assert.Equal(expected, result);
            _readRepositoryMock.Verify(x =>
                x.GetPagedAsync(2, 5, It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
