namespace VehicleManagementAPI.Models
{
    public class VehicleOwner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
        public string Address { get; set; }
        public List<Vehicle> Vehicles { get; set; }
    }
}
