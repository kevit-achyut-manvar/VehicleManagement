using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleManagementAPI.Data;
using VehicleManagementAPI.Dto.Vehicle;
using VehicleManagementAPI.Models;
using VehicleManagementAPI.Services.VehicleServices;

namespace VehicleManagementAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IVehicleService _vehicleService;

        public VehicleController(DataContext context, IVehicleService vehicleService)
        {
            _context = context;
            _vehicleService = vehicleService;
        }

        // GET: api/Vehicle
        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetVehicleDto>>>> GetVehicles()
        {
            return Ok(await _vehicleService.GetAllVehicles());
        }

        // GET: api/Vehicle/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetVehicleDto>>> GetSingleVehicle(int id)
        {
            var temp = await _vehicleService.GetVehicleById(id);

            if (temp.Data == null)
                return NotFound(temp);
            return Ok(temp);
        }

        // PUT: api/Vehicle/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<GetVehicleDto>>> EditVehicle(int id, UpdateVehicleDto updatedVehicle)
        {
            var temp = await _vehicleService.UpdateVehicle(id, updatedVehicle);

            if (id != updatedVehicle.Id)
            {
                return BadRequest(temp);
            }
            
            if (temp.Data == null)
                return NotFound(temp);
            return Ok(temp);
        }

        // POST: api/Vehicle
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetVehicleDto>>>> AddVehicle(AddVehicleDto newVehicle)
        {
            return Ok(await _vehicleService.AddVehicle(newVehicle));
        }

        // DELETE: api/Vehicle/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetVehicleDto>>>> DeleteVehicle(int id)
        {
            var temp = await _vehicleService.DeleteVehicle(id);

            if (temp.Data == null)
                return NotFound(temp);
            return Ok(temp);
        }
    }
}
