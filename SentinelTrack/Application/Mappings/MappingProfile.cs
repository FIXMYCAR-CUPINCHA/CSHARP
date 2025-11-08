using AutoMapper;
using SentinelTrack.Domain.Entities;
using SentinelTrack.Application.DTOs.Response;

namespace SentinelTrack.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Moto, MotoResponse>();
        CreateMap<Yard, YardResponse>();
        CreateMap<User, UserResponse>();
    } 
}