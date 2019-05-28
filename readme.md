# Introduction 

This is a simple program for experimenting with SignalR in a ASP.NET Core Web API.

The solution consists of a .NET Core Web API and a .NET Core console application.  The Web API exposes a SignalR hub to which the console application connects to.

The console application can request the Id of the SignalR connection that it consumes.

The API has an operation which can be used to push messages via SignalR to all connected clients or to one specific connected client.