using MessageHandler.Runtime.StreamProcessing;

namespace Worker
{
    public class MyActionLogic : IAction
    {       
        public async Task Action(ActionableContext context)
        {
            dynamic m = context.Message;
        }
    }
}
