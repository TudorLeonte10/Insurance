using Insurance.Application.Abstractions.Repositories;
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

        public GetPolicyByIdQueryHandler(IPolicyReadRepository readRepository)
        {
            _readRepository = readRepository;
        }

        public async Task<PolicyDetailsDto> Handle(
            GetPolicyByIdQuery request,
            CancellationToken ct)
        {
            var policy = await _readRepository
                .GetByIdAsync(request.PolicyId, ct);

            if (policy is null)
                throw new NotFoundException(
                    $"Policy with ID {request.PolicyId} not found.");

            return policy;
        }
    }

}
