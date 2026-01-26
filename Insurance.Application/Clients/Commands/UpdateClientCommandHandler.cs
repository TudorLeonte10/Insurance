using AutoMapper;
using Insurance.Application.Abstractions;
using Insurance.Application.Abstractions.Audit;
using Insurance.Domain.Abstractions.Repositories;
using Insurance.Domain.Exceptions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Insurance.Application.Clients.Commands
{
    public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, Guid>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuditLogService _auditLogService;

        public UpdateClientCommandHandler(IClientRepository clientRepository, IUnitOfWork unitOfWork, IAuditLogService auditLogService)
        {
            _clientRepository = clientRepository;
            _unitOfWork = unitOfWork;
            _auditLogService = auditLogService;
        }

        public async Task<Guid> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            var client = await _clientRepository
                .GetByIdAsync(request.ClientId, cancellationToken);

            if (client is null)
                throw new NotFoundException("Client not found");


            var exists = await _clientRepository
                .ExistsByIdentifierAsync(request.Dto.IdentificationNumber, cancellationToken);

            if (exists && client.IdentificationNumber != request.Dto.IdentificationNumber)
                throw new ConflictException("Another client with the same identification number already exists");

            var originalIdentificationNumber = client.IdentificationNumber;

            client.UpdateDetails(
                request.Dto.Name,
                request.Dto.Email,
                request.Dto.PhoneNumber,
                request.Dto.Address,
                request.Dto.IdentificationNumber);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var identificationNumberChanged = originalIdentificationNumber != client.IdentificationNumber;

            if (identificationNumberChanged)
            {
                var auditEntry = new AuditEntry
                {
                    EntityType = "Client",
                    EntityId = client.Id,
                    ChangedAt = DateTime.UtcNow,
                    ChangedBy = "BrokerId",
                    Changes = new[]
                    {
                        new AuditChangeEntry("IdentificationNumber", originalIdentificationNumber, client.IdentificationNumber)
                    }
                };

                await _auditLogService.LogAsync(auditEntry, cancellationToken);
            }
            return client.Id;
        }
    }
}
