using MediatR;
using ResourceService.Application.Features.Image.Queries.Common;

namespace ResourceService.Application.Features.Image.Queries.GetImageById
{
    public class GetImageByIdQuery : IRequest<ImageDto>
    {
        public string Id { get; set; }
        public Guid UserId { get; set; }
    }
}