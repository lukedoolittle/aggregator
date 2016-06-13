using System;
using SimpleCQRS.Infrastructure;

namespace Aggregator.Task.Commands
{
    public class CreatePersonCommand : Command
    {
        public Guid PersonId { get; private set; }

        public CreatePersonCommand(
            Guid personId,
            int originalVersion) : 
            base(
            personId, 
            originalVersion)
        {
            PersonId = personId;
        }
    }
}
