# RideShareEventFunctions

## To run locally

Clone the repository and open in Visual Studio 2022 (or latest). Visual studio is recommended as it automatically installs all of the azure function emulator tools the different functions might be looking for.

To run the functions locally you should be able to set the `RideShareEventFunctions` project to the startup proejct, hit the "Start Debugging" button to run the project and you will see a console window with all of the functions in the project being initialized. From here you can call any of the HTTP methods or send event hub messages to trigger the functions. 

## Local configuration

In the file `settings.local.json` there are two settings:

```
    "AzureEventHubConnectionString": "<hub connection string>",
    "AzureSignalRConnectionString": "<signalr serverless connection string>"
```

To avoid picking up messages that other developers are triggering, it is advized to set up an Event Hub and SignalR Service under your own resource group and fill in these details for yourself.

### Event Hub Configuration

Navigate to your azure portal and go to the `Event Hubs` service. Create a new event hub namespace with the default settings in your resource group.

Once that is created, navigate to `Settings > Shared Access Policies` and click on `RootManagedSharedAccessKey`. From there you can grab the connection string that is used in the above local setting.

In addition to this you need to create different hubs for each event that is used. These hubs act as streams for each event. From the `Event Hub Namespace` screen, navigate to `Entities > Event Hubs`, click create and create a hub for each of the following:

- driverconfirmed
- driverfound
- driverrejected
- driverrequested
- ridecompleted
- ridecreated
- riderpickedup
- ridestateupdated

If time permits we will make a script that handles creating these for us, but for the time it is a manual process.

### SignalR configuration

Navigate to your azure portal and go to the `SignalR` service. Create a new SignalR instance with the mode set to `serverless`.

Once that is created you can go to `Connection Strings` and find the connection string to use in your local settings.


## Test Projects

Included in this repository are two test projects to help simulate the sending of events as well as the JS required to connect to a signalr service and subscribe to a ride status.

### SignalRTestClient

This is a default asp.net web application with one exception, under `Pages > Index.cshtml` page I have included a sample for connecting to a signalr client and receiving updates to an ride's state. 

### EventHubTestConsole

This is a basic console application that will help you send out a simple message to an event hub namespace. To use this with the Event Hub you have set up above, you just need to update the connection string in the code with the one you have created:

```
EventHubProducerClient producerClient = new EventHubProducerClient(
    "<your connection string>",
    "ridecreated");
```