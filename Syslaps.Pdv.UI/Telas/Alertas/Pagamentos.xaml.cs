using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Syslaps.Pdv.Cross;
using Syslaps.Pdv.Entity;
using Syslaps.Pdv.UI.Telas.PDV;

namespace Syslaps.Pdv.UI.Telas.Alertas
{
    public partial class Pagamentos : Window
    {
        private readonly PontoDeVendaMvvm _mvvm;

        public Pagamentos(PontoDeVendaMvvm mvvm)
        {
            _mvvm = mvvm;
            DataContext = mvvm;
            InitializeComponent();
        }

        private void BtnOk_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _mvvm.RegistrarVenda();
                DialogResult = true;
            } catch (Exception ex)
            {
                MessageBox.Show(string.Concat("Falha ao Gravar a Venda. Entre em contato com o Administrador.\nErro: ", ex.Message), InstanceManager.Parametros.TituloDasMensagens, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Pagamentos_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (TxtValor != null)
            {
                TxtValor.Valor = _mvvm.ValorTotalDaVenda;
                TxtValor.Focus();
            }
        }

        private void AdicionarPagamento_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var tipoDePagamento = (TipoPagamento)CmbTipoDePagameto.SelectedItem;
                if (tipoDePagamento.CodigoTipoPagamento == "0")
                {
                    MessageBox.Show("Selecione um tipo de pagamento.", InstanceManager.Parametros.TituloDasMensagens, MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                    return;
                }

                var valor = TxtValor.Valor;
                _mvvm.AdicionarVendaPagamento(tipoDePagamento, valor);

                if (_mvvm.ValorTotalDaVenda <= _mvvm.ValorTotalDePagamento)
                    BtnOk.Focus();
                else
                {
                    TxtValor.Valor = _mvvm.ValorTotalDaVenda - _mvvm.ValorTotalDePagamento;
                    TxtValor.Focus();
                }
            }catch(Exception ex)
            {
                MessageBox.Show(string.Concat("Falha ao Adicionar Pagamento. Entre em contato com o Administrador.\nErro: ", ex.Message), InstanceManager.Parametros.TituloDasMensagens, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CmbTipoDePagameto_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TxtValor != null)
            {
                TxtValor.Valor = _mvvm.ValorTotalDaVenda - _mvvm.ValorTotalDePagamento;
                TxtValor?.Focus();
            }
        }

        private void RemoverPagamento_OnClick(object sender, RoutedEventArgs e)
        {
            var vendaPagamento = (VendaPagamento)(sender as Button).DataContext;
            _mvvm.RemoverVendaPagamento(vendaPagamento);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F1:
                    {
                        CmbTipoDePagameto.SelectedIndex = 0;
                        break;
                    }
                case Key.F2:
                    {
                        CmbTipoDePagameto.SelectedIndex = 1;
                        break;
                    }
                case Key.F3:
                    {
                        CmbTipoDePagameto.SelectedIndex = 2;
                        break;
                    }
                case Key.F4:
                    {
                        CmbTipoDePagameto.SelectedIndex = 3;
                        break;
                    }
                case Key.F5:
                    {
                        CmbTipoDePagameto.SelectedIndex = 4;
                        break;
                    }
                case Key.F6:
                    {
                        CmbTipoDePagameto.SelectedIndex = 5;
                        break;
                    }
                case Key.F10:
                    {
                        AdicionarPagamento_OnClick(null, null);
                        break;
                    }

            }
        }
    }
}
