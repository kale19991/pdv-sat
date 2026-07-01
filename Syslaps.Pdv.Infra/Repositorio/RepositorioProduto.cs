using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Syslaps.Pdv.Core;
using Syslaps.Pdv.Core.Dominio.Produto;
using Produto = Syslaps.Pdv.Entity.Produto;

namespace Syslaps.Pdv.Infra.Repositorio
{
    public class RepositorioProduto : RepositorioBase, IProdutoRepositorio
    {
        public List<Produto> RecuperarListaDeProdutosDoPdv()
        {
            return Db.Query<Produto>(
                "select * from produto where Ativo = @Ativo and ExibirNoPdv = @ExibirNoPdv and Empresa_CodigoEmpresa = @CodigoEmpresa Order By Descricao",
                new { Ativo = true, ExibirNoPdv = true, CodigoEmpresa = ContextoAtual.CodigoEmpresaAtual })
                .ToList();
        }

        public List<Produto> RecuperarListaDeProdutosComProducao()
        {
            return Db.Query<Produto>(
                "select * from produto where Ativo = @Ativo and TemProducao = @TemProducao and Empresa_CodigoEmpresa = @CodigoEmpresa Order By Descricao",
                new { Ativo = true, TemProducao = true, CodigoEmpresa = ContextoAtual.CodigoEmpresaAtual })
                .ToList();
        }

        public Entity.Produto RecuperarProdutoPorCodigoDeBarras(string codigoDeBarra)
        {
            return Db.QueryFirstOrDefault<Entity.Produto>(
                "select * from produto where Ativo = @Ativo and CodigoDeBarra = @CodigoDeBarra and Empresa_CodigoEmpresa = @CodigoEmpresa",
                new { Ativo = true, TemProducao = true, CodigoDeBarra = codigoDeBarra, CodigoEmpresa = ContextoAtual.CodigoEmpresaAtual });
        }
    }
}