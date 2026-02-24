using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Authentication;
using Insurance.Application.Exceptions;
using Insurance.Application.Policy.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Queries
{
    public class GetPolicyByIdQueryHandler
    : IRequestHandler<GetPolicyByIdQuery, PolicyDetailsDto>
    {
        private readonly IPolicyReadRepository _readRepository;
        private readonly ICurrentUserContext _currentUserContext;

        public GetPolicyByIdQueryHandler(
            IPolicyReadRepository readRepository,
            ICurrentUserContext currentUserContext)
        {
            _readRepository = readRepository;
            _currentUserContext = currentUserContext;
        }


        public async Task<PolicyDetailsDto> Handle(
            GetPolicyByIdQuery request,
            CancellationToken ct)
        {
            var brokerId = _currentUserContext.BrokerId;

            var policy = await _readRepository
                .GetByIdAsync(request.PolicyId, ct);

            if (policy is null)
                throw new NotFoundException(
                    $"Policy with ID {request.PolicyId} not found.");

            if (policy.BrokerId != brokerId)
                throw new ForbiddenException(
                    "Policy does not belong to the current broker.");

            return policy;
        }
    }

}
