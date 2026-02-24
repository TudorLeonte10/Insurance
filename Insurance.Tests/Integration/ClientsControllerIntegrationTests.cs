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
        }
    }

