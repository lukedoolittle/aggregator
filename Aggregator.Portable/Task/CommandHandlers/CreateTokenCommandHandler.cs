using System;
using Aggregator.Domain.Write;
using Aggregator.Task.Commands;
using SimpleCQRS;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.CommandHandlers
{
    public class CreateTokenCommandHandler<TService> : 
        CommandHandlerBase<Person, CreateTokenCommand<TService>>
        where TService : Service
    {
        public CreateTokenCommandHandler(IRepository<Person> repository) : 
            base(repository)
        {
        }

        public override void Handle(CreateTokenCommand<TService> command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var person = _repository.GetById(command.AggregateId);
            person.CreateToken<TService>(command.Values);
            _repository.Save(person, command.OriginalVersion);
        }
    }
}
