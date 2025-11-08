namespace SentinelTrack.Infrastructure.Security;

public class JwtSettings
{
    public string Key { get; set; } = "ReplaceMeSecretKeyNeedsToBe32Chars";
    public string Issuer { get; set; } = "SentinelTrack";
    public string Audience { get; set; } = "SentinelTrackUsers";
    public int ExpiresMinutes { get; set; } = 60;
}