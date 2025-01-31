using AutoMapper;
using LittleApp.Common.Models;
using LittleApp.Common.Models.UserTask;
using LittleApp.Entities;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<LittleApp.Entities.Task, TaskModel>();
    }
}
