using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SentinelTrack.Application.DTOs.Request;
using SentinelTrack.Application.DTOs.Response;
using SentinelTrack.Application.Interfaces;
using SentinelTrack.Domain.Entities;
using SentinelTrack.Infrastructure.Context;

namespace SentinelTrack.Application.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public UserService(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<(IEnumerable<UserResponse> Items, int Total)> GetAllAsync(int page, int pageSize, string? email)
    {
        var query = _db.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(email))
            query = query.Where(x => x.Email.Contains(email));

        var total = await query.CountAsync();

        var users = await query
            .OrderBy(x => x.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var response = _mapper.Map<IEnumerable<UserResponse>>(users);

        return (response, total);
    }

    public async Task<UserResponse?> GetByIdAsync(Guid id)
    {
        var entity = await _db.Users.FindAsync(id);
        return entity == null ? null : _mapper.Map<UserResponse>(entity);
    }

    public async Task<UserResponse> CreateAsync(UserRequest request)
    {
        var entity = _mapper.Map<User>(request);
        entity.Id = Guid.NewGuid();

        _db.Users.Add(entity);
        await _db.SaveChangesAsync();

        return _mapper.Map<UserResponse>(entity);
    }

    public async Task<bool> UpdateAsync(Guid id, UserRequest request)
    {
        var entity = await _db.Users.FindAsync(id);
        if (entity == null) return false;

        _mapper.Map(request, entity);
        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _db.Users.FindAsync(id);
        if (entity == null) return false;

        _db.Users.Remove(entity);
        await _db.SaveChangesAsync();

        return true;
    }
}
