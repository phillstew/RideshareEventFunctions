using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RideshareEventFunctions.EventHubFunctions;
using RideshareEventFunctions.Models.HubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RideshareEventFunctions.Tests.Functions
{
    [TestClass]
    public class RideCreatedTests
    {

        private RideCreated RideCreatedFunction;

        [TestInitialize] 
        public void TestInitialize() 
        { 
            RideCreatedFunction = new RideCreated(new Mock<ILogger<RideCreated>>().Object);
        }

        [TestMethod]
        public async Task Test_RideCreated_ReturnsValidJson()
        {
            // Arrange
            var sentEvent = new RideCreatedEvent { RideId = 2 };
            var eventJson = JsonSerializer.Serialize(sentEvent);

            // Act
            var jsonResultToTest = await RideCreatedFunction.Run(eventJson);

            // Assert
            Assert.IsNotNull(jsonResultToTest);

            var driverFound = JsonSerializer.Deserialize<DriverFoundEvent>(jsonResultToTest);

            Assert.IsNotNull(driverFound);
            Assert.AreEqual(5, driverFound.DriverId);
            // todo: mock driver search and logic once it's actually there for an actual test
        }
    }
}
