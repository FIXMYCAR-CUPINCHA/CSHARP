using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SentinelTrack.Application.DTOs.Request;
using SentinelTrack.Application.DTOs.Response;
using SentinelTrack.Application.Interfaces;
using SentinelTrack.Domain.Entities;
using SentinelTrack.Infrastructure.Context;

namespace SentinelTrack.Application.Services;

public class YardService : IYardService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public YardService(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<YardResponse>> GetAllAsync(int page, int pageSize)
    {
        var yards = await _db.Yards
            .OrderBy(y => y.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return _mapper.Map<IEnumerable<YardResponse>>(yards);
    }

    public async Task<YardResponse?> GetByIdAsync(Guid id)
    {
        var yard = await _db.Yards.FindAsync(id);
        return yard == null ? null : _mapper.Map<YardResponse>(yard);
    }

    public async Task<YardResponse> CreateAsync(YardRequest request)
    {
        var entity = _mapper.Map<Yard>(request);
        entity.Id = Guid.NewGuid();
        _db.Yards.Add(entity);
        await _db.SaveChangesAsync();
        return _mapper.Map<YardResponse>(entity);
    }

    public async Task<bool> UpdateAsync(Guid id, YardRequest request)
    {
        var entity = await _db.Yards.FindAsync(id);
        if (entity == null) return false;
        _mapper.Map(request, entity);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _db.Yards.FindAsync(id);
        if (entity == null) return false;
        _db.Yards.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}