using FluentValidation;

namespace ResourceService.Application.Features.Image.Commands.DeleteImageById
{
    public class DeleteImageByIdCommandValidator : AbstractValidator<DeleteImageByIdCommand>
    {
        public DeleteImageByIdCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotNull()
                .NotEmpty();

            RuleFor(p => p.UserId)
                .NotNull()
                .NotEmpty();
        }
    }
}