using Insurance.Application.Authentication;
using Insurance.Application.Exceptions;
using Insurance.Domain.Brokers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Common.Behaviours
{
    public class BrokerValidationBehaviour<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, IRequireBrokerValidation
    {
        private readonly IBrokerRepository _brokerRepository;
        private readonly ICurrentUserContext _currentUserContext;

        public BrokerValidationBehaviour(IBrokerRepository brokerRepository, ICurrentUserContext currentUserContext)
        {
            _brokerRepository = brokerRepository;
            _currentUserContext = currentUserContext;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var brokerId = _currentUserContext.BrokerId
                ?? throw new UnauthorizedException("Broker ID is not available in the current user context.");

            var broker = await _brokerRepository.GetByIdAsync(brokerId, cancellationToken);

            if (broker is null)
            {
                throw new UnauthorizedException("Broker no longer exists");
            }

            if(!broker.IsActive)
            {
                throw new ForbiddenException("Broker is not active");
            }

            return await next();
        }
    }
}
