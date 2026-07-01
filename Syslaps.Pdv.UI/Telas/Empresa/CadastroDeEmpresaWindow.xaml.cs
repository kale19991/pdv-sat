using System.Windows;

namespace Syslaps.Pdv.UI.Telas.Empresa
{
    public partial class CadastroDeEmpresaWindow : Window
    {
        private readonly CadastroDeEmpresaMvvm _mvvm;

        public CadastroDeEmpresaWindow()
        {
            InitializeComponent();
            _mvvm = new CadastroDeEmpresaMvvm();
            _mvvm.EmpresaRegistradaHandler += EmpresaRegistradaHandler;
            _mvvm.ErroAoRegistrarHandler += ErroAoRegistrarHandler;
            DataContext = _mvvm;
        }

        private void EmpresaRegistradaHandler()
        {
            MessageBox.Show("Empresa cadastrada com sucesso.", InstanceManager.Parametros.TituloDasMensagens, MessageBoxButton.OK, MessageBoxImage.Information);
            TxtRazaoSocial.Focus();
        }

        private void ErroAoRegistrarHandler(string mensagem)
        {
            MessageBox.Show(mensagem, InstanceManager.Parametros.TituloDasMensagens, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void CadastroDeEmpresaWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            TxtRazaoSocial.Focus();
        }
    }
}
