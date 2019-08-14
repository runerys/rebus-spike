using System.Threading.Tasks;
using Rebus.Handlers;
using SagaDemo.Messages;
using Serilog;
#pragma warning disable 1998

namespace Logger
{
    class EventLogger : IHandleMessages<IEventWithCaseNumber>, IHandleMessages<Brs102MessageReceived>, IHandleMessages<LogMePlease>
    {
        static readonly ILogger Logger = Log.ForContext<EventLogger>();

        public async Task Handle(IEventWithCaseNumber message)
        {
            Logger.Information("Got event {EventName} for case {CaseNumber}", message.GetType().Name, message.CaseNumber);
        }

        public async Task Handle(Brs102MessageReceived message)
        {
            Logger.Information($"Got event Brs 201 for {message.MeteringPointId}");
        }

        public async Task Handle(LogMePlease message)
        {
            Logger.Information($"Got LogMePlease {message.Message}");
        }
    }
}