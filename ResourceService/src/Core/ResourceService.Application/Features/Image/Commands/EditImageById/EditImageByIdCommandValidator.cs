using System.Text.RegularExpressions;
using FluentValidation;

namespace ResourceService.Application.Features.Image.Commands.EditImageById
{
    public class EditImageByIdCommandValidator : AbstractValidator<EditImageByIdCommand>
    {
        public EditImageByIdCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotNull()
                .NotEmpty();

            RuleFor(p => p.UserId)
                .NotNull()
                .NotEmpty();

            RuleFor(p => p.Data)
                .NotNull()
                .Must(IsBase64Encoded)
                .NotEmpty();

            RuleFor(p => p.ContentType)
                .NotNull();

            RuleFor(p => p.Description)
                .NotNull()
                .NotEmpty();

            RuleFor(p => p.Location)
                .NotNull()
                .NotEmpty();
        }

        private static bool IsBase64Encoded(string e)
        {
            e = e.Trim();
            return (e.Length % 4 == 0) && Regex.IsMatch(e, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }
    }
}