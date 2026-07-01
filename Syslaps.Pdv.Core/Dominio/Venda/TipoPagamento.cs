using System.Collections.Generic;
using System.Linq;
using Syslaps.Pdv.Core.Dominio.Base;

namespace Syslaps.Pdv.Core.Dominio.Venda
{
    public class TipoPagamento : ModeloBase
    {
        private readonly IRepositorioBase repositorio;

        private List<Entity.TipoPagamento> listaDeTiposDePagamento;

        public List<Entity.TipoPagamento> ListaDeTiposDePagamento
        {
            get
            {
                if (listaDeTiposDePagamento == null)
                    listaDeTiposDePagamento = repositorio.RecuperarTodos<Entity.TipoPagamento>();

                return listaDeTiposDePagamento;
            }
        }

        public Entity.TipoPagamento Dinheiro
        {
            get { return ListaDeTiposDePagamento.SingleOrDefault(x => x.Nome == "Dinheiro"); }
        }

        public Entity.TipoPagamento Pix
        {
            get { return ListaDeTiposDePagamento.SingleOrDefault(x => x.Nome == "Pix" && x.Categoria == CategoriaFormaPagamento.PagamentoNaEntrega); }
        }

        public TipoPagamento(IRepositorioBase repositorio)
        {
            this.repositorio = repositorio;
        }

        public void AtualizarHabilitacao(Entity.TipoPagamento tipoPagamento, bool habilitada)
        {
            tipoPagamento.Habilitada = habilitada;
            repositorio.Atualizar(tipoPagamento);
        }
    }
}
