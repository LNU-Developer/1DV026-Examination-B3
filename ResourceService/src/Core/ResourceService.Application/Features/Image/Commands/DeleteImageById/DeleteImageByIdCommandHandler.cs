using MediatR;
using Newtonsoft.Json;
using ResourceService.Application.Contracts;
using ResourceService.Application.Contracts.Persistence.Entity;
using ResourceService.Application.Exceptions.Exceptions;

namespace ResourceService.Application.Features.Image.Commands.DeleteImageById
{
    public class DeleteImageByIdCommandHandler : IRequestHandler<DeleteImageByIdCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IImageServiceClient client;

        public DeleteImageByIdCommandHandler(IUnitOfWork unitOfWork, IImageServiceClient client)
        {
            this.unitOfWork = unitOfWork;
            this.client = client;
        }

        public async Task<Unit> Handle(DeleteImageByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new DeleteImageByIdCommandValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count > 0)
                    throw new HttpResponseException(400, new ErrorMessage { StatusCode = 400, Message = "The request cannot or will not be processed due to something that is perceived to be a client error (for example validation error)." });

                var image = await unitOfWork.Images.GetWithFilterAsync(c => c.ImageServiceId == request.Id);
                if (image is null) throw new HttpResponseException(404, new ErrorMessage { StatusCode = 404, Message = "The requested resource was not found." });
                if (image.UserId != request.UserId)
                    throw new HttpResponseException(403, new ErrorMessage
                    {
                        StatusCode = 403,
                        Message = "The request contained valid data and was understood by the server, but the server is refusing action due to the authenticated user not having the necessary permissions for the resource."
                    });
                var response = client.DeleteImageByIdAsync(request.Id).Result;
                var data = response.Content.ReadAsStringAsync(cancellationToken).Result;

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = JsonConvert.DeserializeObject<ErrorMessage>(data);
                    throw new HttpResponseException((int)response.StatusCode, errorResponse);
                }
                unitOfWork.Images.Remove(image);
                await unitOfWork.CompleteAsync();
                return Unit.Value;
            }
            catch (Exception ex) when (ex is not HttpResponseException)
            {
                Console.WriteLine(ex);
                throw new HttpResponseException(500, new ErrorMessage { StatusCode = 500, Message = "An unexpected condition was encountered." });
            }
        }
    }
}