using AutoMapper;
using Business.Interfaces.IBusinessImplements.Auth;
using Data.Interfaces.IDataImplement;
using Entity.Domain.Models.Implements;
using Entity.DTOs.Auth;
using Entity.DTOs.Default;
using Helpers.Business.Business.Helpers.Validation;
using Microsoft.Extensions.Logging;
using Utilities.Custom;
using Utilities.Exceptions;

namespace Business.Services.Auth
{
    public class AuthService : IAuthService
    {

        private readonly IUserRepository _userData;
        private readonly IRolUserRepository _rolUserData;
        private readonly ILogger<AuthService> _logger;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userData, ILogger<AuthService> logger, IRolUserRepository rolUserData, IMapper mapper)
        {
            _logger = logger;
            _userData = userData;
            _rolUserData = rolUserData;
            _mapper = mapper;
        }
        public async Task<UserDto> RegisterAsync(RegisterUserDto dto)
        {
            try
            {
                if (dto.Password != dto.ConfirmPassword)
                    throw new ValidationException("Las contraseñas no coinciden");
                if (await _userData.ExistsByEmailAsync(dto.Email))
                    throw new Exception("Correo ya registrado");

                if (await _userData.ExistsByDocumentAsync(dto.Identification))
                    throw new Exception("Ya existe una persona con este numero de identificacion");

                var validPassword = BusinessValidationHelper.IsValidPassword(dto.Password);
                if (!validPassword)
                {
                    throw new BusinessException("Contraseña no valida");
                }

                var person = _mapper.Map<Person>(dto);
                var user = _mapper.Map<User>(dto);

                user.Password = EncriptePassword.EncripteSHA256(user.Password!);

                user.Person = person;
                user.Active = true;

                await _userData.CreateAsync(user);

                await _rolUserData.AsignateUserRol(user.Id);

                // Recuperar el usuario con sus relaciones para el mapeo correcto
                var createduser = await _userData.GetByIdAsync(user.Id);
                if (createduser == null)
                    throw new BusinessException("Error interno: el usuario no pudo ser recuperado tras la creación.");
                await _rolUserData.AsignateUserRol(createduser.Id);
                return _mapper.Map<UserDto>(createduser);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error en el registro del usuario: {ex.Message}", ex);
            }
        }
    }
}
