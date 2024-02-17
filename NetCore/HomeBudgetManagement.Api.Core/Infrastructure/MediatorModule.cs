using Autofac;
using FluentValidation;
using HomeBudgetManagement.Application.Behaviors;
using HomeBudgetManagement.Application.Commands;
using HomeBudgetManagement.Application.DomainEventHandlers;
using MediatR;
using System.Reflection;

namespace HomeBudgetManagement.API.Core.Infrastructure
{
    public class MediatorModule : Autofac.Module
    {
        public MediatorModule()
        {
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            // Register all the Command classes (they implement IRequestHandler) in assembly holding the Commands
            builder.RegisterAssemblyTypes(typeof(CreateBudgetCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            // Register the DomainEventHandler classes (they implement INotificationHandler<>) in assembly holding the Domain Events
            builder.RegisterAssemblyTypes(typeof(ModifiedEventHandler<>).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(INotificationHandler<>));

            //// Register the Command's Validators (Validators based on FluentValidation library)
            builder
                .RegisterAssemblyTypes(typeof(CreateBudgetCommandValidator).GetTypeInfo().Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces();


            //builder.Register<ServiceFactory>(context =>
            //{
            //    var componentContext = context.Resolve<IComponentContext>();
            //    return t =>
            //    {
            //        object o;
            //        return componentContext.TryResolve(t, out o) ? o : null;
            //    };
            //});

            //builder.RegisterGeneric(typeof(CreatingPayoutOnFlyExceptionHandler<,>)).As(typeof(IRequestExceptionHandler<,,>));
            //builder.RegisterGeneric(typeof(PayoutApplicationExceptionHandler<,>)).As(typeof(IRequestExceptionHandler<,,>));
            //builder.RegisterGeneric(typeof(UnmanagedExceptionHandler<,>)).As(typeof(IRequestExceptionHandler<,,>));
            //builder.RegisterGeneric(typeof(CreatingPayoutDbExceptionHandler<,>)).As(typeof(IRequestExceptionHandler<,,>));
            //builder.RegisterGeneric(typeof(CreatedPayoutDbExceptionHandler<,>)).As(typeof(IRequestExceptionHandler<,,>));

            //builder.RegisterGeneric(typeof(RequestExceptionActionProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            //builder.RegisterGeneric(typeof(RequestExceptionProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            //builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            //builder.RegisterGeneric(typeof(BeneficiaryIdBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            //builder.RegisterGeneric(typeof(IdempotenceBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            //builder.RegisterGeneric(typeof(UniqueUtrBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
            //builder.RegisterType<RegisterBeneficiaryBehaviour>().As<IPipelineBehavior<RegisterBeneficiaryCommand, BeneficiaryResponseDTO>>();

            builder.RegisterGeneric(typeof(TransactionBehavior<,>)).As(typeof(IPipelineBehavior<,>));

        }
    }
}
