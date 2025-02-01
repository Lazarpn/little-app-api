using AutoMapper;
using LittleApp.Common.Enums;
using LittleApp.Common.Exceptions;
using LittleApp.Common.Helpers;
using LittleApp.Common.Models.UserTask;
using LittleApp.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleApp.Core;

public class TaskManager
{
    private readonly LittleAppDbContext db;
    private readonly IMapper mapper;

    public TaskManager(LittleAppDbContext db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

    public async Task<List<TaskModel>> GetTasks(Guid userId)
    {
        var query = db.Tasks.OrderBy(t => t.Order);
        var tasks = await mapper.ProjectTo<TaskModel>(query).ToListAsync();
        return tasks;
    }

    public async Task<TaskModel> Create(Guid userId, TaskCreateModel model)
    {
        var totalTasks = await db.Tasks.Where(t => t.CompletedAt == null && t.UserId == userId).CountAsync();

        var task = new Entities.Task
        {
            UserId = userId,
            Text = model.Text,
            Order = totalTasks + 1,
        };

        await db.Tasks.AddAsync(task);
        await db.SaveChangesAsync();

        var result = mapper.Map<TaskModel>(task);
        return result;
    }

    public async Task UpdateOrder(Guid userId, Guid taskId, TaskReorderModel model)
    {
        var userTaskFromDb = await db.Tasks.FirstOrDefaultAsync(ut => ut.Id == taskId && ut.UserId == userId);
        ValidationHelper.MustExist(userTaskFromDb);

        var taskDateChanged = model.CompletedAt != userTaskFromDb.CompletedAt;

        if (userTaskFromDb.Order == model.Order && !taskDateChanged)
        {
            return;
        }

        var completedState = userTaskFromDb.CompletedAt != null;

        var affectedTasksQuery = db.Tasks
            .Where(t => t.UserId == userTaskFromDb.UserId
                && ((t.CompletedAt != null) == (taskDateChanged ? !completedState : completedState)));

        var totalTasks = await affectedTasksQuery.CountAsync(ut => ut.UserId == userId);

        if (model.Order > totalTasks + 1)
        {
            throw new ValidationException(ErrorCode.OrderCannotBeMoreThanTotal, new { total = totalTasks });
        }

        using var transaction = await db.Database.BeginTransactionAsync();

        try
        {
            if (taskDateChanged)
            {
                await UpdateOrderAfterRemoval(userId, userTaskFromDb.Order, completedState, userTaskFromDb.CompletedAt);
                await affectedTasksQuery
                    .Where(t => t.Order >= model.Order)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(t => t.Order, t => t.Order + 1)
                        .SetProperty(t => t.ModifiedAt, DateTime.UtcNow));
            }
            else
            {
                var orderHasRaised = userTaskFromDb.Order < model.Order;

                affectedTasksQuery = orderHasRaised
                    ? affectedTasksQuery.Where(t => t.Order > userTaskFromDb.Order && t.Order <= model.Order)
                    : affectedTasksQuery.Where(t => t.Order < userTaskFromDb.Order && t.Order >= model.Order);

                var increaseDecreaseOrder = orderHasRaised ? -1 : 1;

                await affectedTasksQuery
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(t => t.Order, t => t.Order + increaseDecreaseOrder)
                        .SetProperty(t => t.ModifiedAt, DateTime.UtcNow));
            }

            userTaskFromDb.Order = model.Order;
            userTaskFromDb.CompletedAt = model.CompletedAt;

            await db.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw new ValidationException(ErrorCode.ErrorReorderingTask);
        }
    }

    public async Task Delete(Guid userId, Guid taskId)
    {
         var task = await db.Tasks.Where(t => t.UserId == userId && t.Id == taskId).FirstOrDefaultAsync();
        ValidationHelper.MustExist(task);

        var completedState = task.CompletedAt != null;

        using var transaction = await db.Database.BeginTransactionAsync();

        try
        {
            db.Tasks.Remove(task);
            await UpdateOrderAfterRemoval(userId, task.Order, completedState, task.CompletedAt);
            await db.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw new ValidationException(ErrorCode.ErrorReorderingTask);
        }
    }

    private async Task UpdateOrderAfterRemoval(Guid userId, int removedTaskOrder, bool completed, DateTime? taskDate)
    {
        await db.Tasks
            .Where(t => t.UserId == userId
                && ((t.CompletedAt != null) == completed)
                && t.Order > removedTaskOrder)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(t => t.Order, t => t.Order - 1)
                .SetProperty(t => t.ModifiedAt, DateTime.UtcNow));
    }
}
