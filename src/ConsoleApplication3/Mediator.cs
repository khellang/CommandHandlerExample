using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    public interface IMediator
    {
        Task<TResult> Handle<TResult>(ICommand<TResult> command, CancellationToken cancellationToken);
    }

    public class Mediator : IMediator
    {
        public Mediator(Func<Type, object> resolver)
        {
            Resolver = resolver;
        }

        private Func<Type, object> Resolver { get; }

        public Task<TResult> Handle<TResult>(ICommand<TResult> command, CancellationToken cancellationToken)
        {
            var commandType = command.GetType();

            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, typeof(TResult));

            var handler = Resolver.Invoke(handlerType);

            var handleMethod = handlerType.GetMethod("Handle");

            var parameters = new object[] { command, cancellationToken };

            return (Task<TResult>) handleMethod.Invoke(handler, parameters);
        }
    }
}