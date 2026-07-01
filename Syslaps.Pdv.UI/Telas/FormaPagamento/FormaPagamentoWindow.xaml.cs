using System.Windows;

namespace Syslaps.Pdv.UI.Telas.FormaPagamento
{
    public partial class FormaPagamentoWindow : Window
    {
        private readonly FormaPagamentoMvvm _mvvm;

        public FormaPagamentoWindow()
        {
            InitializeComponent();
            _mvvm = new FormaPagamentoMvvm();
            _mvvm.FormasDePagamentoSalvasHandler += FormasDePagamentoSalvasHandler;
            DataContext = _mvvm;
        }

        private void FormasDePagamentoSalvasHandler()
        {
            MessageBox.Show("Formas de pagamento atualizadas com sucesso.", InstanceManager.Parametros.TituloDasMensagens, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
