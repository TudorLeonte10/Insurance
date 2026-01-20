using AutoMapper;
using Insurance.Application.Abstractions;
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
        private readonly IMapper _mapper;

        public UpdateClientCommandHandler(IClientRepository clientRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            var client = await _clientRepository
                .GetByIdAsync(request.ClientId, cancellationToken);

            if (client is null)
                throw new NotFoundException("Client not found");

            var updatedClient = _mapper.Map(request.Dto, client);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return updatedClient.Id;
        }
    }
}
