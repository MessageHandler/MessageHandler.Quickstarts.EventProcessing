using System.Reactive.Linq;
using MessageHandler.Runtime.StreamProcessing;
using Contract;

namespace Worker
{
    public class AverageSensorValuesByDevice : IStandingQueryBuilder
    {
        public IObservable<ActionableContext> Build(IObservable<IProcessingContext> stream)
        {
            // todo: improve
            var query = 
                from context in stream.Select(ctx => new { Processing = ctx, Message = (SensorValueChanged)ctx.Message })
                group context by context.Message.DeviceId into p
                from b in p.Buffer(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10))
                select new ActionableContext(b.Select(bf => bf.Processing).FirstOrDefault())
                {
                    Message = b.Count() > 0 ? new
                    {
                        Count = b.Count(),
                        Average = b.Average(m => m.Message.Value)
                    } : null
                };
            return query;
        }
    }
}
