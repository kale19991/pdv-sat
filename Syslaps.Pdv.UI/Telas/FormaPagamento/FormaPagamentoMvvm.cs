using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Syslaps.Pdv.Core;

namespace Syslaps.Pdv.UI.Telas.FormaPagamento
{
    public class GrupoFormaPagamento
    {
        public string Categoria { get; set; }
        public List<Entity.TipoPagamento> Itens { get; set; }
    }

    public class FormaPagamentoMvvm : INotifyPropertyChanged
    {
        private readonly Core.Dominio.Venda.TipoPagamento _tipoPagamentoDominio;

        public Action FormasDePagamentoSalvasHandler;

        public FormaPagamentoMvvm()
        {
            _tipoPagamentoDominio = ContainerIoc.GetInstance<Core.Dominio.Venda.TipoPagamento>();

            var ordem = Core.Dominio.Venda.CategoriaFormaPagamento.Ordem.ToList();

            GruposDeFormasDePagamento = _tipoPagamentoDominio.ListaDeTiposDePagamento
                .GroupBy(x => x.Categoria)
                .OrderBy(grupo => ordem.IndexOf(grupo.Key))
                .Select(grupo => new GrupoFormaPagamento { Categoria = grupo.Key, Itens = grupo.ToList() })
                .ToList();
        }

        private List<GrupoFormaPagamento> _gruposDeFormasDePagamento;

        public List<GrupoFormaPagamento> GruposDeFormasDePagamento
        {
            get { return _gruposDeFormasDePagamento; }
            set { _gruposDeFormasDePagamento = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ICommand _salvarFormasDePagamentoCommand;
        public ICommand SalvarFormasDePagamentoCommand
        {
            get
            {
                return _salvarFormasDePagamentoCommand ?? (_salvarFormasDePagamentoCommand = new RelayCommandMvvm(param =>
                {
                    GruposDeFormasDePagamento
                        .SelectMany(grupo => grupo.Itens)
                        .ToList()
                        .ForEach(tipoPagamento => _tipoPagamentoDominio.AtualizarHabilitacao(tipoPagamento, tipoPagamento.Habilitada));

                    FormasDePagamentoSalvasHandler?.Invoke();
                }, null));
            }
        }
    }
}
