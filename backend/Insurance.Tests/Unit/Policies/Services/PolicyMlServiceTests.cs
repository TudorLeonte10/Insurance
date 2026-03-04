using Insurance.Application.Policy.DTOs;
using Insurance.Application.Policy.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Insurance.Tests.Unit.Policies.Services
{
    public class PolicyMlServiceTests
    {
        [Fact]
        public async Task AnalyzePolicyAsync_ReturnsResult_WhenApiReturnsSuccess()
        {
            // Arrange
            var expected = new PolicyMlResult
            {
                RawScore = 0.5m,
                RiskScore = 0.7m,
                IsAnomaly = 1
            };

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                   "SendAsync",
                   ItExpr.IsAny<HttpRequestMessage>(),
                   ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(JsonSerializer.Serialize(expected), Encoding.UTF8, "application/json")
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost")
            };

            var svc = new PolicyMlService(httpClient, Mock.Of<IConfiguration>());

            var feature = new AnomalyFeatureDto(); // contents don't matter for this unit test

            // Act
            var result = await svc.AnalyzePolicyAsync(feature, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.RawScore, result.RawScore);
            Assert.Equal(expected.RiskScore, result.RiskScore);
            Assert.Equal(expected.IsAnomaly, result.IsAnomaly);

            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && req.RequestUri.AbsolutePath == "/score"),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task AnalyzePolicyAsync_ThrowsException_WhenApiReturnsNonSuccess()
        {
            // Arrange
            var responseBody = "bad request body";
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                   "SendAsync",
                   ItExpr.IsAny<HttpRequestMessage>(),
                   ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.BadRequest,
                   Content = new StringContent(responseBody, Encoding.UTF8, "text/plain")
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost")
            };

            var svc = new PolicyMlService(httpClient, Mock.Of<IConfiguration>());

            var feature = new AnomalyFeatureDto();

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => svc.AnalyzePolicyAsync(feature, CancellationToken.None));
            Assert.Contains("BadRequest", ex.Message); // check name returned by HttpStatusCode.ToString()
            Assert.Contains(responseBody, ex.Message);

            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && req.RequestUri.AbsolutePath == "/score"),
                ItExpr.IsAny<CancellationToken>());
        }
    }
}
