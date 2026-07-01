using Syslaps.Pdv.Cross;
using System.Windows;

namespace Syslaps.Pdv.UI.Telas.Alertas
{
    public partial class DadosConsumidor : Window
    {
        private PDV.PontoDeVendaMvvm _mvvm;

        public DadosConsumidor(PDV.PontoDeVendaMvvm mvvm = null)
        {
            _mvvm = mvvm;
            InitializeComponent();
            this.Title = InstanceManager.Parametros.TituloNoConfig;

            if (!_mvvm.VendaCorrente.VendaCorrente.CpfCnpjCliente.IsNullOrEmpty())
                TxtCpf.Documento = _mvvm.VendaCorrente.VendaCorrente.CpfCnpjCliente;

            if (!_mvvm.VendaCorrente.VendaCorrente.NomeCliente.IsNullOrEmpty())
                TxtNome.Text = _mvvm.VendaCorrente.VendaCorrente.NomeCliente;
        }

        private void BtnOk_OnClick(object sender, RoutedEventArgs e)
        {
            if (!TxtCpf.Documento.IsCpfOrCnpj())
            {
                MessageBox.Show("CPF ou CNPJ inválido.", InstanceManager.Parametros.TituloDasMensagens, MessageBoxButton.OK, MessageBoxImage.Error);
                TxtCpf.Focus();
                return;
            }

            if (_mvvm != null)
            {
                _mvvm.VendaCorrente.VendaCorrente.CpfCnpjCliente = !TxtCpf.Text.IsNullOrEmpty() ? TxtCpf.Text : "";
                _mvvm.VendaCorrente.VendaCorrente.NomeCliente = !TxtNome.Text.IsNullOrEmpty() ? TxtNome.Text : "";
                _mvvm.VendaCorrente.VendaCorrente.TipoDocumento = TxtCpf.Documento.Length > 11 ? "CNPJ" : "CPF";
            }

            this.DialogResult = true;
        }

        private void IniciarNovaVenda_OnLoaded(object sender, RoutedEventArgs e)
        {
            TxtCpf.Focus();
        }
    }
}
