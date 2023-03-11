using MessageHandler.Runtime.StreamProcessing;

namespace Worker
{
    public class DispatchAveragedSensorValues : ICompleteBatches
    {
        private readonly IDispatchMessages _dispatcher;

        public DispatchAveragedSensorValues(IDispatchMessages dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public async Task Complete(IProcessingContext context)
        {
            await _dispatcher.Dispatch(new[] { context.Message });
        }
    }
}
