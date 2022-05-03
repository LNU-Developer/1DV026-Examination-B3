using AutoMapper;

namespace ResourceService.Application.Profiles
{
    public class ErrorMapping : Profile
    {
        public ErrorMapping()
        {
            // CreateMap<ErrorMeddelande, ErrorMessage>()
            //     .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Kod))
            //     .ForMember(dest => dest.ErrorDetails, opt => opt.MapFrom(src => src.Detaljinformation))
            //     .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
            //     .ReverseMap();

            // CreateMap<ErrorDetalj, ErrorDetail>()
            //     .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Kod))
            //     .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
            //     .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Typ))
            //     .ReverseMap();
        }
    }
}