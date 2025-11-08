using SentinelTrack.Application.DTOs.Response;
using SentinelTrack.Application.DTOs.Request;

namespace SentinelTrack.Application.Interfaces
{
    public interface IUserService
    {
        Task<(IEnumerable<UserResponse> Items, int Total)> GetAllAsync(int page, int pageSize, string? email);
        Task<UserResponse?> GetByIdAsync(Guid id);
        Task<UserResponse> CreateAsync(UserRequest request);
        Task<bool> UpdateAsync(Guid id, UserRequest request);
        Task<bool> DeleteAsync(Guid id);
    }
}