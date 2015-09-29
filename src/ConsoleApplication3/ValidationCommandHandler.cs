using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;

namespace ConsoleApplication3
{
    public class ValidationCommandHandler<TCommand, TResult> : ICommandHandler<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        public ValidationCommandHandler(ICommandHandler<TCommand, TResult> innerHandler, IEnumerable<IValidator<TCommand>> validators)
        {
            InnerHandler = innerHandler;
            Validators = validators;
        }

        private ICommandHandler<TCommand, TResult> InnerHandler { get; }

        private IEnumerable<IValidator<TCommand>> Validators { get; }

        public async Task<TResult> Handle(TCommand command, CancellationToken cancellationToken)
        {
            // Run all validators at once.
            var results = await Task.WhenAll(Validators
                .Select(validator => validator.ValidateAsync(command))
                .ToArray());

            // Collect all errors.
            var errors = results.Where(x => !x.IsValid)
                .SelectMany(x => x.Errors)
                .ToArray();

            if (errors.Any())
            {
                // Validation failed, throw exception.
                throw new ValidationException(errors);
            }

            // Continue to next handler.
            return await InnerHandler.Handle(command, cancellationToken);
        }
    }
}