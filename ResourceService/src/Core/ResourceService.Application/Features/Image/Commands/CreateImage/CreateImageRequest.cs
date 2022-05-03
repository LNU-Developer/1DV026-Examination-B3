using ResourceService.Domain.Entities;

namespace ResourceService.Application.Features.Image.Commands.CreateImage
{
    public class CreateImageRequest
    {
        public string Data { get; set; }
        public string ContentType { get; set; }
    }
}