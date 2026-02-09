using AutoMapper;
using Insurance.Application.Abstractions;
using Insurance.Domain.Clients;
using Insurance.Domain.Exceptions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Insurance.Application.Clients.Commands.CreateClient
{
    public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, Guid>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateClientCommandHandler(IClientRepository clientRepository, IUnitOfWork unitOfWork)
        {
            _clientRepository = clientRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {

            var client = Client.Create(
                request.Dto.Type,
                request.Dto.Name,
                request.Dto.IdentificationNumber,
                request.Dto.Email,
                request.Dto.PhoneNumber,
                request.Dto.Address ?? string.Empty
            );

     
            await _clientRepository.AddAsync(client, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return client.Id;
        }

    }
}

