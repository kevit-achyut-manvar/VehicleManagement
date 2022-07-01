using System.Text.Json.Serialization;

namespace VehicleManagementMVC.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum VehicleClass
    {
        Compact = 1,
        Sedan = 2,
        SUV = 3,
        Luxury = 4
    }
}
