using AutoMapper;
using MediatR;
using ResourceService.Application.Contracts.Persistence.Entity;
using ResourceService.Application.Exceptions.Exceptions;
using ResourceService.Application.Features.Image.Queries.Common;

namespace ResourceService.Application.Features.Image.Queries.GetAllImages
{
    public class GetAllImagesQueryHandler : IRequestHandler<GetAllImagesQuery, List<ImageDto>>
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public GetAllImagesQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        public async Task<List<ImageDto>> Handle(GetAllImagesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await unitOfWork.Images.GetAllAsync();
                return mapper.Map<List<ImageDto>>(response);
            }
            catch (Exception ex) when (ex is not HttpResponseException)
            {
                throw new HttpResponseException(500, new ErrorMessage { StatusCode = 500, Message = "An unexpected condition was encountered." });
            }

        }
    }
}