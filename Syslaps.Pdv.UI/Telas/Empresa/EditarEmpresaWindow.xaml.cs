using System.Windows;

namespace Syslaps.Pdv.UI.Telas.Empresa
{
    public partial class EditarEmpresaWindow : Window
    {
        private readonly EditarEmpresaMvvm _mvvm;

        public EditarEmpresaWindow()
        {
            InitializeComponent();
            _mvvm = new EditarEmpresaMvvm();
            _mvvm.EmpresaAtualizadaHandler += EmpresaAtualizadaHandler;
            _mvvm.ErroAoAtualizarHandler += ErroAoAtualizarHandler;
            DataContext = _mvvm;
        }

        private void EmpresaAtualizadaHandler()
        {
            MessageBox.Show("Empresa atualizada com sucesso.", InstanceManager.Parametros.TituloDasMensagens, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ErroAoAtualizarHandler(string mensagem)
        {
            MessageBox.Show(mensagem, InstanceManager.Parametros.TituloDasMensagens, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void EditarEmpresaWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            TxtRazaoSocial.Focus();
        }
    }
}
