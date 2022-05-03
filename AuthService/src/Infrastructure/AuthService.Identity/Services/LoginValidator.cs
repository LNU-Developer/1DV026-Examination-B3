using AuthService.Application.Models.Identity;
using FluentValidation;

namespace AuthService.Identity.Services
{
    public class LoginValidator : AbstractValidator<AuthenticationRequest>
    {
        public LoginValidator()
        {
            RuleFor(p => p.Username)
                .NotNull()
                .NotEmpty();

            RuleFor(p => p.Password)
                .NotNull()
                .NotEmpty();
        }
    }
}