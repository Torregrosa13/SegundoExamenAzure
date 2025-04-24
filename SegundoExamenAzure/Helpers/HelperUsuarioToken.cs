using System.Security.Claims;
using Newtonsoft.Json;
using SegundoExamenAzure.Models;

namespace SegundoExamenAzure.Helpers
{
    public class HelperUsuarioToken
    {
        private IHttpContextAccessor contextAccesor;

        public HelperUsuarioToken(IHttpContextAccessor contextAccesor)
        {
            this.contextAccesor = contextAccesor;
        }

        public UsuarioModel GetUsuario()
        {
            Claim claim = this.contextAccesor.HttpContext.User.FindFirst(x => x.Type == "UserData");
            string json = claim.Value;
            string jsonUsuario = HelperCryptography.DecryptString(json);
            UsuarioModel model = JsonConvert.DeserializeObject<UsuarioModel>(jsonUsuario);
            return model;
        }
    }
}
