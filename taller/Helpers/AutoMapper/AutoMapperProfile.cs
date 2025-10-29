using AutoMapper;
using Entity.Domain.Models;
using Entity.Domain.Models.Implements;
using Entity.DTOs.Auth;
using Entity.DTOs.Default;
using Entity.DTOs.Select;

namespace Helpers.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Rol, RolSelectDto>().ReverseMap();
            CreateMap<Rol, RolDto>().ReverseMap();

            CreateMap<User, UserSelectDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();


            CreateMap<RolUser, RolUserDto>().ReverseMap();
            CreateMap<RolUser, RolUserSelectDto>().ReverseMap();

            CreateMap<Form, FormSelectDto>().ReverseMap();
            CreateMap<Form, FormDto>().ReverseMap();


            CreateMap<Module, ModuleSelectDto>().ReverseMap();
            CreateMap<Module, ModuleDto>().ReverseMap();


            CreateMap<Permission, PermissionSelectDto>().ReverseMap();
            CreateMap<Permission, PermissionDto>().ReverseMap();


            CreateMap<Person, PersonSelectDto>().ReverseMap();
            CreateMap<Person, PersonDto>().ReverseMap();

            CreateMap<FormModule, FormModuleSelectDto>().ReverseMap();
            CreateMap<FormModule, FormModuleDto>().ReverseMap();

            CreateMap<RolFormPermission, RolFormPermissionDto>().ReverseMap();
            CreateMap<RolFormPermission, RolFormPermissionSelectDto>().ReverseMap();

            CreateMap<FormModule, FormModuleDto>().ReverseMap();
            CreateMap<FormModule, FormModuleSelectDto>().ReverseMap();

            CreateMap<RegisterUserDto, User>();
            CreateMap<RegisterUserDto, Person>();



            //CreateMap<TouristicAttraction, TouristicAttractionApiDto>().ReverseMap();
        }

    }
}
