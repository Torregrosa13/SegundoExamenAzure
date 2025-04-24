using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SegundoExamenAzure.Helpers;
using SegundoExamenAzure.Models;
using SegundoExamenAzure.Repositories;

namespace SegundoExamenAzure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CubosController : ControllerBase
    {
        private RepositoryCubos repo;
        private HelperUsuarioToken helper;
        public CubosController(RepositoryCubos repo, HelperUsuarioToken helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<Cubo>>> GetCubosAsync()
        {
            return await this.repo.GetCubosAsync();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> CreateUsuario(Usuario user)
        {
            await this.repo.CreateUsuarioAsync(user.Nombre, user.Email, user.Pass);
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<UsuarioModel>> PerfilUsuario()
        {

            UsuarioModel model = this.helper.GetUsuario();
            return model;
        }
    }
}
