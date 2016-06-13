using System;
using Aggregator.Domain.Write;
using Aggregator.Task.Commands;
using SimpleCQRS;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.CommandHandlers
{
    public class ChangePollingIntervalCommandHandler<TRequest> : 
        CommandHandlerBase<Person, ChangePollingIntervalCommand<TRequest>>
        where TRequest : Request, new()
    {
        public ChangePollingIntervalCommandHandler(IRepository<Person> repository) :
            base(repository)
        {
        }

        public override void Handle(ChangePollingIntervalCommand<TRequest> command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var person = _repository.GetById(command.AggregateId);
            person.ChangePollingInterval<TRequest>(command.NewPollingInterval);
            _repository.Save(person, command.OriginalVersion);
        }
    }
}
