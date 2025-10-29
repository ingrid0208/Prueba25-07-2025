using Business.Interfaces.IBusinessImplements;
using Entity.Domain.Enums;
using Entity.DTOs.Default;
using Entity.DTOs.Select;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.ControllersBase.Web.Controllers.BaseController;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    [Produces("application/json")]


    public class RolUserController : BaseController<RolUserSelectDto,RolUserDto, IRolUserService>
    {
        public RolUserController(IRolUserService service, ILogger<RolUserController> logger)
            : base(service, logger) { }

        protected override Task<IEnumerable<RolUserSelectDto>> GetAllAsync(GetAllType g) => _service.GetAllAsync(g);

        protected override Task<RolUserSelectDto?> GetByIdAsync(int id) => _service.GetByIdAsync(id);

        protected override Task AddAsync(RolUserDto dto) => _service.CreateAsync(dto);
        protected override Task<bool> UpdateAsync(int id, RolUserDto dto) => _service.UpdateAsync(dto);

        protected override async Task<bool> DeleteAsync(int id, DeleteType deleteType)
        {
            var user = await _service.GetByIdAsync(id);
            if (user is null) return false;

            await _service.DeleteAsync(id);
            return true;
        }

        protected override Task<bool> RestaureAsync(int id) => _service.RestoreLogical(id);

        
    }
}
