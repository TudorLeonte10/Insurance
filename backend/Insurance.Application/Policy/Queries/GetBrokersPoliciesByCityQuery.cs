using Insurance.Application.Policy.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Queries
{
    public class GetBrokersPoliciesByCityQuery() : IRequest<IEnumerable<PolicyByCityDto>>
    {
    }
}
