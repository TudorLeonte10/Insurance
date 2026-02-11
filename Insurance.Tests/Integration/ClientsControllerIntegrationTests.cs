using Insurance.Application.Clients.Commands;
using Insurance.Application.Exceptions;
using Insurance.Domain.Clients;
using Insurance.Tests.Integration.Setup;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace Insurance.Tests.Integration
{
    namespace Insurance.Tests.Integration.Clients
    {
        public class ClientsControllerIntegrationTests : IntegrationTestBase, IClassFixture<ControllerTestWebApplicationFactory>
        {
            private readonly HttpClient _client;
            private readonly ControllerTestWebApplicationFactory _factory;

            public ClientsControllerIntegrationTests(
                ControllerTestWebApplicationFactory factory)
            {
                _factory = factory;
                _client = factory.CreateClient();
            }

            [Fact]
            public async Task GetClientById_Should_ReturnClient()
            {
                var (factory, client) = CreateTestContext();

                var clientId = await CreateClientAndGetId(client);

                var response = await client.GetAsync(
                    $"/api/brokers/clients/{clientId}");

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }

            [Fact]
            public async Task UpdateClient_Should_ReturnNoContent()
            {
                var clientId = Guid.NewGuid();

                _factory.MediatorMock
                    .Setup(m => m.Send(
                        It.IsAny<UpdateClientCommand>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(clientId);

                var dto = new
                {
                    Name = "Updated",
                    Email = "updated@test.ro",
                    PhoneNumber = "0799999999",
                    Address = "New Address",
                    IdentificationNumber = "1234567890123"
                };

                var response = await _client.PutAsJsonAsync(
                    $"/api/brokers/clients/{clientId}", dto);

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                _factory.MediatorMock.Verify(
                    m => m.Send(
                        It.Is<UpdateClientCommand>(c =>
                            c.ClientId == clientId),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
            }

            [Fact]
            public async Task UpdateClient_When_NotFound_Should_Return404()
            {
                var clientId = Guid.NewGuid();

                _factory.MediatorMock
                    .Setup(m => m.Send(
                        It.IsAny<UpdateClientCommand>(),
                        It.IsAny<CancellationToken>()))
                    .ThrowsAsync(new NotFoundException("Client not found"));

                var dto = new
                {
                    Name = "Updated",
                    IdentificationNumber = "1234567890123"
                };

                var response = await _client.PutAsJsonAsync(
                    $"/api/brokers/clients/{clientId}", dto);

                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }
    }
}

