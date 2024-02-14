using AutoMapper;
using enoca_challenge.Dto;
using enoca_challenge.Models;

namespace enoca_challenge.HelperAutoMapper
{
	public class MappingProfiles : Profile
	{
        public MappingProfiles()
        {
            CreateMap<Carriers, CarriersDto>();
            CreateMap<Carrier_Dto, Carriers>();
            CreateMap<CarrierConfigurations, CarrierConfigurationDto>();
			CreateMap<CarrierConfiguration_Dto, CarrierConfigurations>();
			CreateMap<Orders, OrderDto>();
			CreateMap<Order_Dto, Orders>();
		}
    }
}
