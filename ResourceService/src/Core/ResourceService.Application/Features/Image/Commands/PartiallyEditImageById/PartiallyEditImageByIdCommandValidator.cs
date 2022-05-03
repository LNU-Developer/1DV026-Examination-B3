using FluentValidation;

namespace ResourceService.Application.Features.Image.Commands.PartiallyEditImageById
{
    public class PartiallyEditImageByIdCommandValidator : AbstractValidator<PartiallyEditImageByIdCommand>
    {
        public PartiallyEditImageByIdCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotNull()
                .NotEmpty();

            RuleFor(p => p.UserId)
                .NotNull()
                .NotEmpty();

            RuleFor(p => p.Description)
                .NotNull()
                .NotEmpty();
        }
    }
}