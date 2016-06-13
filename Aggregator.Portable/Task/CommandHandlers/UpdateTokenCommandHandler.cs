using System;
using Aggregator.Domain.Write;
using Aggregator.Task.Commands;
using SimpleCQRS;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.CommandHandlers
{
    public class UpdateTokenCommandHandler<TService> : 
        CommandHandlerBase<Person, UpdateTokenCommand<TService>>
        where TService : Service
    {
        public UpdateTokenCommandHandler(IRepository<Person> repository) :
            base(repository)
        {
        }

        public override void Handle(UpdateTokenCommand<TService> command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var person = _repository.GetById(command.AggregateId);
            person.UpdateToken<TService>(command.NewValues);
            _repository.Save(person, command.OriginalVersion);
        }
    }
}
