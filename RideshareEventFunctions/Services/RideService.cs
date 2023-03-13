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
            // todo: Connect to database and perform actual search

            return Task.FromResult(new Driver { EmailAddress = "test@tester.com", Name = "John Smith", DriverID = 1 });
        }

        public Task<Ride> GetRide(int rideId)
        {
            // todo: pull from DB

            return Task.FromResult(new Ride { RideId = rideId });
        }

        public async Task<Ride> UpdateRideStatus(int rideId, string status)
        {
            var ride = await GetRide(rideId);

            ride.Status = status;

            // Update DB record

            return ride;
        }
    }
}
