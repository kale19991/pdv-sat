using System.Windows;
using Bolaria.WpfUI.UI;
using Syslaps.Pdv.Cross;
using Caixa = Syslaps.Pdv.Core.Dominio.Caixa.Caixa;
using Produto = Syslaps.Pdv.Core.Dominio.Produto.Produto;
using TelaPrincipal = Syslaps.Pdv.UI.Telas.TelaPrincipal;

namespace Syslaps.Pdv.UI.Telas.ConfiguracaoInicial
{
    public partial class ConfiguracaoInicialWindow : Window
    {
        private readonly ConfiguracaoInicialMvvm _mvvm;

        public ConfiguracaoInicialWindow()
        {
            InitializeComponent();
            _mvvm = new ConfiguracaoInicialMvvm();
            _mvvm.ConfiguracaoConcluidaHandler += ConfiguracaoConcluidaHandler;
            _mvvm.ErroAoConcluirHandler += ErroAoConcluirHandler;
            DataContext = _mvvm;
        }

        private string TituloDasMensagens => InstanceManager.Parametros?.TituloDasMensagens ?? "Pdv - SAT";

        private void ErroAoConcluirHandler(string mensagem)
        {
            MessageBox.Show(mensagem, TituloDasMensagens, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void ConfiguracaoConcluidaHandler()
        {
            ContainerIoc.containerIoc.Configure(ioc =>
            {
                ioc.ForConcreteType<Caixa>().Configure.Ctor<string>("nomeDoCaixa").Is("NomeDoCaixa".GetConfigValue()).Ctor<Entity.Usuario>().Is(_mvvm.UsuarioDominio.UsuarioLogado);
            });

            InstanceManager.UsuarioCorrente = _mvvm.UsuarioDominio;
            InstanceManager.EmpresaCorrente = _mvvm.EmpresaDominio;
            InstanceManager.ListaDeProdutosDoPdv = ContainerIoc.GetInstance<Produto>().RecuperarListaDeProdutosDoPdvPoTipo(1);

            UIManager<TelaPrincipal>.Show();
            Close();
        }

        private void BtnConcluir_OnClick(object sender, RoutedEventArgs e)
        {
            _mvvm.Senha = TxtSenha.Password;
        }

        private void BtnSair_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
            Application.Current.Shutdown();
        }

        private void ConfiguracaoInicialWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            TxtRazaoSocial.Focus();
        }
    }
}
