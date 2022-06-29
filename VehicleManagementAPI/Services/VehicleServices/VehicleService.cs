using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VehicleManagementAPI.Data;
using VehicleManagementAPI.Dto.Vehicle;
using VehicleManagementAPI.Models;

namespace VehicleManagementAPI.Services.VehicleServices
{
    public class VehicleService : IVehicleService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public VehicleService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetVehicleDto>>> AddVehicle(AddVehicleDto newVehicle)
        {
            var response = new ServiceResponse<List<GetVehicleDto>>();
            Vehicle vehicle = _mapper.Map<Vehicle>(newVehicle);

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            response.Data = await _context.Vehicles.Select(v => _mapper.Map<GetVehicleDto>(v)).ToListAsync();
            response.Message = "Vehicle added successfully.";

            return response;
        }

        public async  Task<ServiceResponse<List<GetVehicleDto>>> DeleteVehicle(int id)
        {
            var response = new ServiceResponse<List<GetVehicleDto>>();

            Vehicle vehicle = await _context.Vehicles.FindAsync(id);

            if(vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();

                response.Data = await _context.Vehicles.Select(v => _mapper.Map<GetVehicleDto>(v)).ToListAsync();
                response.Message = "Vehicle deleted successfully";                
            }
            else
            {
                response.Success = false;
                response.Message = "Vehicle not found.";
            }
            return response; 
        }

        public async Task<ServiceResponse<List<GetVehicleDto>>> GetAllVehicles()
        {
            var response = new ServiceResponse<List<GetVehicleDto>>();
            var dbVehicles = await _context.Vehicles.ToListAsync();

            response.Data = dbVehicles.Select(v => _mapper.Map<GetVehicleDto>(v)).ToList();
            if (dbVehicles == null)
                response.Message = "No vehicle present.";

            return response;
        }

        public async Task<ServiceResponse<GetVehicleDto>> GetVehicleById(int id)
        {
            var response = new ServiceResponse<GetVehicleDto>();
            var dbVehicle = await _context.Vehicles.FindAsync(id);

            response.Data = _mapper.Map<GetVehicleDto>(dbVehicle);
            if (dbVehicle == null)
                response.Message = "Vehicle not found.";
            return response;
        }

        public async Task<ServiceResponse<GetVehicleDto>> UpdateVehicle(int id, UpdateVehicleDto updatedVehicle)
        {
            var response = new ServiceResponse<GetVehicleDto>();
            Vehicle vehicle = await _context.Vehicles.FindAsync(id);

            if(vehicle != null)
            {
                vehicle.ModelName = updatedVehicle.ModelName;
                vehicle.RTONumber = updatedVehicle.RTONumber;
                vehicle.CompanyName = updatedVehicle.CompanyName;
                vehicle.Class = updatedVehicle.Class;

                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<GetVehicleDto>(vehicle);
                response.Message = "Vehicle updated successfully.";
            }
            else
                response.Message = "Vehicle not found.";

            return response;
        }
    }
}
