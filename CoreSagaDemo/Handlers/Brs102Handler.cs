using System;
using System.Threading.Tasks;
using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Sagas;
using SagaDemo.Messages;
using Serilog;
using Serilog.Core;

namespace SagaDemo.Handlers
{
    public class Brs102Handler : Saga<Brs102SagaData>, IAmInitiatedBy<Brs102MessageReceived>
    {
        static readonly ILogger Logger = Log.ForContext<PayoutSaga>();

        readonly IBus _bus;

        public Brs102Handler(IBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(Brs102MessageReceived message)
        {
            Logger.Information($"Received: {Data.MeteringPointId}, Saga revision: {Data.Revision}");
        }

        protected override void CorrelateMessages(ICorrelationConfig<Brs102SagaData> config)
        {
            config.Correlate<Brs102MessageReceived>(m => m.MeteringPointId, c => c.MeteringPointId);
        }
    }

    public class Brs102SagaData : ISagaData
    {
        public string MeteringPointId { get; set; }

        public bool MeteringPointIsActive { get; set; }

        public Guid Id { get; set; }
        public int Revision { get; set; }
    }
}