using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTO.User;
using School.Application.Interfaces;
using School.Domain.Account;
using SchoolAPI.Models;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IAuthenticate _authenticate;

    public UserController(IUserService userService,  IAuthenticate authenticate)
    {
        _userService = userService;
        _authenticate = authenticate;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetUserById(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        var deletedUser = await _userService.DeleteAsync(id);
        if (deletedUser == null) return NotFound();
        return Ok(deletedUser);
    }

    [HttpPost]
    public async Task<ActionResult> CreateUser(UserPostDto userDto)
    {
        var userExists = await _authenticate.UserExists(userDto.Email);
        if (userExists)
        {
            return BadRequest(new {message = "This email is already in use"});
        }
        var user = await _userService.AddAsync(userDto);
        var token = _authenticate.GenerateToken(user.Id, user.Email.ToLower(), user.Role);
        return Ok(new {Nome = user.Name, token = token});
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser(int id, UserPutDto userDto)
    {
        var updatedUser = await _userService.UpdateAsync(id, userDto);
        if (updatedUser == null) return BadRequest();
        return Ok(updatedUser);
    }

    [HttpPost("login")]
    public async Task<ActionResult> GetUserToken(UserLogin userLogin)
    {
        var user = await _authenticate.GetUserByEmail(userLogin.Email);
        if(user == null) return BadRequest(new{message = "Username or password is incorrect"});

        var validUser = await _authenticate.AuthenticateAsync(userLogin.Email, userLogin.Password);
        if(!validUser) return BadRequest(new {message = "Username or password is incorrect"});

        var token = _authenticate.GenerateToken(user.Id, user.Email.ToLower(), user.Role);
        return Ok(new {Nome = user.Name, Token = token});
    }

    [HttpGet("route-test")]
    [Authorize(Roles = "Admin, Student")]
    public async Task<ActionResult> Test()
    {
        return Ok(new { message = "Usuário logado!" });
    }
}
