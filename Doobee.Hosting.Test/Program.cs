// See https://aka.ms/new-console-template for more information
using Doobee.Engine.Configuration;

var hostTask = DoobeeConfiguration
    .Create()
    //.UseMemoryStorage()
    .UseFileStorage(AppDomain.CurrentDomain.BaseDirectory + @"\test\data")
    .UseTestListener()
    .UseEngineId(Guid.Parse("a571921b-8536-4056-9412-a2392cbdc2db"))
    .Start();

//var host = await hostTask;