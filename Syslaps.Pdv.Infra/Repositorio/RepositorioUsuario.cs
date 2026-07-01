using Dapper;
using Syslaps.Pdv.Core.Dominio.Usuario;
using Usuario = Syslaps.Pdv.Entity.Usuario;

namespace Syslaps.Pdv.Infra.Repositorio
{
    public sealed class RepositorioUsuario : RepositorioBase, IUsuarioRepositorio
    {

        public Usuario RecuperarUsuarioPorNome(string nome)
        {
            return Db.QuerySingleOrDefault<Usuario>("select * from Usuario Where Nome = @Nome", new { Nome = nome });
        }

        
    }
}
