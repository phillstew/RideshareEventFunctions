using RideshareEventFunctions.Models;
using RideshareEventFunctions.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideshareEventFunctions.Services
{
    internal class RideService : IRideService
    {
        public Task<Ride> AssignDriverToRide(string riderEmail, string driverEmail)
        {
            throw new NotImplementedException();
        }

        public Task<Ride> CompleteRide(int rideId)
        {
            throw new NotImplementedException();
        }

        public Task<Ride> CreateRide(string riderEmail)
        {
            throw new NotImplementedException();
        }

        public Task<Driver> FindDriverForRide(int rideId)
        {
            throw new NotImplementedException();
        }

        public Task<Ride> GetRide(int rideId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRideStatus(Ride ride, string status)
        {
            throw new NotImplementedException();
        }
    }
}
