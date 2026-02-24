using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Authentication;
using Insurance.Application.Exceptions;
using Insurance.Application.Policy.DTOs;
using Insurance.Application.Policy.Queries;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.Policies.Queries
{
    public class GetPolicyByIdQueryHandlerTests
    {
        private readonly Mock<IPolicyReadRepository> _readRepositoryMock;
        private readonly Mock<ICurrentUserContext> _currentUserContextMock;
        private readonly GetPolicyByIdQueryHandler _handler;

        public GetPolicyByIdQueryHandlerTests()
        {
            _readRepositoryMock = new Mock<IPolicyReadRepository>();
            _currentUserContextMock = new Mock<ICurrentUserContext>();
            _handler = new GetPolicyByIdQueryHandler(_readRepositoryMock.Object, _currentUserContextMock.Object);
        }

        [Fact]
        public async Task Handle_WhenPolicyExists_ShouldReturnPolicy()
        {
            var policyId = Guid.NewGuid();
            var brokerId = Guid.NewGuid(); 

            var policy = new PolicyDetailsDto
            {
                Id = policyId,
                BrokerId = brokerId 
            };

            _currentUserContextMock
               .SetupGet(x => x.BrokerId)
               .Returns(brokerId); 

            _readRepositoryMock
                .Setup(r => r.GetByIdAsync(policyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(policy);

            var result = await _handler.Handle(
                new GetPolicyByIdQuery(policyId),
                CancellationToken.None);

            Assert.Equal(policyId, result.Id);
        }

        [Fact]
        public async Task Handle_WhenPolicyDoesNotExist_ShouldThrowNotFoundException()
        {
            var policyId = Guid.NewGuid();

            _readRepositoryMock
                .Setup(r => r.GetByIdAsync(policyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((PolicyDetailsDto?)null);

            _currentUserContextMock
               .SetupGet(x => x.BrokerId)
               .Returns(Guid.NewGuid());

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(
                    new GetPolicyByIdQuery(policyId),
                    CancellationToken.None));
        }
    }

}
