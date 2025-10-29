using Entity.Domain.Models.Auth;

namespace Data.Interfaces.IDataImplement.Auth
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken token);
        Task<RefreshToken?> GetByHashAsync(string tokenHash);
        Task RevokeAsync(RefreshToken token, string? replacedByTokenHash = null);
        Task<IEnumerable<RefreshToken>> GetValidTokensByUserAsync(int userId);
    }
}
