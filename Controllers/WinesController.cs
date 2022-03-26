using api_design_assignment.Models;
using api_design_assignment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_design_assignment.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WinesController : ControllerBase
{
    private readonly WinesService _winesService;

    public WinesController(WinesService winesService) =>
        _winesService = winesService;

    [HttpGet]
    public async Task<List<Wine>> GetAll()
    {
        var wines =   await _winesService.GetAsync();
        
        return wines;
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Wine>> GetById(string id)
    {
        var wine = await _winesService.GetAsync(id);

        if (wine is null)
        {
            return NotFound();
        }

        return Ok(CreateLinksForWine(wine));
    }

    [HttpPost]
    public async Task<IActionResult> Post(Wine wine)
    {  
        await _winesService.CreateAsync(wine);

        return CreatedAtAction(nameof(GetById), new { id = wine.Id }, wine);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Wine updatedWine)
    {
        var wine = await _winesService.GetAsync(id);

        if (wine is null)
        {
            return NotFound();
        }

        updatedWine.Id = wine.Id;

        await _winesService.UpdateAsync(id, updatedWine);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var wine = await _winesService.GetAsync(id);

        if (wine is null)
        {
            return NotFound();
        }

        await _winesService.RemoveAsync(wine.Id);

        return NoContent();
    }
    
    private Wine CreateLinksForWine(Wine wine)
    {
        wine.Links.Add(
            new Link($"https://localhost:3000/api/user/{wine.Id}", "self", "GET" )
        );
        
        wine.Links.Add(
            new Link($"https://localhost:3000/api/user/{wine.Id}", "update", "PUT" )
        );
        
        wine.Links.Add(
            new Link($"https://localhost:3000/api/user/{wine.Id}",
                "delete",
                "DELETE"));

        return wine;
    }
}
