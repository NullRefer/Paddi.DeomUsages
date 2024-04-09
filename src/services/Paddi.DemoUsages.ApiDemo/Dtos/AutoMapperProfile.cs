using AutoMapper;

namespace Paddi.DemoUsages.ApiDemo.Dtos
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Dict, DictDto>();
        }
    }
}
