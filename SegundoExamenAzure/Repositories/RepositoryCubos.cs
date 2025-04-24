using Microsoft.EntityFrameworkCore;
using SegundoExamenAzure.Data;
using SegundoExamenAzure.Models;

namespace SegundoExamenAzure.Repositories
{
    public class RepositoryCubos
    {
        private CubosContext context;
        
        public RepositoryCubos(CubosContext context)
        {
            this.context = context;
        }

        public async Task<List<Cubo>> GetCubosAsync()
        {
            return await this.context.Cubos.ToListAsync();
        }

        public async Task<int> GetMaxIdAsync()
        {
            return await this.context.Usuarios.Select(x => x.IdUsuario).MaxAsync() + 1;
        }

        public async Task CreateUsuarioAsync(string nombre, string email, string pass)
        {
            Usuario user = new Usuario
            {
                IdUsuario = await this.GetMaxIdAsync(),
                Nombre = nombre,
                Email = email,
                Pass = pass,
                Imagen = ""
            };
            await this.context.Usuarios.AddAsync(user);
            await this.context.SaveChangesAsync();
        }

        public async Task<Usuario> LoginUsuarioAsync(string email, string pass)
        {
            return await this.context.Usuarios.Where(x => x.Email == email && x.Pass == pass).FirstOrDefaultAsync();
        }

        public async Task<Usuario> PerfilUsuario(int id)
        {
            return await this.context.Usuarios.Where(x => x.IdUsuario == id).FirstOrDefaultAsync();
        }
    }
}
