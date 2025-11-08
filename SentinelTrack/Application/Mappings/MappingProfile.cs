using AutoMapper;
using SentinelTrack.Domain.Entities;
using SentinelTrack.Application.DTOs.Response;
using SentinelTrack.Application.DTOs.Request;

namespace SentinelTrack.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Moto, MotoResponse>();
        CreateMap<MotoRequest, Moto>();
        
            
        CreateMap<Yard, YardResponse>();
        CreateMap<YardRequest, Yard>();
        
        CreateMap<User, UserResponse>();
        CreateMap<UserRequest, User>();
    } 
}