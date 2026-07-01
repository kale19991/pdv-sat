using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Syslaps.Pdv.Core;
using Syslaps.Pdv.Core.Dominio;
using Syslaps.Pdv.Core.Dominio.Usuario;

namespace Syslaps.Pdv.UI.Telas.ConfiguracaoInicial
{
    public class ConfiguracaoInicialMvvm : INotifyPropertyChanged
    {
        public Action ConfiguracaoConcluidaHandler;
        public Action<string> ErroAoConcluirHandler;

        public Core.Dominio.Empresa.Empresa EmpresaDominio { get; private set; }
        public Core.Dominio.Usuario.Usuario UsuarioDominio { get; private set; }

        public ConfiguracaoInicialMvvm()
        {
            Empresa = new Entity.Empresa();
        }

        private Entity.Empresa _empresa;

        public Entity.Empresa Empresa
        {
            get { return _empresa; }
            set { _empresa = value; OnPropertyChanged(); }
        }

        private string _nomeUsuario;

        public string NomeUsuario
        {
            get { return _nomeUsuario; }
            set { _nomeUsuario = value; OnPropertyChanged(); }
        }

        public string Senha { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ICommand _concluirCommand;
        public ICommand ConcluirCommand
        {
            get
            {
                return _concluirCommand ?? (_concluirCommand = new RelayCommandMvvm(param =>
                {
                    if (string.IsNullOrEmpty(NomeUsuario) || string.IsNullOrEmpty(Senha))
                    {
                        ErroAoConcluirHandler?.Invoke("Informe o usuário e a senha do administrador.");
                        return;
                    }

                    EmpresaDominio = ContainerIoc.GetInstance<Core.Dominio.Empresa.Empresa>();
                    EmpresaDominio.RegistrarNovaEmpresa(Empresa);
                    if (EmpresaDominio.Status != EnumStatusDoResultado.MensagemDeSucesso)
                    {
                        ErroAoConcluirHandler?.Invoke(EmpresaDominio.Mensagem);
                        return;
                    }

                    ContextoAtual.CodigoEmpresaAtual = EmpresaDominio.EmpresaCorrente.CodigoEmpresa;

                    UsuarioDominio = ContainerIoc.GetInstance<Core.Dominio.Usuario.Usuario>();
                    UsuarioDominio.RegistrarNovoUsuario(NomeUsuario, Senha, EnumTipoUsuario.Administrador);
                    if (UsuarioDominio.Status != EnumStatusDoResultado.MensagemDeSucesso)
                    {
                        ErroAoConcluirHandler?.Invoke(UsuarioDominio.Mensagem);
                        return;
                    }

                    EmpresaDominio.VincularUsuario(UsuarioDominio.UsuarioLogado.CodigoUsuario, EmpresaDominio.EmpresaCorrente.CodigoEmpresa);

                    ContainerIoc.GetInstance<Bootstrap>().SemearDadosDaEmpresa(EmpresaDominio.EmpresaCorrente.CodigoEmpresa);

                    ConfiguracaoConcluidaHandler?.Invoke();
                }, null));
            }
        }
    }
}
