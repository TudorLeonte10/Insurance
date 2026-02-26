using AutoMapper;
using Insurance.Application.Abstractions;
using Insurance.Application.Abstractions.Audit;
using Insurance.Application.Abstractions.Loggers;
using Insurance.Application.Authentication;
using Insurance.Application.Exceptions;
using Insurance.Domain.Clients;
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
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IAuditLogger _auditLogger;
        private readonly IAuditLogService _auditLogService;

        public UpdateClientCommandHandler(IClientRepository clientRepository, IUnitOfWork unitOfWork, IAuditLogService auditLogService, IAuditLogger auditLogger, ICurrentUserContext currentUserContext)
        {
            _clientRepository = clientRepository;
            _unitOfWork = unitOfWork;
            _auditLogService = auditLogService;
            _auditLogger = auditLogger;
            _currentUserContext = currentUserContext;
        }

        public async Task<Guid> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            var client = await GetClientOrThrowAsync(request.ClientId, cancellationToken);

            var originalIdentificationNumber = client.IdentificationNumber;

            client.UpdateDetails(
                request.Dto.Name,
                request.Dto.Email,
                request.Dto.PhoneNumber,
                request.Dto.Address,
                request.Dto.IdentificationNumber);

            await _clientRepository.UpdateAsync(client, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await AuditIdentificationNumberChangeIfNeededAsync(client, originalIdentificationNumber, cancellationToken);

            return client.Id;
        }


        private async Task<Client> GetClientOrThrowAsync(Guid clientId, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetByIdAsync(clientId, cancellationToken);

            if (client is null)
                throw new NotFoundException("Client not found");

            var brokerId = _currentUserContext.BrokerId!.Value;
            if (client.BrokerId != brokerId)
                throw new ForbiddenException($"Client with id {clientId} does not belong to the current broker");

            return client;
        }


        private async Task AuditIdentificationNumberChangeIfNeededAsync(Client client, string originalIdentificationNumber, CancellationToken cancellationToken)
        {
            if (originalIdentificationNumber == client.IdentificationNumber)
                return;

            var auditEntry = new AuditEntry
            {
                EntityType = "Client",
                EntityId = client.Id,
                ChangedAt = DateTime.UtcNow,
                ChangedBy = "BrokerId",
                Changes = new[]
                {
            new AuditChangeEntry(
                "IdentificationNumber",
                originalIdentificationNumber,
                client.IdentificationNumber)
        }
            };

            await _auditLogService.LogAsync(auditEntry, cancellationToken);
            _auditLogger.LogAudit("Update", "Client", client.Id);
        }
    }
}
