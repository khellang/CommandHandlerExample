using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    public class TimingCommandHandler<TCommand, TResult> : ICommandHandler<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        public TimingCommandHandler(ICommandHandler<TCommand, TResult> innerHandler, ILogger logger)
        {
            InnerHandler = innerHandler;
            Logger = logger;
        }

        private ICommandHandler<TCommand, TResult> InnerHandler { get; }

        private ILogger Logger { get; }

        public async Task<TResult> Handle(TCommand command, CancellationToken cancellationToken)
        {
            Logger.Info($"Executing command '{typeof(TCommand).Name}'.");

            var stopwatch = Stopwatch.StartNew();

            var result = await InnerHandler.Handle(command, cancellationToken);

            var elapsed = stopwatch.Elapsed;

            Logger.Info($"Finished executing command '{typeof(TCommand).Name}' in {elapsed.TotalMilliseconds}ms.");

            return result;
        }
    }
}