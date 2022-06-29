using AutoMapper;
using VehicleManagementAPI.Dto.Vehicle;
using VehicleManagementAPI.Models;

namespace VehicleManagementAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Vehicle, GetVehicleDto>();
            CreateMap<AddVehicleDto, Vehicle>();
        }
    }
}
