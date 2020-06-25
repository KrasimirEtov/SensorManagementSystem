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

			// UserSensorEntity <--> CreateUpdateUserSensorViewModel
			CreateMap<UserSensorEntity, CreateUpdateUserSensorViewModel>()
				.ForMember(dest => dest.CustomMaxRangeValue, opt => opt.MapFrom(src => src.MaxRangeValue))
				.ForMember(dest => dest.CustomMinRangeValue, opt => opt.MapFrom(src => src.MinRangeValue))
				.ForMember(dest => dest.CustomPollingInterval, opt => opt.MapFrom(src => src.PollingInterval))
				.ForMember(dest => dest.MeasureType, opt => opt.MapFrom(src => src.Sensor.SensorProperty.MeasureType))
				.ForMember(dest => dest.MeasureUnit, opt => opt.MapFrom(src => src.Sensor.SensorProperty.MeasureUnit))
				.ForMember(dest => dest.SensorMaxRangeValue, opt => opt.MapFrom(src => src.Sensor.MaxRangeValue))
				.ForMember(dest => dest.SensorMinRangeValue, opt => opt.MapFrom(src => src.Sensor.MinRangeValue))
				.ForMember(dest => dest.SensorPollingInterval, opt => opt.MapFrom(src => src.Sensor.PollingInterval));
			CreateMap<CreateUpdateUserSensorViewModel, UserSensorEntity>()
				.ForMember(dest => dest.MaxRangeValue, opt => opt.MapFrom(src => src.CustomMaxRangeValue.HasValue ? src.CustomMaxRangeValue : src.SensorMaxRangeValue))
				.ForMember(dest => dest.MinRangeValue, opt => opt.MapFrom(src => src.CustomMinRangeValue.HasValue ? src.CustomMinRangeValue : src.SensorMinRangeValue))
				.ForMember(dest => dest.PollingInterval, opt => opt.MapFrom(src => src.CustomPollingInterval))
				.ForMember(dest => dest.IsAlarmOn, opt => opt.ResolveUsing(src =>
				{
					return src.IsSwitch ? (bool?)null : src.IsAlarmOn;
				}));

			// UserSensorEntity <--> UserSensorViewModel
			CreateMap<UserSensorEntity, UserSensorViewModel>()
				.ForMember(dest => dest.SensorPropertyId, opt => opt.MapFrom(src => src.Sensor.SensorPropertyId)).ReverseMap();
			CreateMap<UserSensorViewModel, UserSensorEntity>();

			// UserSensorTableViewModel <-- UserSensorEntity
			CreateMap<UserSensorEntity, UserSensorTableViewModel>()
				.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
				.ForMember(dest => dest.IsAlarmOn, opt => opt.MapFrom(src => src.IsAlarmOn))
				.ForMember(dest => dest.IsPublic, opt => opt.MapFrom(src => src.IsPublic))
				.ForMember(dest => dest.MaxRangeValue, opt => opt.MapFrom(src => src.MaxRangeValue))
				.ForMember(dest => dest.MinRangeValue, opt => opt.MapFrom(src => src.MinRangeValue))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.PollingInterval, opt => opt.MapFrom(src => src.PollingInterval))
				.ForMember(dest => dest.UpdatedOn, opt => opt.MapFrom(src => src.UpdatedOn))
				.ForMember(dest => dest.Value, opt => opt.ResolveUsing(src =>
				{
					string result = null;

					if (src.Value != null)
					{
						if (double.TryParse(src.Value, out double parsedDoubleSensorValue))
						{
							result = string.Format("{0:0.00}", parsedDoubleSensorValue);
						}
						else if (bool.TryParse(src.Value, out bool parsedBoolSensorValue))
						{
							result = parsedBoolSensorValue ? 1.ToString() : 0.ToString();
						}
					}

					return result;
				}))
				.ForMember(dest => dest.MeasureType, opt => opt.MapFrom(src => src.Sensor.SensorProperty.MeasureType))
				.ForMember(dest => dest.MeasureUnit, opt => opt.MapFrom(src => src.Sensor.SensorProperty.MeasureUnit))
				.ForMember(dest => dest.IsSwitch, opt => opt.MapFrom(src => src.Sensor.SensorProperty.IsSwitch))
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

			// UserSensorGaugeData <-- UserSensorEntity
			CreateMap<UserSensorEntity, UserSensorGaugeData>()
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.Value, opt => opt.ResolveUsing(src =>
				{
					string result = null;

					if (src.Value != null)
					{
						if (double.TryParse(src.Value, out double parsedDoubleSensorValue))
						{
							result = string.Format("{0:0.00}", parsedDoubleSensorValue);
						}
						else if (bool.TryParse(src.Value, out bool parsedBoolSensorValue))
						{
							result = parsedBoolSensorValue ? 1.ToString() : 0.ToString();
						}
					}

					return result;
				}))
				.ForMember(dest => dest.MinRange, opt => opt.ResolveUsing(src =>
				{
					double? result = null;

					if (src.MinRangeValue.HasValue)
					{
						result = src.MinRangeValue;
					}
					else if (!src.MinRangeValue.HasValue && src.Sensor.MinRangeValue.HasValue)
					{
						result = src.Sensor.MinRangeValue;
					}

					return result;
				}))
				.ForMember(dest => dest.MaxRange, opt => opt.ResolveUsing(src =>
				{
					double? result = null;

					if (src.MaxRangeValue.HasValue)
					{
						result = src.MaxRangeValue;
					}
					else if (!src.MaxRangeValue.HasValue && src.Sensor.MaxRangeValue.HasValue)
					{
						result = src.Sensor.MaxRangeValue;
					}

					return result;
				}));
		}
	}
}
