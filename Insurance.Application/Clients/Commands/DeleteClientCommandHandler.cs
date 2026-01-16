using Insurance.Application.Abstractions;
using Insurance.Domain.Abstractions.Repositories;
using Insurance.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Clients.Commands
{
    public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand, Guid>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteClientCommandHandler(
            IClientRepository clientRepository,
            IUnitOfWork unitOfWork)
        {
            _clientRepository = clientRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(DeleteClientCommand request,CancellationToken cancellationToken)
        {
            var client = await _clientRepository
                .GetByIdAsync(request.ClientId, cancellationToken);

            if (client is null)
                throw new NotFoundException("Client not found");

            await _clientRepository.DeleteAsync(client.Id);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return request.ClientId;
        }

    }
}
