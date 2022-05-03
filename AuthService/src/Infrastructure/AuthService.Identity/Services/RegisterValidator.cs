using AuthService.Application.Models.Identity;
using FluentValidation;

namespace AuthService.Identity.Services
{
    public class RegisterValidator : AbstractValidator<RegistrationRequest>
    {
        public RegisterValidator()
        {
            RuleFor(p => p.FirstName)
                .NotNull()
                .NotEmpty();

            RuleFor(p => p.LastName)
                .NotNull()
                .NotEmpty();

            RuleFor(p => p.Username)
                .NotNull()
                .NotEmpty();

            RuleFor(p => p.Email)
                .NotNull()
                .NotEmpty();

            RuleFor(p => p.Password)
                .NotNull()
                .NotEmpty();
        }
    }
}