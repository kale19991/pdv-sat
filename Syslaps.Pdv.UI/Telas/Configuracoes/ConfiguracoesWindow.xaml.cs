using System.Windows;

namespace Syslaps.Pdv.UI.Telas.Configuracoes
{
    public partial class ConfiguracoesWindow : Window
    {
        public ConfiguracoesWindow()
        {
            InitializeComponent();
        }

        private void BtnEditarEmpresa_OnClick(object sender, RoutedEventArgs e)
        {
            (new Empresa.EditarEmpresaWindow()).ShowDialog();
        }

        private void BtnFormasDePagamento_OnClick(object sender, RoutedEventArgs e)
        {
            (new FormaPagamento.FormaPagamentoWindow()).ShowDialog();
        }
    }
}
