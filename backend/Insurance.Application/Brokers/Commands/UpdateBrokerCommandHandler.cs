using Insurance.Application.Abstractions;
using Insurance.Application.Exceptions;
using Insurance.Domain.Brokers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Brokers.Commands
{
    public class UpdateBrokerCommandHandler : IRequestHandler<UpdateBrokerCommand, Guid>
    {
        private readonly IBrokerRepository _brokerRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateBrokerCommandHandler(IBrokerRepository brokerRepository, IUnitOfWork unitOfWork)
        {
            _brokerRepository = brokerRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(UpdateBrokerCommand request, CancellationToken cancellationToken)
        {
            var broker = await _brokerRepository.GetByIdAsync(request.brokerId, cancellationToken);

            if (broker == null)
            {
                throw new NotFoundException("Broker not found");
            }

            broker.UpdateDetails(request.brokerDto.Name, request.brokerDto.Email, request.brokerDto.Phone);

            await _brokerRepository.UpdateAsync(broker, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return broker.Id;
        }
    }
}
