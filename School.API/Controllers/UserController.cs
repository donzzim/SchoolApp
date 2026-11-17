using Microsoft.AspNetCore.Mvc;
using School.Application.DTO.User;
using School.Application.Interfaces;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
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
        var createdUser = await _userService.AddAsync(userDto);
        if (createdUser == null) return BadRequest();
        return Ok(createdUser);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser(int id, UserPutDto userDto)
    {
        var updatedUser = await _userService.UpdateAsync(id, userDto);
        if (updatedUser == null) return BadRequest();
        return Ok(updatedUser);
    }
}
