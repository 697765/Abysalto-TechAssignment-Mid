using AbySalto.Mid.WebApi.Models;
using FluentValidation;

namespace AbySalto.Mid.WebApi.Validators
{
    public class RegistrationRequestValidator : AbstractValidator<RegistrationRequest>
    {
        public RegistrationRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(2, 20).WithMessage("Name must be between 2 and 20 characters");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname is required")
                .Length(2, 20).WithMessage("Surname must be between 2 and 20 characters");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .Length(2, 20).WithMessage("Username must be between 2 and 20 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(30).WithMessage("Email cannot exceed 30 characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .MaximumLength(20).WithMessage("Password cannot exceed 20 characters")
                .Must(ContainNumber).WithMessage("Password must contain at least one number")
                .Must(ContainSpecialCharacter).WithMessage("Password must contain at least one special character");
        }

        private bool ContainNumber(string password) => password.Any(char.IsDigit);

        private bool ContainSpecialCharacter(string password) => password.Any(ch => !char.IsLetterOrDigit(ch));
    }
}
