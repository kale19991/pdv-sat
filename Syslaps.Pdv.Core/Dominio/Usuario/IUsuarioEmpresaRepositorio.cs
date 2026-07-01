using System.Collections.Generic;
using Syslaps.Pdv.Core.Dominio.Base;

namespace Syslaps.Pdv.Core.Dominio.Usuario
{
    public interface IUsuarioEmpresaRepositorio : IRepositorioBase
    {
        List<Entity.Empresa> RecuperarEmpresasDoUsuario(string codigoUsuario);
    }
}
