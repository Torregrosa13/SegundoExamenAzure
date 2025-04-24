using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SegundoExamenAzure.Helpers;
using SegundoExamenAzure.Models;
using SegundoExamenAzure.Repositories;

namespace SegundoExamenAzure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private RepositoryCubos repo;
        private HelperActionServicesOAuth helper;
        public AuthController(RepositoryCubos repo,
            HelperActionServicesOAuth helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            Usuario usuario = await this.repo.LoginUsuarioAsync(model.Email, model.Pass);

            if (usuario == null)
            {
                return Unauthorized();
            }

            SigningCredentials credentials = new SigningCredentials(this.helper.GetKeyToken(), SecurityAlgorithms.HmacSha256);
            string jsonUsuario = JsonConvert.SerializeObject(usuario);

            Claim[] info = new[]
            {
         new Claim(ClaimTypes.Name, usuario.Nombre),
         new Claim("IdUsuario", usuario.IdUsuario.ToString()),
         new Claim("Email", usuario.Email)
     };

            JwtSecurityToken token = new JwtSecurityToken(
                claims: info,
                issuer: this.helper.Issuer,
                audience: this.helper.Audience,
                signingCredentials: credentials,
                expires: DateTime.UtcNow.AddMinutes(30),
                notBefore: DateTime.UtcNow
            );

            return Ok(new
            {
                response = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }
}
