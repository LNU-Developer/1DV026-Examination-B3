using ResourceService.Application.Features.Image.Commands.CreateImage;
using ResourceService.Application.Features.Image.Commands.EditImageById;
using ResourceService.Application.Features.Image.Commands.PartiallyEditImageById;

namespace ResourceService.Application.Contracts
{
    public interface IImageServiceClient
    {
        Task<HttpResponseMessage> GetAllImagesAsync();
        Task<HttpResponseMessage> CreateImageAsync(CreateImageRequest request);
        Task<HttpResponseMessage> GetImageByIdAsync(string id);
        Task<HttpResponseMessage> PartiallyEditImageByIdAsync(string id, PartiallyEditImageByIdRequest request);
        Task<HttpResponseMessage> EditImageByIdAsync(string id, EditImageByIdRequest request);
        Task<HttpResponseMessage> DeleteImageByIdAsync(string id);
    }
}