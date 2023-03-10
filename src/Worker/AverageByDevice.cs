using System.Reactive.Linq;
using MessageHandler.Runtime.StreamProcessing;
using System.Text.Json;
using Contract;

namespace Worker
{
    public class AverageByDevice : IStandingQueryBuilder
    {
        public IObservable<ActionableContext> Build(IObservable<IProcessingContext> stream)
        {
            // todo: improve
            var actionQuery = from context in stream.Select(ctx => new { Processing = ctx, Message = JsonSerializer.Deserialize<SensorValueChanged>((JsonElement)ctx.Message) })
                              group context by context.Message.DeviceId into p
                              from b in p.Buffer(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10))
                              select new ActionableContext(b.Select(bf => bf.Processing).FirstOrDefault())
                              {
                                  Message = new
                                  {
                                      Count = b.Count(),
                                      Average = b.Average(m => m.Message.Value)
                                  }
                              };
            return actionQuery;
        }
    }
}
