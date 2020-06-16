using AutoMapper;
using SensorManagementSystem.Models.DTOs;
using SensorManagementSystem.Models.Entities;
using SensorManagementSystem.Models.ViewModels;

namespace SensorManagementSystem.Models.AutoMapper
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			// SensorDTO <--> SensorEntity
			CreateMap<SensorDTO, SensorEntity>()
				.ForMember(dest => dest.UserSensors, opt => opt.Ignore());

			CreateMap<SensorEntity, SensorDTO>();

			// SensorPropertyDTO <--> SensorPropertyEntity
			CreateMap<SensorPropertyDTO, SensorPropertyEntity>()
				.ForMember(dest => dest.Sensors, opt => opt.Ignore());

			CreateMap<SensorPropertyEntity, SensorPropertyDTO>();

			// SensorDataDTO <--> SensorEntity
			CreateMap<SensorEntity, SensorDataDTO>()
				.ForMember(dest => dest.MeasureType, opt => opt.MapFrom(src => src.SensorProperty.MeasureType))
				.ForMember(dest => dest.IsSwitch, opt => opt.MapFrom(src => src.SensorProperty.IsSwitch))
				.ForMember(dest => dest.Value, opt => opt.Ignore());

			// SensorEntity <--> SensorViewModel
			CreateMap<SensorEntity, SensorViewModel>();				
			CreateMap<SensorViewModel, SensorEntity>()
				.ForMember(dest => dest.UserSensors, opt => opt.Ignore());

			// SensorPropertyEntity <--> SensorPropertyViewModel
			CreateMap<SensorPropertyEntity, SensorPropertyViewModel>();
			CreateMap<SensorPropertyViewModel, SensorPropertyEntity>()
				.ForMember(dest => dest.Sensors, opt => opt.Ignore());

			// SensorEntity <--> UserSensorViewModel
			CreateMap<UserSensorEntity, CreateUpdateUserSensorViewModel>()
				.ForMember(dest => dest.CustomMaxRangeValue, opt => opt.MapFrom(src => src.MaxRangeValue))
				.ForMember(dest => dest.CustomMinRangeValue, opt => opt.MapFrom(src => src.MinRangeValue))
				.ForMember(dest => dest.CustomPollingInterval, opt => opt.MapFrom(src => src.PollingInterval));
			CreateMap<CreateUpdateUserSensorViewModel, UserSensorEntity>()
				.ForMember(dest => dest.MaxRangeValue, opt => opt.MapFrom(src => src.CustomMaxRangeValue))
				.ForMember(dest => dest.MinRangeValue, opt => opt.MapFrom(src => src.CustomMinRangeValue))
				.ForMember(dest => dest.PollingInterval, opt => opt.MapFrom(src => src.CustomPollingInterval));
		}
	}
}
