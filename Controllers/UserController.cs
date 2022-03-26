using api_design_assignment.Models;
using api_design_assignment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_design_assignment.Controllers;

[Authorize]
[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    
    public UserController(UserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    public async Task<List<User>> GetAll() => await _userService.GetAsync();
    
    
    [HttpGet("{id:length(24)}", Name = nameof(GetById))]
    public async Task<ActionResult<User>> GetById(string id)
    {
        var user = await _userService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }
        
        return Ok(CreateLinksForUser(user));
    }

    [HttpPost]
    public async Task<IActionResult> Create(User user)
    {
        await _userService.CreateAsync(user);

        return CreatedAtAction(nameof(GetById), new { _id = user.Id }, user);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, User updatedUser)
    {
        var user = await _userService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        updatedUser.Id = user.Id;

        await _userService.UpdateAsync(id, updatedUser);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        await _userService.RemoveAsync(user.Id);

        return NoContent();
    }
    
    [AllowAnonymous]
    [Route("authenticate")]
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] User user)
    {
        var token = _userService.Authenticate(user.Email, user.Password);
        return token == null ? Unauthorized("Wrong email or password") : Ok(new {token});
    }
    
    private User CreateLinksForUser(User user)
    {
        var idObj = new { id = user.Id };
        
        user.Links.Add(
            new Link($"https://localhost:3000/api/user/{idObj.id}", "self", "GET" )
            );

        return user;
    }
}