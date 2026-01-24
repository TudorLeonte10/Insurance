using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Insurance.Application.Common.Behaviours;

namespace Insurance.Tests.Unit.Common.Behaviours
{
    public class LoggingBehaviourTests
    {
        public class TestRequest : IRequest<int>
        {
            public string SomeProp { get; set; } = "X";
        }

        private static void VerifyLoggerLog(Mock<ILogger<LoggingBehaviour<TestRequest, int>>> loggerMock, LogLevel level, Times times)
        {
            loggerMock.Verify(
                x => x.Log(level,It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) =>
                        v != null &&
                        v.ToString() != null &&
                        v.ToString()!.Contains(nameof(TestRequest))),
                    It.IsAny<Exception?>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                times);
        }

        [Fact]
        public async Task Given_Request_Should_InvokeNext_And_LogInformation()
        {
            var loggerMock = new Mock<ILogger<LoggingBehaviour<TestRequest, int>>>();
            var behaviour = new LoggingBehaviour<TestRequest, int>(loggerMock.Object);

            var nextCalled = false;
            RequestHandlerDelegate<int> next = () =>
            {
                nextCalled = true;
                return Task.FromResult(7);
            };

            var request = new TestRequest();

            var result = await behaviour.Handle(request, next, CancellationToken.None);

            Assert.True(nextCalled);
            Assert.Equal(7, result);

            VerifyLoggerLog(loggerMock, LogLevel.Information, Times.AtLeast(2));
        }

        [Fact]
        public async Task Given_Request_WhenNextThrows_Should_LogWarningAndError_And_Rethrow()
        {
            var loggerMock = new Mock<ILogger<LoggingBehaviour<TestRequest, int>>>();
            var behaviour = new LoggingBehaviour<TestRequest, int>(loggerMock.Object);

            var ex = new InvalidOperationException("boom");

            RequestHandlerDelegate<int> next = () =>
            {
                throw ex;
            };

            var request = new TestRequest();

            var thrown = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                behaviour.Handle(request, next, CancellationToken.None));

            Assert.Same(ex, thrown);

            VerifyLoggerLog(loggerMock, LogLevel.Error, Times.AtLeastOnce());
        }
    }
}
