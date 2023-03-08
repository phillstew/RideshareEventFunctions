using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideshareEventFunctions.Models.HubEvents
{
    public class DriverRequestedEvent
    {
        public int RideId { get; set; }

        public DateTimeOffset DateTimeCreated { get; set; } = DateTimeOffset.Now;
    }
}
