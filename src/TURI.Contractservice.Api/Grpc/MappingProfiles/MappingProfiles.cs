using AutoMapper;

namespace TURI.Contractservice.Grpc.MappingProfiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //   CreateMap<List<AvailableUnitsDto>, AvailableUnitsResult>()
            //     .ForMember(a => a.Units, orig => orig.MapFrom(src => src));
        }
    }
}
