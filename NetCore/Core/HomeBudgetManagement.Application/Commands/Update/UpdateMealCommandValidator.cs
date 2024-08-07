﻿using FluentValidation;

namespace HomeBudgetManagement.Application.Commands
{
    public class UpdateMealCommandValidator : AbstractValidator<UpdateMealCommand>
    {
        public UpdateMealCommandValidator()
        {
            RuleFor(obj => obj.Id).NotNull().GreaterThan(0)
                .WithMessage("Id is required");            
            
            RuleFor(obj => obj.Name).NotEmpty()
                .WithMessage("Name is required");

            //Other examples...
            //RuleFor(obj => obj.RequestID).Length(1, 50)
            //    .WithMessage(ResponseCode.InvalidRequestId.IdString)
            //    .NotEmpty()
            //    .WithMessage(ResponseCode.InvalidRequestId.IdString)
            //    .Must((obj, prop) => Guid.TryParse(obj.RequestID, out Guid guid) && guid != Guid.Empty)
            //    .WithMessage(ResponseCode.InvalidRequestId.IdString);


            //RuleFor(obj => obj.NotificationUrl)
            //    .Must(obj => Uri.TryCreate(obj, UriKind.Absolute, out var _))
            //    .Must(obj => obj.Length > 1 && obj.Length <= 255)
            //    .When(obj => !string.IsNullOrEmpty(obj.NotificationUrl))
            //    .WithMessage(ResponseCode.InvalidNotificationUrl.IdString);

            //RuleFor(obj => obj.BeneficiaryEmail)
            //     .Matches(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$")
            //     .When(obj => !string.IsNullOrWhiteSpace(obj.BeneficiaryEmail))
            //     .WithMessage(ResponseCode.InvalidBeneficiaryEmailAddress.IdString);
        }
    }
}
