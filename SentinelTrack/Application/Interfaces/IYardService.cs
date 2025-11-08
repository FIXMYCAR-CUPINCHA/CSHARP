using SentinelTrack.Application.DTOs.Request;
using SentinelTrack.Application.DTOs.Response;

namespace SentinelTrack.Application.Interfaces;

public interface IYardService
{
    Task<IEnumerable<YardResponse>> GetAllAsync(int page, int pageSize);
    Task<YardResponse?> GetByIdAsync(Guid id);
    Task<YardResponse> CreateAsync(YardRequest request);
    Task<bool> UpdateAsync(Guid id, YardRequest request);
    Task<bool> DeleteAsync(Guid id);
}