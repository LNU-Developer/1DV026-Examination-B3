using MediatR;
using ResourceService.Domain.Entities;

namespace ResourceService.Application.Features.Image.Commands.CreateImage
{
    public class CreateImageCommand : IRequest<CreateImageResponse>
    {
        public string Data { get; set; }
        public ContentType ContentType { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public Guid UserId { get; set; }
    }
}