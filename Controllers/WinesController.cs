using api_design_assignment.Models;
using api_design_assignment.Services;
using Microsoft.AspNetCore.Mvc;

namespace api_design_assignment.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WinesController : ControllerBase
{
    private readonly WinesService _winesService;

    public WinesController(WinesService winesService) =>
        _winesService = winesService;

    [HttpGet]
    public async Task<List<Wine>> Get() =>
        await _winesService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Wine>> Get(string id)
    {
        var wine = await _winesService.GetAsync(id);

        if (wine is null)
        {
            return NotFound();
        }

        return wine;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Wine newWine)
    {
        await _winesService.CreateAsync(newWine);

        return CreatedAtAction(nameof(Get), new { id = newWine.Id }, newWine);
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
}