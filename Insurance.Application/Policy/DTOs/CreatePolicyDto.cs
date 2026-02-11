using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.DTOs
{
    public class CreatePolicyDto
    {
        public Guid BrokerId { get; set; }
        public Guid ClientId { get; set; }
        public Guid BuildingId { get; set; }
        public Guid CurrencyId { get; set; }

        public decimal BasePremium { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

}
