using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideshareEventFunctions.Services.Interfaces
{
    public interface IRideShareEventProducer
    {
        /// <summary>
        /// Sends an event object through event hubs
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hubName"></param>
        /// <param name="eventToSend"></param>
        /// <returns></returns>
        Task SendEvent<T>(string hubName, T eventToSend);
    }
}
