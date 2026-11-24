using School.Domain.Entities;

namespace School.Domain.Account;

public interface IAuthenticate
{
    string GenerateToken(int id, string email, string role);
    Task<User> GetUserByEmail(string email);
    Task<bool> UserExists(string email);
    Task<bool> AuthenticateAsync(string email, string password);
}