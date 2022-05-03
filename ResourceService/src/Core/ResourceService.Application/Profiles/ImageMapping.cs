using System.Net.Mime;
using AutoMapper;
using ResourceService.Application.Features.Image.Queries.Common;
using ResourceService.Domain.Entities;

namespace ScroW.Application.Profiles
{
    public class ImageMapping : Profile
    {
        public ImageMapping()
        {
            CreateMap<Image, ImageDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ImageServiceId))
                .ReverseMap();
        }
    }
}