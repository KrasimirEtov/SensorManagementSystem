using AutoMapper;
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
				.ForMember(dest => dest.IsDeleted, opt => opt.ResolveUsing(src =>
				{
					return src.IsDeleted.HasValue ? src.IsDeleted.Value : false;
				}))
				.ForMember(dest => dest.SensorProperty, opt => opt.ResolveUsing(src =>
				{
					var sensor = new SensorPropertyEntity
					{
						CreatedOn = src.SensorProperty.CreatedOn,
						DeletedOn = src.SensorProperty.DeletedOn,
						Id = src.SensorProperty.Id,
						IsDeleted = src.SensorProperty.IsDeleted.HasValue ? src.SensorProperty.IsDeleted.Value : false,
						MeasureUnit = src.SensorProperty.MeasureUnit,
						ModifiedOn = src.SensorProperty.ModifiedOn,
						Type = src.SensorProperty.Type
					};
					return sensor;
				}))
				.ForMember(dest => dest.UserSensors, opt => opt.Ignore());

			CreateMap<SensorEntity, SensorDTO>()
				.ForMember(dest => dest.SensorProperty, opt => opt.ResolveUsing(src =>
				{
					return new SensorPropertyDTO
					{
						CreatedOn = src.SensorProperty.CreatedOn,
						DeletedOn = src.SensorProperty.DeletedOn,
						Id = src.SensorProperty.Id,
						IsDeleted = src.SensorProperty.IsDeleted,
						MeasureUnit = src.SensorProperty.MeasureUnit,
						ModifiedOn = src.SensorProperty.ModifiedOn,
						Type = src.SensorProperty.Type
					};
				}));

			// SensorPropertyDTO <--> SensorPropertyEntity
			CreateMap<SensorPropertyDTO, SensorPropertyEntity>()
				.ForMember(dest => dest.IsDeleted, opt => opt.ResolveUsing(src =>
				{
					return src.IsDeleted.HasValue ? src.IsDeleted.Value : false;
				}))
				.ForMember(dest => dest.Sensors, opt => opt.Ignore());

			CreateMap<SensorPropertyEntity, SensorPropertyDTO>();
		}
	}
}
