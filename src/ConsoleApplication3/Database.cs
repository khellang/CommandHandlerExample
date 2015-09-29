using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    public interface IDatabase
    {
        Task Save<T>(T entity, CancellationToken cancellationToken);
    }

    public class Database : IDatabase
    {
        public Task Save<T>(T entity, CancellationToken cancellationToken) => Task.FromResult(0);
    }
}