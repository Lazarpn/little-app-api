using LittleApp.Api.Authentication;
using LittleApp.Common.Exceptions;
using LittleApp.Common.Models.UserTask;
using LittleApp.Common.Models.Vote;
using LittleApp.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LittleApp.Api.Controllers;

[Route("api/votes")]
[ApiController]
[Authorize(Policy = Policies.RegisteredUser)]
public class VoteController : BaseController
{
    private readonly VoteManager voteManager;

    public VoteController(VoteManager taskManager)
    {
        this.voteManager = taskManager;
    }

    /// <summary>
    /// Gets all user's votes
    /// </summary>
    /// <returns></returns>
    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VotesMeModel))]
    public async Task<IActionResult> GetVotes()
    {
        var votes = await voteManager.GetVotes(GetCurrentUserId().Value);
        return Ok(votes);
    }

    /// <summary>
    /// Creates a vote
    /// </summary>
    /// <param name="model">VoteCreateModel</param>
    /// <returns>Vote model</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VoteModel))]
    public async Task<IActionResult> Create([FromBody] VoteUpsertModel model)
    {
        var vote = await voteManager.Create(GetCurrentUserId().Value, model);
        return Ok(vote);
    }
}
