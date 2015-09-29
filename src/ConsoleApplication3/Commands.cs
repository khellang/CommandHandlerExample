using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    public interface ICommand<TResult> { }

    public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        Task<TResult> Handle(TCommand command, CancellationToken cancellationToken);
    }
}