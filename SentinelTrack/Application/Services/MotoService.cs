using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SentinelTrack.Application.DTOs.Request;
using SentinelTrack.Application.DTOs.Response;
using SentinelTrack.Application.Interfaces;
using SentinelTrack.Domain.Entities;
using SentinelTrack.Infrastructure.Context;

namespace SentinelTrack.Application.Services;

public class MotoService : IMotoService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public MotoService(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<object> GetAllAsync(int page, int pageSize, string? plate, Guid? yardId)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        var q = _db.Motos.AsQueryable();

        if (!string.IsNullOrWhiteSpace(plate))
            q = q.Where(x => x.Plate.Contains(plate));

        if (yardId.HasValue)
            q = q.Where(x => x.YardId == yardId.Value);

        var total = await q.CountAsync();
        var items = await q
            .OrderBy(x => x.Plate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = new
        {
            page,
            pageSize,
            totalItems = total,
            totalPages = Math.Max(1, (int)Math.Ceiling(total / (double)pageSize)),
            items = _mapper.Map<IEnumerable<MotoResponse>>(items)
        };

        return result;
    }

    public async Task<MotoResponse?> GetByIdAsync(Guid id)
    {
        var moto = await _db.Motos.FindAsync(id);
        return moto == null ? null : _mapper.Map<MotoResponse>(moto);
    }

    public async Task<MotoResponse?> CreateAsync(MotoRequest request)
    {
        var yard = await _db.Yards.FindAsync(request.YardId);
        if (yard == null) return null;

        var entity = _mapper.Map<Moto>(request);
        entity.Id = Guid.NewGuid();

        _db.Motos.Add(entity);
        await _db.SaveChangesAsync();

        return _mapper.Map<MotoResponse>(entity);
    }

    public async Task<string?> UpdateAsync(Guid id, MotoRequest request)
    {
        var entity = await _db.Motos.FindAsync(id);
        if (entity == null) return "not_found";

        var yard = await _db.Yards.FindAsync(request.YardId);
        if (yard == null) return "invalid_yard";

        _mapper.Map(request, entity);
        await _db.SaveChangesAsync();
        return null;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _db.Motos.FindAsync(id);
        if (entity == null) return false;

        _db.Motos.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}
