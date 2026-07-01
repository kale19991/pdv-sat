using System.Collections.Generic;
using System.Linq;
using Dapper;
using Syslaps.Pdv.Core.Dominio.Usuario;
using Empresa = Syslaps.Pdv.Entity.Empresa;

namespace Syslaps.Pdv.Infra.Repositorio
{
    public sealed class RepositorioUsuarioEmpresa : RepositorioBase, IUsuarioEmpresaRepositorio
    {
        public List<Empresa> RecuperarEmpresasDoUsuario(string codigoUsuario)
        {
            return Db.Query<Empresa>(
                @"select e.* from Empresa e
                  inner join UsuarioEmpresa ue on ue.Empresa_CodigoEmpresa = e.CodigoEmpresa
                  where ue.Usuario_CodigoUsuario = @Usuario_CodigoUsuario",
                new { Usuario_CodigoUsuario = codigoUsuario }).ToList();
        }
    }
}
