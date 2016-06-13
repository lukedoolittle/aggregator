using System;
using Aggregator.Domain.Write;
using Aggregator.Task.Commands;
using SimpleCQRS;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.CommandHandlers
{
    public class DeactivateSensorCommandHandler<TRequest> : 
        CommandHandlerBase<Person, DeactivateSensorCommand<TRequest>>
        where TRequest : Request, new()
    {
        public DeactivateSensorCommandHandler(IRepository<Person> repository) : 
            base(repository)
        {
        }

        public override void Handle(DeactivateSensorCommand<TRequest> command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var person = _repository.GetById(command.AggregateId);
            person.DeactivateSensor<TRequest>();
            _repository.Save(person, command.OriginalVersion);
        }
    }
}
