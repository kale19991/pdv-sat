using Syslaps.Pdv.Core.Dominio.Base;

namespace Syslaps.Pdv.Core.Dominio.Usuario
{
    public interface IUsuarioRepositorio : IRepositorioBase
    {
        Entity.Usuario RecuperarUsuarioPorNome(string nome);
        bool ExisteAlgumUsuario();
    }
}