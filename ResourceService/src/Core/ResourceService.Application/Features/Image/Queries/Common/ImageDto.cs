
using ResourceService.Domain.Entities;

namespace ResourceService.Application.Features.Image.Queries.Common
{
    public class ImageDto
    {
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }
    }
}