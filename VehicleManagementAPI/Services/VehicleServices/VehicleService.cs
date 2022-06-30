using System.Security.Claims;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VehicleService(DataContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<List<GetVehicleDto>>> AddVehicle(AddVehicleDto newVehicle)
        {
            var response = new ServiceResponse<List<GetVehicleDto>>();

            try
            {
                Vehicle vehicle = _mapper.Map<Vehicle>(newVehicle);

                vehicle.VehicleOwner = await _context.VehicleOwners.FirstOrDefaultAsync(v => v.Id == GetUserId());

                _context.Vehicles.Add(vehicle);
                await _context.SaveChangesAsync();

                response.Data = await _context.Vehicles.Where(v => v.VehicleOwner.Id == GetUserId()).Select(v => _mapper.Map<GetVehicleDto>(v)).ToListAsync();
                response.Message = "Vehicle added successfully.";
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async  Task<ServiceResponse<List<GetVehicleDto>>> DeleteVehicle(int id)
        {
            var response = new ServiceResponse<List<GetVehicleDto>>();

            try
            {
                Vehicle vehicle = await _context.Vehicles.FirstAsync(v => v.Id == id && v.VehicleOwner.Id == GetUserId());

                if (vehicle != null)
                {
                    _context.Vehicles.Remove(vehicle);
                    await _context.SaveChangesAsync();

                    response.Data = await _context.Vehicles.Where(v => v.VehicleOwner.Id == GetUserId()).Select(v => _mapper.Map<GetVehicleDto>(v)).ToListAsync();
                    response.Message = "Vehicle deleted successfully";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Vehicle not found.";
                }
            }
            catch(Exception ex)
            {
                response.Success= false;
                response.Message = ex.Message;
            }
            return response; 
        }

        public async Task<ServiceResponse<List<GetVehicleDto>>> GetAllVehicles()
        {
            var response = new ServiceResponse<List<GetVehicleDto>>();
            var dbVehicles = await _context.Vehicles.Where(v => v.VehicleOwner.Id == GetUserId()).ToListAsync();

            response.Data = dbVehicles.Select(v => _mapper.Map<GetVehicleDto>(v)).ToList();
            if (dbVehicles.Count == 0)
                response.Message = "No vehicle present.";

            return response;
        }

        public async Task<ServiceResponse<GetVehicleDto>> GetVehicleById(int id)
        {
            var response = new ServiceResponse<GetVehicleDto>();
            var dbVehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == id && v.VehicleOwner.Id == GetUserId());

            response.Data = _mapper.Map<GetVehicleDto>(dbVehicle);
            if (dbVehicle == null)
                response.Message = "Vehicle not found.";
            return response;
        }

        public async Task<ServiceResponse<GetVehicleDto>> UpdateVehicle(int id, UpdateVehicleDto updatedVehicle)
        {
            var response = new ServiceResponse<GetVehicleDto>();

            if(id != updatedVehicle.Id)
            {
                response.Message = "ID field is not same in edit request.";
                return response;
            }

            try
            {
                Vehicle vehicle = await _context.Vehicles.Include(v => v.VehicleOwner).FirstOrDefaultAsync(v => v.Id == updatedVehicle.Id);

                if (vehicle.VehicleOwner.Id == GetUserId())
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
                {
                    response.Success = false;
                    response.Message = "Vehicle not found.";
                }
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
