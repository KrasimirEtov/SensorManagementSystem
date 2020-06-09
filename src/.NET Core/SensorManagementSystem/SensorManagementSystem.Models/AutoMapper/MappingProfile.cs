﻿using AutoMapper;
using SensorManagementSystem.Models.DTOs;
using SensorManagementSystem.Models.Entities;

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
				.ForMember(dest => dest.SensorType, opt => opt.MapFrom(src => src.SensorProperty.Type))
				.ForMember(dest => dest.Value, opt => opt.Ignore());
		}
	}
}