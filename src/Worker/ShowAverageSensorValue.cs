using MessageHandler.Runtime.StreamProcessing;

namespace Worker
{
    public class ShowAverageSensorValue : IAction
    {
        public async Task Action(ActionableContext context)
        {
            if (context.Message != null)
            {
                await Console.Out.WriteLineAsync("Average is " + ((dynamic)context.Message).Average);
            }
            else
            {
                await Console.Out.WriteLineAsync("Query yielded no result");
            }
        }
    }
}
