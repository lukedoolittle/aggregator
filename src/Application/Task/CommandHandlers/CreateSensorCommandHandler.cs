using System;
using Aggregator.Domain.Write;
using Aggregator.Task.Commands;
using Material.Infrastructure;
using SimpleCQRS;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.CommandHandlers
{
    public class CreateSensorCommandHandler<TRequest> : 
        CommandHandlerBase<Person, CreateSensorCommand<TRequest>>
        where TRequest : Request, new()
    {
        public CreateSensorCommandHandler(IRepository<Person> repository) : 
            base(repository)
        {
        }

        public override void Handle(CreateSensorCommand<TRequest> command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var person = _repository.GetById(command.AggregateId);
            person.AddSensor<TRequest>(
                command.PollingInterval, 
                command.RequiresAuthentication);
            _repository.Save(person, command.OriginalVersion);
        }
    }
}
