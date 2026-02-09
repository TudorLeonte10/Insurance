using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using Xunit;
using Insurance.Application.Common.Behaviours;

namespace Insurance.Tests.Unit.Common.Behaviours
{
    public class ValidationBehaviourTests
    {
        public class TestRequest : IRequest<int> { }

        [Fact]
        public async Task Given_NoValidators_Should_CallNext()
        {
            var validators = new List<IValidator<TestRequest>>();
            var behaviour = new ValidationBehaviour<TestRequest, int>(validators);

            var nextCalled = false;
            RequestHandlerDelegate<int> next = () =>
            {
                nextCalled = true;
                return Task.FromResult(42);
            };

            var result = await behaviour.Handle(new TestRequest(), next, CancellationToken.None);

            Assert.True(nextCalled);
            Assert.Equal(42, result);
        }

        [Fact]
        public async Task Given_ValidatorWithErrors_Should_ThrowValidationException()
        {
            var validatorMock = new Mock<IValidator<TestRequest>>();
            validatorMock
                .Setup(v => v.Validate(It.IsAny<ValidationContext<TestRequest>>()))
                .Returns(new ValidationResult(new[] {
                    new ValidationFailure("Field", "Error message")
                }));

            var validators = new List<IValidator<TestRequest>> { validatorMock.Object };
            var behaviour = new ValidationBehaviour<TestRequest, int>(validators);

            RequestHandlerDelegate<int> next = () => Task.FromResult(42);

            await Assert.ThrowsAsync<ValidationException>(() =>
                behaviour.Handle(new TestRequest(), next, CancellationToken.None));
        }

        [Fact]
        public async Task Given_ValidatorWithoutErrors_Should_CallNext()
        {
            var validatorMock = new Mock<IValidator<TestRequest>>();
            validatorMock
                .Setup(v => v.Validate(It.IsAny<ValidationContext<TestRequest>>()))
                .Returns(new ValidationResult());

            var validators = new List<IValidator<TestRequest>> { validatorMock.Object };
            var behaviour = new ValidationBehaviour<TestRequest, int>(validators);

            var nextCalled = false;
            RequestHandlerDelegate<int> next = () =>
            {
                nextCalled = true;
                return Task.FromResult(100);
            };

            var result = await behaviour.Handle(new TestRequest(), next, CancellationToken.None);

            Assert.True(nextCalled);
            Assert.Equal(100, result);
        }
    }
}
