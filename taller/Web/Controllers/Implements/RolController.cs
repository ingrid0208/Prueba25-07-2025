using Business.Interfaces.BusinessBasic;
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

    public class RolController : BaseController<RolSelectDto,RolDto, IRolService>
    {
        public RolController(IRolService service, ILogger logger) : base(service, logger)
        {
        }

        protected override async Task<IEnumerable<RolSelectDto>> GetAllAsync(GetAllType g)
        {
            //=> _service.GetAllAsync();
            var rol = await _service.GetAllAsync(g);
            if (rol is null) return null;

  
            return rol;

        }


        protected override Task<RolSelectDto?> GetByIdAsync(int id) =>_service.GetByIdAsync(id);

        protected override Task AddAsync(RolDto dto) => _service.CreateAsync(dto);
        protected override Task<bool> UpdateAsync(int id, RolDto dto) => _service.UpdateAsync(dto);

        protected override async Task<bool> DeleteAsync(int id, DeleteType deleteType)
        {
            var rol = await _service.GetByIdAsync(id);
            if (rol is null) return false;

            await _service.DeleteAsync(id, deleteType);
            return true;
        }

        protected override Task<bool> RestaureAsync(int id)=> _service.RestoreLogical(id);




        //public Task<IActionResult> RestaureAsync(int id)=> _service.RestoreLogical(id);
    }
}
