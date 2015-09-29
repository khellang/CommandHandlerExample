using System.Threading;
using System.Threading.Tasks;
using FluentValidation;

namespace ConsoleApplication3
{
    public class CreatePerson : ICommand<Person>
    {
        public CreatePerson(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string FirstName { get; }

        public string LastName { get; }

        public class Handler : ICommandHandler<CreatePerson, Person>
        {
            public Handler(IDatabase database)
            {
                Database = database;
            }

            private IDatabase Database { get; }

            public async Task<Person> Handle(CreatePerson command, CancellationToken cancellationToken)
            {
                var person = new Person(command.FirstName, command.LastName);

                await Database.Save(person, cancellationToken);

                return person;
            }
        }

        public class Validator : AbstractValidator<CreatePerson>
        {
            public Validator()
            {
                RuleFor(x => x.FirstName).NotEmpty();
                RuleFor(x => x.LastName).NotEmpty();
            }
        }
    }
}