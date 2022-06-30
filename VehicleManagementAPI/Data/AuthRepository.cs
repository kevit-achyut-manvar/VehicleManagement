using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using VehicleManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace VehicleManagementAPI.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                        return false;
                }
                return true;
            }
        }

        private string CreateToken(VehicleOwner vehicleOwner)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, vehicleOwner.Id.ToString()),
                new Claim(ClaimTypes.Name, vehicleOwner.Username)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = System.DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public AuthRepository(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            var response = new ServiceResponse<string>();
            var vehicleOwner = await _context.VehicleOwners.FirstOrDefaultAsync(x => x.Username.ToLower().Equals(username.ToLower()));
            if (vehicleOwner == null)
            {
                response.Success = false;
                response.Message = "User not found.";
            }
            else if (!VerifyPasswordHash(password, vehicleOwner.PasswordHash, vehicleOwner.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong Password.";
            }
            else
                response.Data = CreateToken(vehicleOwner);

            return response;
        }

        public async Task<ServiceResponse<int>> Register(VehicleOwner vehicleOwner, string password)
        {
            var response = new ServiceResponse<int>();
            if(await UserExists(vehicleOwner.Username))
            {
                response.Success = false;
                response.Message = "User already exists.";
                return response;
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            vehicleOwner.PasswordHash = passwordHash;
            vehicleOwner.PasswordSalt = passwordSalt;

            _context.VehicleOwners.Add(vehicleOwner);
            await _context.SaveChangesAsync();
  
            response.Data = vehicleOwner.Id;
            response.Message = "User registered successfully.";
            return response;
        }

        public async Task<bool> UserExists(string username)
        {
            if( await _context.VehicleOwners.AnyAsync(x => x.Username.ToLower().Equals(username.ToLower())))
                return true;

            return false;
        }
    }
}
