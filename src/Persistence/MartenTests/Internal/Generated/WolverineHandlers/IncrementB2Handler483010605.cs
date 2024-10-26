// <auto-generated/>
#pragma warning disable
using Wolverine.Marten.Publishing;

namespace Internal.Generated.WolverineHandlers
{
    // START: IncrementB2Handler483010605
    public class IncrementB2Handler483010605 : Wolverine.Runtime.Handlers.MessageHandler
    {
        private readonly Wolverine.Marten.Publishing.OutboxedSessionFactory _outboxedSessionFactory;

        public IncrementB2Handler483010605(Wolverine.Marten.Publishing.OutboxedSessionFactory outboxedSessionFactory)
        {
            _outboxedSessionFactory = outboxedSessionFactory;
        }



        public override async System.Threading.Tasks.Task HandleAsync(Wolverine.Runtime.MessageContext context, System.Threading.CancellationToken cancellation)
        {
            // The actual message body
            var incrementB2 = (MartenTests.IncrementB2)context.Envelope.Message;

            await using var documentSession = _outboxedSessionFactory.OpenSession(context);
            var eventStore = documentSession.Events;
            
            // Loading Marten aggregate
            var eventStream = await eventStore.FetchForWriting<MartenTests.SelfLetteredAggregate>(incrementB2.SelfLetteredAggregateId, cancellation).ConfigureAwait(false);

            if (eventStream.Aggregate == null) throw new Wolverine.Marten.UnknownAggregateException(typeof(MartenTests.SelfLetteredAggregate), incrementB2.SelfLetteredAggregateId);
            var selfLetteredAggregate = new MartenTests.SelfLetteredAggregate();
            
            // The actual message execution
            var outgoing1 = await eventStream.Aggregate.Handle(incrementB2).ConfigureAwait(false);

            eventStream.AppendOne(outgoing1);
            await documentSession.SaveChangesAsync(cancellation).ConfigureAwait(false);
        }

    }

    // END: IncrementB2Handler483010605
    
    
}

