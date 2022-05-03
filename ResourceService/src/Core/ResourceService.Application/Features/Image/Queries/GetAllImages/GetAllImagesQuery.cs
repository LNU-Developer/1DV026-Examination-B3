using MediatR;
using ResourceService.Application.Features.Image.Queries.Common;

namespace ResourceService.Application.Features.Image.Queries.GetAllImages
{
    public class GetAllImagesQuery : IRequest<List<ImageDto>>
    {
    }
}