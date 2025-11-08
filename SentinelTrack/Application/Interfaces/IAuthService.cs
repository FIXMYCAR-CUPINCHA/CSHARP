using SentinelTrack.Application.DTOs.Request;
using SentinelTrack.Application.DTOs.Response;

namespace SentinelTrack.Application.Interfaces
{
    public interface IAuthService
    {
        AuthResponse? Authenticate(AuthRequest request);
    }
}