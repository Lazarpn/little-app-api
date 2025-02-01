using AutoMapper;
using LittleApp.Common.Enums;
using LittleApp.Common.Helpers;
using LittleApp.Common.Models.Memory;
using LittleApp.Common.Models.UserTask;
using LittleApp.Data;
using LittleApp.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleApp.Core;

public class MemoryManager
{
    private readonly LittleAppDbContext db;
    private readonly IMapper mapper;

    public MemoryManager(LittleAppDbContext db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

    public async Task<List<MemoryModel>> GetMemories(Guid userId)
    {
        var query = db.Memories.OrderByDescending(m => m.Date);
        return await mapper.ProjectTo<MemoryModel>(query).ToListAsync();
    }

    public async System.Threading.Tasks.Task Create(Guid userId, MemoryCreateModel model)
    {
        var newMemory = new Memory
        {
            Description = model.Description,
            Date = model.Date
        };

        await db.Memories.AddAsync(newMemory);
        await db.SaveChangesAsync();
    }

    public async System.Threading.Tasks.Task Delete(Guid userId, Guid memoryId)
    {
        var memoryExists = await db.Memories.AnyAsync(m => m.Id == memoryId);
        ValidationHelper.MustExist<Memory>(memoryExists);

        await db.Memories.Where(m => m.Id == memoryId).ExecuteDeleteAsync();
    }
}
