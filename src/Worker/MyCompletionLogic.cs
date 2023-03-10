using MessageHandler.Runtime.StreamProcessing;

namespace Worker
{
    public class MyCompletionLogic : ICompleteBatches
    {
        private readonly IDispatchMessages _dispatcher;

        public MyCompletionLogic(IDispatchMessages dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public async Task Complete(IProcessingContext context)
        {
           // await _dispatcher.Dispatch(new[] { new { MyId = Guid.NewGuid() } });
        }
    }
}
