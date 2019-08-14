using System;
using Rebus.Config;
using Rebus.Sagas.Exclusive;
using Serilog;
// ReSharper disable ArgumentsStyleLiteral

namespace Common
{
    public static class CommonRebusConfigurationExtensions
    {
        public static RebusConfigurer ConfigureEndpoint(this RebusConfigurer configurer, EndpointRole role)
        {
            var dbConnectionString = Environment.GetEnvironmentVariable("REBUSSPIKE_DB");
            var servicebusConnectionString = Environment.GetEnvironmentVariable("REBUSSPIKE_ASB");

            configurer
                .Logging(l => l.Serilog(Log.Logger))
                //.Transport(t =>
                //{
                //    if (role == EndpointRole.Client)
                //    {
                //        t.UseMsmqAsOneWayClient();
                //    }
                //    else
                //    {
                //        t.UseMsmq(Config.AppSetting("QueueName"));
                //    }
                //})
                .Transport(t =>
                {
                    if (role == EndpointRole.Client)
                    {
                        t.UseAzureServiceBusAsOneWayClient(servicebusConnectionString);
                    }
                    else
                    {
                        t.UseAzureServiceBus(servicebusConnectionString, "Rebus_" + Enum.GetName(typeof(EndpointRole), role))
                        .EnablePartitioning();
                    }
                })
                //.Subscriptions(s =>
                //{
                //    var subscriptionsTableName = "Rebus_Subscriptions";

                //    s.StoreInSqlServer(RebusDbConnectionString, subscriptionsTableName, isCentralized: true);
                //})
                .Sagas(s =>
                {
                    if (role != EndpointRole.SagaHost) return;

                    //var dataTableName = ;
                    //var indexTableName = ;

                    // store sagas in SQL Server to make them persistent and survive restarts
                    s.StoreInSqlServer(dbConnectionString, "Rebus_SagaData", "Rebus_SagaIndex");
                    s.EnforceExclusiveAccess();
                })
                
                //.Timeouts(t =>
                //{
                //    if (role == EndpointRole.Client) return;

                //    var tableName = "Rebus_Timeouts";

                //    // store timeouts in SQL Server to make them persistent and survive restarts
                //    t.StoreInSqlServer(RebusDbConnectionString, tableName);
                //})
                ;

            return configurer;
        }
    }
}
