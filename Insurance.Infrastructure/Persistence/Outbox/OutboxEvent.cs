using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Outbox
{
    public class OutboxEvent
    {
        public Guid Id { get; set; }

        public string EventType { get; set; } = null!;

        public string Payload { get; set; } = null!;

        public DateTime OccurredOn { get; set; }

        public bool Enqueued { get; set; }

        public bool Processed { get; set; }
    }
}
