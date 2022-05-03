using ResourceService.Domain.Entities;

namespace ResourceService.Application.Features.Image.Commands.CreateImage
{
    public class CreateImageResponse
    {
        public string ImageUrl { get; set; }
        public string ContentType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Id { get; set; }
    }
}