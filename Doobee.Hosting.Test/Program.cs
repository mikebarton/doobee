// See https://aka.ms/new-console-template for more information
using Doobee.Engine.Configuration;

var hostTask = DoobeeConfiguration
    .Create()
    .UseMemoryStorage()
    .UseTestListener()
    .Start();

//var host = await hostTask;