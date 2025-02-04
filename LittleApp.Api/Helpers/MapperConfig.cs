using AutoMapper;
using LittleApp.Common.Models;
using LittleApp.Common.Models.Memory;
using LittleApp.Common.Models.UserTask;
using LittleApp.Common.Models.Vote;
using LittleApp.Entities;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<LittleApp.Entities.Task, TaskModel>();
        CreateProjection<Memory, MemoryModel>();
        CreateProjection<Vote, VoteModel>();
    }
}
