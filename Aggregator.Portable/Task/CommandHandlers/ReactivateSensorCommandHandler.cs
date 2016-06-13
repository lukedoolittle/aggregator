using System;
using Aggregator.Domain.Write;
using Aggregator.Task.Commands;
using SimpleCQRS;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.CommandHandlers
{
    public class ReactivateSensorCommandHandler<TRequest> : 
        CommandHandlerBase<Person, ReactivateSensorCommand<TRequest>>
        where TRequest : Request, new()
    {
        public ReactivateSensorCommandHandler(IRepository<Person> repository) : 
            base(repository)
        {
        }

        public override void Handle(ReactivateSensorCommand<TRequest> command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var person = _repository.GetById(command.AggregateId);
            person.ReactivateSensor<TRequest>();
            _repository.Save(person, command.OriginalVersion);
        }
    }
}
