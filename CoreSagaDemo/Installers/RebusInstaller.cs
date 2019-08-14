using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Common;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using SagaDemo.Messages;

namespace SagaDemo.Installers
{
    public class RebusInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            Configure.With(new CastleWindsorContainerAdapter(container))
                .ConfigureEndpoint(EndpointRole.SagaHost)
                .Routing(r => r.TypeBased().Map<LogMePlease>("rebus_subscriber")) // command must be routed. Pattern messages in project local to endpoint
                .Start();
        }
    }
}