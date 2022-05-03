using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ResourceService.Application.Contracts;
using ResourceService.Application.Features.Image.Commands.CreateImage;
using ResourceService.Application.Features.Image.Commands.EditImageById;
using ResourceService.Application.Features.Image.Commands.PartiallyEditImageById;

namespace ResourceService.Infrastructure.Image
{
    class ImageServiceClient : IImageServiceClient
    {
        public HttpClient Client { get; }

        public ImageServiceClient(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Client = client;
        }
        public async Task<HttpResponseMessage> CreateImageAsync(CreateImageRequest request)
        {
            var body = new StringContent(JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Formatting = Formatting.Indented
            }));
            body.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return await Client.PostAsync(Client.BaseAddress + "images", body);
        }

        public async Task<HttpResponseMessage> DeleteImageByIdAsync(string id)
        {
            return await Client.DeleteAsync(Client.BaseAddress + "images/" + id);
        }

        public async Task<HttpResponseMessage> EditImageByIdAsync(string id, EditImageByIdRequest request)
        {
            var body = new StringContent(JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Formatting = Formatting.Indented
            }));
            body.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return await Client.PutAsync(Client.BaseAddress + "images/" + id, body);
        }

        public async Task<HttpResponseMessage> GetAllImagesAsync()
        {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, Client.BaseAddress + "images");
            return await Client.SendAsync(requestMessage);
        }

        public async Task<HttpResponseMessage> GetImageByIdAsync(string id)
        {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, Client.BaseAddress + "images/" + id);
            return await Client.SendAsync(requestMessage);
        }

        public async Task<HttpResponseMessage> PartiallyEditImageByIdAsync(string id, PartiallyEditImageByIdRequest request)
        {
            var body = new StringContent(JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Formatting = Formatting.Indented
            }));
            body.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return await Client.PatchAsync(Client.BaseAddress + "images/" + id, body);
        }
    }
}