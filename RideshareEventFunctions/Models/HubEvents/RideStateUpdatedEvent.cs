using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideshareEventFunctions.Models.HubEvents
{
    public class RideStateUpdatedEvent
    {
        public int RideId { get; set; }

        public string State { get; set; }

        public DateTimeOffset DateTimeCreated { get; set; } = DateTimeOffset.Now;
    }
}
