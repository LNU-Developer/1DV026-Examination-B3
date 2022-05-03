using MediatR;
using ResourceService.Domain.Entities;

namespace ResourceService.Application.Features.Image.Commands.EditImageById
{
    public class EditImageByIdCommand : IRequest
    {
        public string Id { get; set; }
        public string Data { get; set; }
        public ContentType ContentType { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public Guid UserId { get; set; }
    }
}