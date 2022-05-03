using MediatR;
using ResourceService.Domain.Entities;

namespace ResourceService.Application.Features.Image.Commands.PartiallyEditImageById
{
    public class PartiallyEditImageByIdCommand : IRequest
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
    }
}