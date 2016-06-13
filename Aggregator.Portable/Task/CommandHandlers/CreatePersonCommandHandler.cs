using System;
using Aggregator.Domain.Write;
using Aggregator.Task.Commands;
using SimpleCQRS;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.CommandHandlers
{
    public class CreatePersonCommandHandler : 
        CommandHandlerBase<Person, CreatePersonCommand>
    {
        public CreatePersonCommandHandler(IRepository<Person> repository) : 
            base(repository)
        {
        }

        public override void Handle(CreatePersonCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            var person = new Person(command.PersonId);
            _repository.Save(person, -1);
        }
    }
}
