using Insurance.Application.Brokers.Commands;
using Insurance.Application.Brokers.DTOs;
using Insurance.Application.Brokers.Queries;
using Insurance.Application.Common.Paging;
using Insurance.Domain.Clients;
using Insurance.Infrastructure.Persistence;
using Insurance.Tests.Integration.Setup;
using Insurance.WebApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace Insurance.Tests.Integration
{
    public class BrokersControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly BrokersController _controller;

        public BrokersControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new BrokersController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Create_Should_CallMediator_And_ReturnCreatedAtAction()
        {
            var brokerId = Guid.NewGuid();
            var dto = new CreateBrokerDto
            {
                BrokerCode = "BR001",
                Name = "Test Broker",
                Email = "test@test.com",
                Phone = "0712345678"
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateBrokerCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(brokerId);

            var result = await _controller.Create(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetById), createdResult.ActionName);
            Assert.Equal(brokerId, createdResult.RouteValues["brokerId"]);

            _mediatorMock.Verify(
                m => m.Send(It.Is<CreateBrokerCommand>(c => c.Dto == dto), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetById_WhenBrokerExists_Should_ReturnOk()
        {
            var brokerId = Guid.NewGuid();
            var brokerDto = new BrokerDetailsDto
            {
                Id = brokerId,
                BrokerCode = "BR001",
                Name = "Test",
                Email = "test@test.com",
                Phone = "123",
                IsActive = true
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetBrokerByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(brokerDto);

            var result = await _controller.GetById(brokerId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(brokerDto, okResult.Value);
        }

        [Fact]
        public async Task GetById_WhenBrokerNotFound_Should_ReturnNotFound()
        {
            var brokerId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetBrokerByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((BrokerDetailsDto?)null);

            var result = await _controller.GetById(brokerId);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetAll_Should_ReturnOkWithPagedResult()
        {
            var pagedResult = new PagedResult<BrokerDetailsDto>(
                new[] { new BrokerDetailsDto { Id = Guid.NewGuid(), Name = "Test" } },
                1,
                10,
                1);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetBrokersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedResult);

            var result = await _controller.GetAll(1, 10);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(pagedResult, okResult.Value);
        }


        [Fact]
        public async Task Deactivate_Should_CallMediator_And_ReturnNoContent()
        {
            var brokerId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ChangeBrokerStatusCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(Guid.NewGuid()));

            var result = await _controller.Deactivate(brokerId);

            Assert.IsType<NoContentResult>(result);
            _mediatorMock.Verify(
                m => m.Send(It.Is<ChangeBrokerStatusCommand>(c => c.BrokerId == brokerId && c.IsActive == false), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Activate_Should_CallMediator_And_ReturnNoContent()
        {
            var brokerId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ChangeBrokerStatusCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(Guid.NewGuid()));

            var result = await _controller.Activate(brokerId);

            Assert.IsType<NoContentResult>(result);
            _mediatorMock.Verify(
                m => m.Send(It.Is<ChangeBrokerStatusCommand>(c => c.BrokerId == brokerId && c.IsActive == true), It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}



