using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Rebus.Activation;
using Rebus.Config;
using SagaDemo.Messages;
using Serilog;

namespace CoreClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                //.WriteTo.ColoredConsole()
                .MinimumLevel.Warning()
                .CreateLogger();

            using (var activator = new BuiltinHandlerActivator())
            {
                var bus = Configure.With(activator)
                    .ConfigureEndpoint(EndpointRole.Client)
                    .Start();

                while (true)
                {
                    Console.Write("Case number > ");
                    var caseNumber = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(caseNumber))
                    {
                        Console.WriteLine("Quitting...");
                        return;
                    }

                    Console.WriteLine(@"Which event to publish?
a) AmountsCalculated
t) TaxesCalculated
p) PayoutMethodSelected
b) BRS102
");
                    var key = ReadKey("atpb");

                    switch (key)
                    {
                        case 'a':
                            bus.Publish(new AmountsCalculated(caseNumber)).Wait();
                            break;
                        case 't':
                            bus.Publish(new TaxesCalculated(caseNumber)).Wait();
                            break;
                        case 'p':
                            bus.Publish(new PayoutMethodSelected(caseNumber)).Wait();
                            break;
                        case 'b':
                            bus.Publish(new Brs102MessageReceived{ MeteringPointId = caseNumber}).Wait();
                            break;
                    }
                }
            }
        }

        static char ReadKey(IEnumerable<char> allowedCharacters)
        {
            var chars = new HashSet<char>(allowedCharacters.Select(char.ToLowerInvariant));

            while (true)
            {
                var key = char.ToLowerInvariant(Console.ReadKey(true).KeyChar);
                if (!chars.Contains(key)) continue;
                return key;
            }
        }
    }
    
}
