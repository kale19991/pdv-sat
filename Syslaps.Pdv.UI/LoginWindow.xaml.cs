using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Bolaria.WpfUI.UI;
using Syslaps.Pdv.Core;
using Syslaps.Pdv.Core.Dominio;
using Syslaps.Pdv.Cross;
using Syslaps.Pdv.Entity;
using Syslaps.Pdv.Infra.Repositorio;
using Caixa = Syslaps.Pdv.Core.Dominio.Caixa.Caixa;
using Empresa = Syslaps.Pdv.Core.Dominio.Empresa.Empresa;
using Produto = Syslaps.Pdv.Core.Dominio.Produto.Produto;
using TelaPrincipal = Syslaps.Pdv.UI.Telas.TelaPrincipal;
using Usuario = Syslaps.Pdv.Core.Dominio.Usuario.Usuario;
using Syslaps.Pdv.Core.Dominio.Usuario;
using System.Configuration;

namespace Syslaps.Pdv.UI
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            this.Title = ConfigurationManager.AppSettings["TituloInicial"];
            lblTitle.Content = ConfigurationManager.AppSettings["TituloInicial"];

            txtUsuario.Focus();


#if DEBUG
            txtUsuario.Text = "admin";
            txtSenha.Password = "123";
            //btnEntrar_Click(null, null);
#endif
        }

        private void btnSair_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Application.Current.Shutdown();
        }

        private async void btnEntrar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.StartWait();
                if (txtUsuario.Text.Length == 0 || txtSenha.Password.Length == 0) return;
                var login = txtUsuario.Text;
                var senha = txtSenha.Password;
                var task = Task.Factory.StartNew(() =>
                {
                    InstanceManager.UsuarioCorrente = ContainerIoc.GetInstance<Usuario>();
                    InstanceManager.UsuarioCorrente.LogarUsuario(login, senha);
                });

                await task;

                if (InstanceManager.UsuarioCorrente.Status == EnumStatusDoResultado.MensagemDeSucesso)
                {
                    ContainerIoc.containerIoc.Configure(ioc =>
                    {
                        ioc.ForConcreteType<Caixa>().Configure.Ctor<string>("nomeDoCaixa").Is("NomeDoCaixa".GetConfigValue()).Ctor<Entity.Usuario>().Is(InstanceManager.UsuarioCorrente.UsuarioLogado);
                    });

                    InstanceManager.ListaDeProdutosDoPdv = ContainerIoc.GetInstance<Produto>().RecuperarListaDeProdutosDoPdvPoTipo(RdoTp2.IsChecked.Value ? 2 : 1);

                    DefinirEmpresaCorrenteDoUsuario();

                    UIManager<TelaPrincipal>.Show();
                    Close();
                }
                else
                {
                    var titulo = InstanceManager.Parametros?.TituloDasMensagens ?? ConfigurationManager.AppSettings["TituloInicial"];
                    MessageBox.Show(InstanceManager.UsuarioCorrente.Mensagem, titulo, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                InstanceManager.Logger.Error(ex);
                var titulo = InstanceManager.Parametros?.TituloDasMensagens ?? ConfigurationManager.AppSettings["TituloInicial"];
                MessageBox.Show(ex.Message, titulo, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                this.StopWait();
            }
        }

        /// <summary>
        /// Resolve a(s) empresa(s) vinculadas ao usuário logado e define
        /// <see cref="InstanceManager.EmpresaCorrente"/>. Com uma única empresa vinculada,
        /// seleciona automaticamente. Com mais de uma, abre <see cref="Telas.Empresa.SelecaoDeEmpresaWindow"/>
        /// para o usuário escolher — se ele cancelar, usa a primeira da lista para não travar
        /// o login. Sem nenhuma empresa vinculada, não define nada (cenário tratado pelo wizard).
        /// </summary>
        private void DefinirEmpresaCorrenteDoUsuario()
        {
            var empresas = ContainerIoc.GetInstance<IUsuarioEmpresaRepositorio>()
                .RecuperarEmpresasDoUsuario(InstanceManager.UsuarioCorrente.UsuarioLogado.CodigoUsuario);

            if (empresas == null || empresas.Count == 0) return;

            var empresaEscolhida = empresas.First();

            if (empresas.Count > 1)
            {
                var telaSelecao = new Telas.Empresa.SelecaoDeEmpresaWindow(empresas);
                var confirmou = telaSelecao.ShowDialog();
                if (confirmou == true && telaSelecao.EmpresaSelecionada != null)
                    empresaEscolhida = telaSelecao.EmpresaSelecionada;
            }

            var empresa = ContainerIoc.GetInstance<Empresa>();
            empresa.DefinirEmpresaCorrente(empresaEscolhida);
            InstanceManager.EmpresaCorrente = empresa;
        }

        private void LoginWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                var repositorio = ContainerIoc.GetInstance<RepositorioBase>();
                repositorio.LimparLogDaBase();
                InstanceManager.ListaDeTipoPagamentos = repositorio.RecuperarTodos<TipoPagamento>();
                InstanceManager.Logger.Info("Login Loaded.....");
            });
        }
        
    }
}
