using System.Threading;
using Autofac;
using FluentValidation;

namespace ConsoleApplication3
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var container = BuildContainer();

            var mediator = container.Resolve<IMediator>();

            var command = new CreatePerson("Kristian", "Hellang");

            var person = mediator.Handle(command, CancellationToken.None).Result;
        }

        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Database>().As<IDatabase>().InstancePerLifetimeScope();
            builder.RegisterType<ConsoleLogger>().As<ILogger>().InstancePerLifetimeScope();

            // Register the mediator
            builder.Register(c => new Mediator(c.Resolve<IComponentContext>().Resolve))
                .AsImplementedInterfaces()
                .SingleInstance();

            // Register all IValidator<T>
            builder.RegisterAssemblyTypes(typeof (Program).Assembly)
                .AsClosedTypesOf(typeof (IValidator<>))
                .InstancePerLifetimeScope();

            // Register all ICommandHandler<TCommand, TResult>
            builder.RegisterAssemblyTypes(typeof (Program).Assembly)
                .AsClosedTypesOf(typeof (ICommandHandler<,>), "handler")
                .InstancePerLifetimeScope();

            // Decorate command handlers with validation
            builder.RegisterGenericDecorator(
                typeof(ValidationCommandHandler<,>),
                typeof(ICommandHandler<,>), "handler", "validation");

            // Decorate validation command handlers with timing
            builder.RegisterGenericDecorator(
                typeof (TimingCommandHandler<,>),
                typeof (ICommandHandler<,>), "validation");

            return builder.Build();
        }
    }
}