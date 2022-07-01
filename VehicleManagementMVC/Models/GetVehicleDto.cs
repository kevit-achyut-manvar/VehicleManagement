﻿namespace VehicleManagementMVC.Models
{
    public class GetVehicleDto
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string ModelName { get; set; }
        public string RTONumber { get; set; }
        public VehicleClass Class { get; set; }
    }
}
