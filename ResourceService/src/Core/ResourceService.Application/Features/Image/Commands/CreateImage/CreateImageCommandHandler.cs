using MediatR;
using Newtonsoft.Json;
using ResourceService.Application.Contracts;
using ResourceService.Application.Contracts.Persistence.Entity;
using ResourceService.Application.Exceptions.Exceptions;

namespace ResourceService.Application.Features.Image.Commands.CreateImage
{
    public class CreateImageCommandHandler : IRequestHandler<CreateImageCommand, CreateImageResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IImageServiceClient client;

        public CreateImageCommandHandler(IUnitOfWork unitOfWork, IImageServiceClient client)
        {
            this.unitOfWork = unitOfWork;
            this.client = client;
        }

        public async Task<CreateImageResponse> Handle(CreateImageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new CreateImageCommandValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count > 0)
                    throw new HttpResponseException(400, new ErrorMessage { StatusCode = 400, Message = "The request cannot or will not be processed due to something that is perceived to be a client error (for example validation error)." });

                var createImageRequest = new CreateImageRequest
                {
                    Data = request.Data,
                    ContentType = "image/" + request.ContentType.ToString().ToLower()
                };
                var response = client.CreateImageAsync(createImageRequest).Result;
                var data = response.Content.ReadAsStringAsync(cancellationToken).Result;

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = JsonConvert.DeserializeObject<ErrorMessage>(data);
                    throw new HttpResponseException((int)response.StatusCode, errorResponse);
                }

                var obj = JsonConvert.DeserializeObject<CreateImageResponse>(data);
                await unitOfWork.Images.AddAsync(new Domain.Entities.Image
                {
                    ImageUrl = obj.ImageUrl,
                    Location = request.Location,
                    Description = request.Description,
                    ContentType = request.ContentType,
                    ImageServiceId = obj.Id,
                    UserId = request.UserId
                });
                await unitOfWork.CompleteAsync();
                return obj;
            }
            catch (Exception ex) when (ex is not HttpResponseException)
            {
                throw new HttpResponseException(500, new ErrorMessage { StatusCode = 500, Message = "An unexpected condition was encountered." });
            }
        }
    }
}