using SentinelTrack.Application.DTOs.Request;
using SentinelTrack.Application.DTOs.Response;

namespace SentinelTrack.Application.Interfaces;

public interface IMotoService
{
    Task<object> GetAllAsync(int page, int pageSize, string? plate, Guid? yardId);
    Task<MotoResponse?> GetByIdAsync(Guid id);
    Task<MotoResponse?> CreateAsync(MotoRequest request);
    Task<string?> UpdateAsync(Guid id, MotoRequest request); // retorna "not_found", "invalid_yard" ou null
    Task<bool> DeleteAsync(Guid id);
}