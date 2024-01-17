using FluentValidation;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HomeBudgetManagement.Application.Behaviors
{
    //the TRequest here is the CreateExpenseCommand or can be any other request that you inject from MediaTr
    public class ValidatorBehavior<TRequest, TResponse>
          : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IValidator<TRequest>[] _validators;
        public ValidatorBehavior(IValidator<TRequest>[] validators) =>
                                                             _validators = validators;

        public async Task<TResponse> Handle(TRequest request,
                                            RequestHandlerDelegate<TResponse> next,
                                            CancellationToken cancellationToken)
        {
            var failures = _validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            if (failures.Any())
            {
                //throw new OrderingDomainException(
                //    $"Command Validation Errors for type {typeof(TRequest).Name}",
                //            new ValidationException("Validation exception", failures));
                throw new Exception(failures.First().ErrorMessage);
            }

            //in IPipelineBehavior the TResponse is what will be return from next call in the pipeline
            //In this example is the CreatePayoutCommandHandler which returns CreateExpenseResponseDto
            //so the TResponse is CreateExpenseResponseDto
            var response = await next();
            return response;
        }
    }
}
    