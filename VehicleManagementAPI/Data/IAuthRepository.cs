using VehicleManagementAPI.Models;

namespace VehicleManagementAPI.Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(VehicleOwner vehicleOwner, string password);
        Task<ServiceResponse<string>> Login(string username, string password);
        Task<bool> UserExists(string username);
    }
}
