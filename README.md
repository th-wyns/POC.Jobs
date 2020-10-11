# Description
Proof of concept for a custom bakcground job implementation. The implementation uses a Hangfire as the job processor, SignalR for the state communication and MsSql for state persistance. Over the abstraction layers the solution has a Hangfire-Signalr-MsSql sample implementation. These can be replaced with other implementations like MySQL, Azure Functions.

# Criteria
- Should be able to register new jobs
- Jobs should be distigushed by job types for scaling out
- Jobs sbould be pausable and resumable from the last state
- Jobs should be cancelable
- Job states should be searchable/queryable (for eg: show specific user's jobs with a specified type)
- Job state updates should be communicated instantly

# Prerequisities
- Download and install [.NET Core SDK 3.1.201](https://dotnet.microsoft.com/download/dotnet-core/3.1)
- Install MsSql (at least version 2016) on defult instance or update the appsettings file for the test and sample projects if you're using a named instance

# Solution Hierarchy
The soulution has two major folders:

- **src**: contains the source code
- **samples**: cotains the sample application to test the functionality; currently it has a Hangfire-SignalR sample application

## Source Projects
- **POC.Jobs.Agent.Core** : the implementation indepent code base which contains abstractions and interfaces for the agent
- **POC.Jobs.Agent.Hangfire-SignalR** : code base for Hangfire-SignalR based agents
- **POC.Jobs.Core** :  core codebase
- **POC.Jobs.Manager.Core** : abstractions and implementation independent job management code base
- **POC.Jobs.Manager.Hangfire** : Hangfire based job managment implementation
- **POC.Jobs.State.Communication.Core** : abstractions and implementation independent state communication code base
- **POC.Jobs.State.Communication.SignalR** : SignalR based state communication implementation
- **POC.Jobs.State.Storage.Core** : abstractions and implementation independent state persistance code base
- **POC.Jobs.State.Store.EntityFrameworkCore** :  Entity Framework Core based state persistance implementation

## Sample Projects
The solution has a sample for the Hangfire-SignalR-MsSql implementation in the **Jobs.Hangfire.SignalR** folder.

### Jobs.Hangfire.SignalR
- **POC.Jobs.Samples.AgentSvc** : test agent for processing the test job
- **POC.Jobs.Samples.ServerApi** : sample background job Web API for job management and state communication
- **POC.Jobs.Samples.ServerApp** : test console application producing/registering jobs
- **POC.Jobs.Sample.ClientWeb** : sample frontend for job management and state communication


# Running Sample Application
1. Build the **POC.Jobs.Samples** solution.

## Web API
1. Open command prompt
2. Navigate to **artifacts\Debug\POC.Jobs.Samples\ServerApi\netcoreapp3.1**
3. Run **POC.Jobs.Samples.ServerApi.exe**
4. Ensure the Web Api is running by opening a web browser and navigate to **https://localhost:5001/api/job**

## Agent
1. Open command prompt
2. Navigate to **artifacts\Debug\POC.Jobs.Samples.AgentSvc\netcoreapp3.1**
3. Run **POC.Jobs.Samples.AgentSvc.exe**
4. Ensure there is no error on the output and the program keeps running

## Web Client
1. Open command prompt
2. Navigate to **\samples\Jobs.Hangfire.SignalR\ClientWeb**
3. Run **npm install**
4. Run **npm start**
5. Open **http://localhost:4200/**

On the web interface you can:
- create new jobs with the **Queue New** button
- select jobs with the **Id** textbox
- cancel the selected job with the **Cancel** button
- pause the selected job with the **Pause** button
- resume the selected job with the **Resume** button
- monitor live status in the table below the buttons

## Job Producer
1. Open command prompt
2. Navigate to **artifacts\Debug\POC.Jobs.Samples.ServerApp\netcoreapp3.1**
3. Run **POC.Jobs.Samples.ServerApp.exe**

This application will produce a new job every 5 seconds and you can monitor and handle it with the **Web Client** sample application.
