using Data.Services;
using Entity.Domain.Models.Implements;
using Entity.DTOs.Auth;
using Entity.Infrastructure.Contexts;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace test.Data
{
    public class UserRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly UserRepository _repository;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new UserRepository(_context);

            // ================
            // Datos base
            // ================
            var person1 = new Person { Id = 1, Identification = "1001", FirstName = "Juan", LastName = "Pepito", PhoneNumber = "123", Address ="Calle"};
            var person2 = new Person { Id = 2, Identification = "1002", FirstName = "Ana", LastName ="Pepita", PhoneNumber ="321", Address = "Carrera"};

            var user1 = new User
            {
                Id = 1,
                Email = "juan@mail.com",
                Password = "12345",
                IsDeleted = false,
                PersonId = 1,
                Person = person1
            };

            var user2 = new User
            {
                Id = 2,
                Email = "ana@mail.com",
                Password = "54321",
                IsDeleted = true,
                PersonId = 2,
                Person = person2
            };

            _context.Persons.AddRange(person1, person2);
            _context.Users.AddRange(user1, user2);
            _context.SaveChanges();
        }

        // ========================================================
        // TEST 1: FindEmail
        // ========================================================
        [Fact]
        public async Task FindEmail_ShouldReturnUser_WhenExists()
        {
            var result = await _repository.FindEmail("juan@mail.com");

            result.Should().NotBeNull();
            result!.Email.Should().Be("juan@mail.com");
        }

        [Fact]
        public async Task FindEmail_ShouldReturnNull_WhenNotExists()
        {
            var result = await _repository.FindEmail("noexist@mail.com");
            result.Should().BeNull();
        }

        // ========================================================
        // TEST 2: ExistsByEmailAsync
        // ========================================================
        [Fact]
        public async Task ExistsByEmailAsync_ShouldReturnTrue_ForActiveUser()
        {
            var result = await _repository.ExistsByEmailAsync("juan@mail.com");
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ExistsByEmailAsync_ShouldReturnFalse_ForDeletedUser()
        {
            var result = await _repository.ExistsByEmailAsync("ana@mail.com");
            result.Should().BeFalse();
        }

        // ========================================================
        // TEST 3: ExistsByDocumentAsync
        // ========================================================
        [Fact]
        public async Task ExistsByDocumentAsync_ShouldReturnTrue_WhenPersonIdentificationMatches()
        {
            var result = await _repository.ExistsByDocumentAsync("1001");
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ExistsByDocumentAsync_ShouldReturnFalse_WhenNoPersonWithIdentification()
        {
            var result = await _repository.ExistsByDocumentAsync("9999");
            result.Should().BeFalse();
        }

        // ========================================================
        // TEST 4: LoginUser
        // ========================================================
        [Fact]
        public async Task LoginUser_ShouldReturnUser_WhenCredentialsAreValid()
        {
            var loginDto = new LoginUserDto
            {
                Email = "juan@mail.com",
                Password = "12345"
            };

            var result = await _repository.LoginUser(loginDto);

            result.Should().NotBeNull();
            result!.Email.Should().Be(loginDto.Email);
        }

        [Fact]
        public async Task LoginUser_ShouldThrow_WhenCredentialsInvalid()
        {
            var loginDto = new LoginUserDto
            {
                Email = "juan@mail.com",
                Password = "wrongpass"
            };

            Func<Task> act = async () => await _repository.LoginUser(loginDto);

            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Credenciales inválidas");
        }
    }
}
