using Contract;
using MessageHandler.Runtime.StreamProcessing;

namespace Worker
{
    public class EmulateEventGenerator : IHostedService
    {
        private IDispatchMessages dispatcher;

        public EmulateEventGenerator(IDispatchMessages dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var evt = new SensorValueChangedBuilder()
                                        .WellknownSensor("D98204AA-5328-4078-8D66-354286514994")
                                        .Build();

            await dispatcher.Dispatch(new[] { evt });

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }      
    }
}
