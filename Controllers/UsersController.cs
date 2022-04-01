using api_design_assignment.Models;
using api_design_assignment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BC = BCrypt.Net.BCrypt;
namespace api_design_assignment.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;
    
    public UsersController(UserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    public async Task<List<User>> GetAll()
    {       
        var users = await _userService.GetAsync();
        
        var linkedUsers = new List<User>();
        users.ForEach(user => linkedUsers.Add(CreateLinksForUser(user)));
            
        return linkedUsers;
    } 
    
    
    [HttpGet("{id:length(24)}", Name = nameof(GetById))]
    public async Task<ActionResult<User>> GetById(string id)
    {
        var user = await _userService.GetAsync(id);

        if (user is null)
        {
            return NotFound($"No user with id {id} exists");
        }
        
        return Ok(CreateLinksForUser(user));
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Create(User newUser)
    {
        var user = new User
        {
            Id = newUser.Id,
            Email = newUser.Email,
            Password =  BC.HashPassword(newUser.Password)
        };

        try
        {
            await _userService.CreateAsync(user);
        }
        catch(Exception e)
        {
            return Conflict(e.Message);
        }
        
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, CreateLinksForUser(user));
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, User updatedUser)
    {
        var user = await _userService.GetAsync(id);

        if (user is null)
        {
            return NotFound($"No user with id {id} exists");
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
            return NotFound($"No user with id {id} exists");
        }

        await _userService.RemoveAsync(user.Id);

        return NoContent();
    }
    
    [AllowAnonymous]
    [Route("authenticate")]
    [HttpPost]
    public Task<IActionResult> Login([FromBody] User user)
    {
        try
        {
            var token = _userService.Authenticate(user.Email, user.Password);
            return Task.FromResult<IActionResult>(Ok(new {token}));
        }
        catch (Exception e)
        {
            return Task.FromResult<IActionResult>(Unauthorized("Wrong email or password"));
        }
    }
    
    private User CreateLinksForUser(User user)
    {
        var idObj = new { id = user.Id };
        
        user.Links.Add(
            new Link($"https://localhost:3000/api/user/{idObj.id}", "self", "GET" )
            );
        user.Links.Add(
            new Link($"https://localhost:3000/api/user/{user.Id}", "update", "PUT" )
        );
        
        user.Links.Add(
            new Link($"https://localhost:3000/api/user/{user.Id}",
                "delete",
                "DELETE"));

        return user;
    }
}