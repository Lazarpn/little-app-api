using LittleApp.Api.Authentication;
using LittleApp.Api.Controllers;
using LittleApp.Common.Models.Memory;
using LittleApp.Common.Models.UserTask;
using LittleApp.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LittleApp.Api.Controllers;

[Route("api/memories")]
[ApiController]
[Authorize(Policy = Policies.RegisteredUser)]
public class MemoryController : BaseController
{
    private readonly MemoryManager memoryManager;

    public MemoryController(MemoryManager memoryManager)
    {
        this.memoryManager = memoryManager;
    }

    /// <summary>
    /// Gets all user's memories
    /// </summary>
    /// <returns></returns>
    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MemoryModel>))]
    public async Task<IActionResult> GetMemories()
    {
        var memories = await memoryManager.GetMemories(GetCurrentUserId().Value);
        return Ok(memories);
    }

    /// <summary>
    /// Creates a memory
    /// </summary>
    /// <param name="model">UserTaskCreateModel</param>
    /// <returns>UserTask model</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Create([FromBody] MemoryCreateModel model)
    {
         await memoryManager.Create(GetCurrentUserId().Value, model);
        return NoContent();
    }

    /// <summary>
    /// Deletes a memory with a given Id.
    /// </summary>
    /// <param name="memoryId">Task identifier</param>
    /// <returns></returns>
    [HttpDelete("{memoryId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute] Guid memoryId)
    {
        await memoryManager.Delete(GetCurrentUserId().Value, memoryId);
        return NoContent();
    }
}
