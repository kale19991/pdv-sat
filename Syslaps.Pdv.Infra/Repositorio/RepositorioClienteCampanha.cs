using System.Linq;
using Dapper;
using Syslaps.Pdv.Core;
using Syslaps.Pdv.Core.Dominio.Cliente;
using Syslaps.Pdv.Entity;

namespace Syslaps.Pdv.Infra.Repositorio
{
    public class RepositorioClienteCampanha : RepositorioBase, IClienteCampanhaRepositorio
    {
        public Entity.ClienteCampanha RecuperarcClienteNaCampanha(Entity.ClienteCampanha clienteCampanha)
        {
            return Db.Query<Entity.ClienteCampanha>("select * from ClienteCampanha Where CpfCnpj = @CpfCnpj and NomeCampanha = @NomeCampanha and Empresa_CodigoEmpresa = @Empresa_CodigoEmpresa",
                new Entity.ClienteCampanha { CpfCnpj = clienteCampanha.CpfCnpj, NomeCampanha = clienteCampanha.NomeCampanha, Empresa_CodigoEmpresa = ContextoAtual.CodigoEmpresaAtual }).FirstOrDefault();
        }
    }
}