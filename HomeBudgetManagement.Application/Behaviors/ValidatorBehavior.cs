﻿using FluentValidation;
using MediatR;

namespace HomeBudgetManagement.Application.Behaviors
{
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

            var response = await next();
            return response;
        }
    }
}
    