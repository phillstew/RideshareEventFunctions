using RideshareEventFunctions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideshareEventFunctions.Services.Interfaces
{
    public interface IRideService
    {
        /// <summary>
        /// Gets information about a ride
        /// </summary>
        /// <param name="rideId"></param>
        /// <returns></returns>
        Task<Ride> GetRide(int rideId);

        /// <summary>
        /// Creates a new ride if given a valid RideId. 
        /// Status of the ride should be NEW
        /// </summary>
        /// <param name="riderEmail"></param>
        /// <returns></returns>
        Task<Ride> CreateRide(string riderEmail);

        /// <summary>
        /// Updates a Ride to a new status
        /// </summary>
        /// <param name="rideId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<Ride> UpdateRideStatus(int rideId, string status);

        /// <summary>
        /// Assigns a driver to a ride and updates the Ride with that new information
        /// Status should be DRIVER ASSIGNED
        /// </summary>
        /// <param name="riderEmail"></param>
        /// <param name="driverEmail"></param>
        /// <returns></returns>
        Task<Ride> AssignDriverToRide(string riderEmail, string driverEmail);

        /// <summary>
        /// Given a Ride, will return back a valid driver within the parameters TBD
        /// </summary>
        /// <param name="rideId"></param>
        /// <returns></returns>
        Task<Driver> FindDriverForRide(int rideId);

        /// <summary>
        /// Ends the ride and sets the status to COMPLETE. 
        /// Adter this call Rider should be eligable to place a new ride and Driver should be eligable to receive new passengers.
        /// </summary>
        /// <param name="rideId"></param>
        /// <returns></returns>
        Task<Ride> CompleteRide(int rideId);
    }
}
