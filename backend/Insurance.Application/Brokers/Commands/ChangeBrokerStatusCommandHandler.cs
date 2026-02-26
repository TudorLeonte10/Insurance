using Insurance.Application.Abstractions;
using Insurance.Application.Exceptions;
using Insurance.Domain.Brokers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Brokers.Commands
{
    public class ChangeBrokerStatusCommandHandler : IRequestHandler<ChangeBrokerStatusCommand, Guid>
    {
        private readonly IBrokerRepository _brokerRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ChangeBrokerStatusCommandHandler(IBrokerRepository brokerRepository, IUnitOfWork unitOfWork)
        {
            _brokerRepository = brokerRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(ChangeBrokerStatusCommand request, CancellationToken cancellationToken)
        {
            var broker = await _brokerRepository.GetByIdAsync(request.BrokerId, cancellationToken);

            if (broker == null)
            {
                throw new NotFoundException($"Broker with ID {request.BrokerId} not found.");
            }

            if (request.IsActive)
            {
                broker.Activate();
            }
            else
            {
                broker.Deactivate();
            }

            await _brokerRepository.UpdateAsync(broker, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return broker.Id;
        }
    }
}
