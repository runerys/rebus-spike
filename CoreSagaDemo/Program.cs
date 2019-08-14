using System;
using Castle.Windsor;
using SagaDemo.Installers;
using Serilog;

namespace CoreSagaDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // configure Serilog to log with colors in a fairly compact format
            Log.Logger = new LoggerConfiguration()
                .WriteTo.ColoredConsole(outputTemplate: "{Timestamp:HH:mm:ss} {Message}{NewLine}{Exception}")
                .CreateLogger();

            // create the Windsor container that holds our application
            using (var container = new WindsorContainer())
            {
                container
                    .Install(new RebusHandlersInstaller())
                    .Install(new RebusInstaller())
                    .Install(new StartupActions());

                Console.WriteLine("Press ENTER to quit");
                Console.ReadLine();
            }
        }
    }
}
