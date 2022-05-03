using FluentValidation;

namespace ResourceService.Application.Features.Image.Queries.GetImageById
{
    public class GetImageByIdQueryValidator : AbstractValidator<GetImageByIdQuery>
    {
        public GetImageByIdQueryValidator()
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