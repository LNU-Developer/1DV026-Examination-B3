using AutoMapper;
using MediatR;
using ResourceService.Application.Contracts.Persistence.Entity;
using ResourceService.Application.Exceptions.Exceptions;
using ResourceService.Application.Features.Image.Queries.Common;

namespace ResourceService.Application.Features.Image.Queries.GetImageById
{
    public class GetAllImagesQueryHandler : IRequestHandler<GetImageByIdQuery, ImageDto>
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public GetAllImagesQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ImageDto> Handle(GetImageByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new GetImageByIdQueryValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count > 0)
                    throw new HttpResponseException(400, new ErrorMessage { StatusCode = 400, Message = "The request cannot or will not be processed due to something that is perceived to be a client error (for example validation error)." });

                var response = await unitOfWork.Images.GetWithFilterAsync(c => c.ImageServiceId == request.Id);
                if (response is null) throw new HttpResponseException(404, new ErrorMessage { StatusCode = 404, Message = "The requested resource was not found." });
                if (response.UserId != request.UserId)
                    throw new HttpResponseException(403, new ErrorMessage
                    {
                        StatusCode = 403,
                        Message = "The request contained valid data and was understood by the server, but the server is refusing action due to the authenticated user not having the necessary permissions for the resource."
                    });
                return mapper.Map<ImageDto>(response);
            }
            catch (Exception ex) when (ex is not HttpResponseException)
            {
                throw new HttpResponseException(500, new ErrorMessage { StatusCode = 500, Message = "An unexpected condition was encountered." });
            }
        }
    }
}