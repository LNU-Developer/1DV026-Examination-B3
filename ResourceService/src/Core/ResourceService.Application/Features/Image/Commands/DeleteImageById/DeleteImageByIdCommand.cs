using MediatR;

namespace ResourceService.Application.Features.Image.Commands.DeleteImageById
{
    public class DeleteImageByIdCommand : IRequest
    {
        public string Id { get; set; }
        public Guid UserId { get; set; }
    }
}