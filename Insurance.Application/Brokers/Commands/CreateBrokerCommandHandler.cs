using Insurance.Application.Abstractions;
using Insurance.Domain.Brokers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Brokers.Commands
{
    public class CreateBrokerCommandHandler : IRequestHandler<CreateBrokerCommand, Guid>
    {
        private readonly IBrokerRepository _brokerRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateBrokerCommandHandler(IBrokerRepository brokerRepository, IUnitOfWork unitOfWork)
        {
            _brokerRepository = brokerRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(CreateBrokerCommand request, CancellationToken cancellationToken)
        {
            var broker = Broker.Create(
                request.Dto.BrokerCode,
                request.Dto.Name,
                request.Dto.Email,
                request.Dto.Phone);

            await _brokerRepository.AddAsync(broker, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return broker.Id;
        }
    }
}
