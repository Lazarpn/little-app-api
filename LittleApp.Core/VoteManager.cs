using AutoMapper;
using LittleApp.Common.Helpers;
using LittleApp.Common.Models.UserTask;
using LittleApp.Common.Models.Vote;
using LittleApp.Data;
using LittleApp.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleApp.Core;

public class VoteManager
{
    private readonly LittleAppDbContext db;
    private readonly IMapper mapper;

    public VoteManager(LittleAppDbContext db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

    public async Task<VotesMeModel> GetVotes(Guid userId)
    {
        var votes = await mapper.ProjectTo<VoteModel>(db.Votes).ToListAsync();
        var model = new VotesMeModel
        {
            Votes = votes,
            UserId = userId
        };

        return model;
    }

    public async Task<VoteModel> Create(Guid userId, VoteUpsertModel model)
    {

        var voteCount = await db.Votes.CountAsync();

        if (voteCount >= 2)
        {
            db.Votes.RemoveRange(db.Votes);
        }

        if (voteCount == 1)
        {
            var voteExists = await db.Votes.AnyAsync(v => v.UserId == userId);
            ValidationHelper.MustNotExist<Vote>(voteExists);
        }

        var vote = new Vote
        {
            UserId = userId,
            Answer = model.Answer
        };

        await db.Votes.AddAsync(vote);
        await db.SaveChangesAsync();

        return await mapper.ProjectTo<VoteModel>(db.Votes.Where(v => v.Id == vote.Id)).FirstAsync();
    }
}
