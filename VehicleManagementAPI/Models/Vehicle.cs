namespace VehicleManagementAPI.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string ModelName { get; set; }
        public int RTONumber { get; set; }
        public VehicleClass Class { get; set; }
        public VehicleOwner VehicleOwner { get; set; }
    }
}
