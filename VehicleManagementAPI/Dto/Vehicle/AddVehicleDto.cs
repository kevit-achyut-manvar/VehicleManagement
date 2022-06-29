﻿using VehicleManagementAPI.Models;

namespace VehicleManagementAPI.Dto.Vehicle
{
    public class AddVehicleDto
    {
        public string CompanyName { get; set; }
        public string ModelName { get; set; }
        public int RTONumber { get; set; }
        public VehicleClass Class { get; set; }
    }
}
