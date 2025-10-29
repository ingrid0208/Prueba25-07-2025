using Data.Services.Auth;
using Entity.Domain.Models.Auth;
using Entity.Infrastructure.Contexts;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace test.Data.Auth
{
    public class RefreshTokenRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly RefreshTokenRepository _repository;

        public RefreshTokenRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new RefreshTokenRepository(_context);

            _context.RefreshTokens.AddRange(
                new RefreshToken
                {
                    Id = 1,
                    UserId = 10,
                    TokenHash = "hash_valid_1",
                    ExpiresAt = DateTime.UtcNow.AddHours(1),
                    IsRevoked = false
                },
                new RefreshToken
                {
                    Id = 2,
                    UserId = 10,
                    TokenHash = "hash_expired",
                    ExpiresAt = DateTime.UtcNow.AddHours(-1),
                    IsRevoked = false
                },
                new RefreshToken
                {
                    Id = 3,
                    UserId = 10,
                    TokenHash = "hash_revoked",
                    ExpiresAt = DateTime.UtcNow.AddHours(2),
                    IsRevoked = true
                }
            );
            _context.SaveChanges();
        }

        [Fact]
        public async Task AddAsyncShouldInsertNewToken()
        {
            var newToken = new RefreshToken
            {
                Id = 4,
                UserId = 20,
                TokenHash = "hash_new",
                ExpiresAt = DateTime.UtcNow.AddHours(2),
                IsRevoked = false
            };

            await _repository.AddAsync(newToken);

            var stored = await _context.RefreshTokens.FindAsync(4);
            stored.Should().NotBeNull();
            stored!.TokenHash.Should().Be("hash_new");
        }

        [Fact]
        public async Task GetByHashAsyncShouldReturnTokenWhenExists()
        {
            var result = await _repository.GetByHashAsync("hash_valid_1");

            result.Should().NotBeNull();
            result!.UserId.Should().Be(10);
            result.IsRevoked.Should().BeFalse();
        }

        [Fact]
        public async Task GetByHashAsyncShouldReturnNullWhenNotExists()
        {
            var result = await _repository.GetByHashAsync("not_found");
            result.Should().BeNull();
        }

        [Fact]
        public async Task RevokeAsyncShouldSetIsRevokedTrueAndSetReplacedByHash()
        {
            var token = await _context.RefreshTokens.FindAsync(1);
            token.Should().NotBeNull();

            await _repository.RevokeAsync(token!, "hash_replacement");

            var updated = await _context.RefreshTokens.FindAsync(1);
            updated!.IsRevoked.Should().BeTrue();
            updated.ReplacedByTokenHash.Should().Be("hash_replacement");
        }

        [Fact]
        public async Task RevokeAsyncShouldDoNothingWhenTokenNotExists()
        {
            var fakeToken = new RefreshToken { Id = 999 };

            await _repository.RevokeAsync(fakeToken);

            var countRevoked = _context.RefreshTokens.Count(t => t.IsRevoked);
            countRevoked.Should().Be(1); // Solo el del test anterior
        }

        [Fact]
        public async Task GetValidTokensByUserAsyncShouldReturnOnlyNonRevokedAndNonExpiredTokens()
        {
            var result = await _repository.GetValidTokensByUserAsync(10);

            result.Should().HaveCount(1);
            result.First().TokenHash.Should().Be("hash_valid_1");
        }

        [Fact]
        public async Task GetValidTokensByUserAsyncShouldReturnEmptyWhenNoValidTokens()
        {
            var result = await _repository.GetValidTokensByUserAsync(99);

            result.Should().BeEmpty();
        }
    }
}
