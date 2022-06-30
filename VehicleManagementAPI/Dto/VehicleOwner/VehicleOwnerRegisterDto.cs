using VehicleManagementAPI.Models;

namespace VehicleManagementAPI.Dto.VehicleOwner
{
    public class VehicleOwnerRegisterDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
