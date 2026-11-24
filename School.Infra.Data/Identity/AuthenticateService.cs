using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using School.Domain.Account;
using School.Domain.Entities;
using School.Infra.Data.Context;

namespace School.Infra.Data.Identity;

public class AuthenticateService : IAuthenticate
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthenticateService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public string GenerateToken(int id, string email, string role)
    {
        var claims = new[]
        {
            new Claim("id", id.ToString()),
            new Claim(ClaimTypes.Email, email.ToLower()),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var privateKey =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
        var credentials = new SigningCredentials(privateKey, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken
        (
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _context.User.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower() && u.Deleted == false);
    }

    public Task<bool> UserExists(string email)
    {
        return _context.User.AnyAsync(u => u.Email.ToLower() == email.ToLower() &&  u.Deleted == false);
    }

    public async Task<bool> AuthenticateAsync(string email, string password)
    {
        var user = await _context.User.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower() && u.Deleted == false);
        if (user == null || user.Deleted)
        {
            return false;
        }

        using var hmac = new System.Security.Cryptography.HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i])
            {
                return false;
            }
        }

        return true;
    }
}