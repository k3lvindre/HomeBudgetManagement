using FluentValidation;

namespace HomeBudgetManagement.Application.Commands
{
    public class CreateBudgetCommandValidator : AbstractValidator<CreateBudgetCommand>
    {
        public CreateBudgetCommandValidator()
        {
            RuleFor(obj => obj.Description).Length(1, 100).NotEmpty()
                .WithMessage("Provides Description");

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
