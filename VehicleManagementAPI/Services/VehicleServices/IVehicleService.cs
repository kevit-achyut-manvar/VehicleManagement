using VehicleManagementAPI.Dto.Vehicle;
using VehicleManagementAPI.Models;

namespace VehicleManagementAPI.Services.VehicleServices
{
    public interface IVehicleService
    {
        Task<ServiceResponse<List<GetVehicleDto>>> GetAllVehicles();
        Task<ServiceResponse<GetVehicleDto>> GetVehicleById(int id);
        Task<ServiceResponse<List<GetVehicleDto>>> AddVehicle(AddVehicleDto newVehicle);
        Task<ServiceResponse<GetVehicleDto>> UpdateVehicle(int id, ServiceResponse<UpdateVehicleDto> updatedVehicle);
        Task<ServiceResponse<List<GetVehicleDto>>> DeleteVehicle(int id);
    }
}
