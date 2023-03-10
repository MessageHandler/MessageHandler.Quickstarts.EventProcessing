using Worker;
using MessageHandler.Runtime;
using MessageHandler.Runtime.StreamProcessing;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddLogging();

        var storageConnectionString = hostContext.Configuration.GetValue<string>("azurestoragedata")
                                  ?? throw new Exception("No 'TableStorageConnectionString' was provided. Use User Secrets or specify via environment variable.");


        var eventhubsnamespace = hostContext.Configuration.GetValue<string>("eventhubsnamespace")
                                        ?? throw new Exception("No 'eventhubsnamespace' was provided. Use User Secrets or specify via environment variable.");

        services.AddMessageHandler("eventprocessing", runtimeConfiguration =>
        {
            runtimeConfiguration.StreamProcessingPipeline(streaming =>
            {
                streaming
                    .PullMessagesFrom(from => from.EventHub("largehub", "$Default", eventhubsnamespace, storageConnectionString, "leases"))                    
                    .DeserializeMessagesWith(new JSonMessageSerializer())
                    .EnableReactiveProcessing(processing =>
                    {
                        processing.InterpretMessagesUsing<AverageByDevice>();
                       // processing.RespondUsing<MyActionLogic>();
                        processing.CompleteBatchesWith<MyCompletionLogic>();
                    });
            });

            runtimeConfiguration.BufferedDispatchingPipeline(dispatching =>
            {
                dispatching.SerializeMessagesWith(new JSonMessageSerializer());
                dispatching.RouteMessages(to => to.EventHub("largehub", eventhubsnamespace));
            });
        });

        services.AddHostedService<EmulateEventGenerator>();
     
    })
    .Build();

await host.RunAsync();