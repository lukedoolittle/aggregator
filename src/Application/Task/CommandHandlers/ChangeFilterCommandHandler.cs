using System;
using Aggregator.Domain.Write;
using Aggregator.Task.Commands;
using Material.Infrastructure;
using SimpleCQRS;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.CommandHandlers
{
    public class ChangeFilterCommandHandler<TRequest> :
        CommandHandlerBase<Person, ChangeFilterCommand<TRequest>>
        where TRequest : Request, new()
    {
        public ChangeFilterCommandHandler(IRepository<Person> repository) :
            base(repository)
        {
        }

        public override void Handle(ChangeFilterCommand<TRequest> command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var person = _repository.GetById(command.AggregateId);
            person.ChangeFilter<TRequest>(command.Samples);
            _repository.Save(person, command.OriginalVersion);
        }
    }
}
