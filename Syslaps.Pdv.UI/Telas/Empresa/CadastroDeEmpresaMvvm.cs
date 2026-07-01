using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Syslaps.Pdv.Core;
using Syslaps.Pdv.Core.Dominio;

namespace Syslaps.Pdv.UI.Telas.Empresa
{
    public class CadastroDeEmpresaMvvm : INotifyPropertyChanged
    {
        public Action EmpresaRegistradaHandler;
        public Action<string> ErroAoRegistrarHandler;

        public CadastroDeEmpresaMvvm()
        {
            Empresa = new Entity.Empresa();
        }

        private Entity.Empresa _empresa;

        public Entity.Empresa Empresa
        {
            get { return _empresa; }
            set { _empresa = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ICommand _registrarEmpresaCommand;
        public ICommand RegistrarEmpresaCommand
        {
            get
            {
                return _registrarEmpresaCommand ?? (_registrarEmpresaCommand = new RelayCommandMvvm(param =>
                {
                    var empresaDominio = ContainerIoc.GetInstance<Core.Dominio.Empresa.Empresa>();
                    empresaDominio.RegistrarNovaEmpresa(Empresa);
                    if (empresaDominio.Status != EnumStatusDoResultado.MensagemDeSucesso)
                    {
                        ErroAoRegistrarHandler?.Invoke(empresaDominio.Mensagem);
                        return;
                    }

                    empresaDominio.VincularUsuario(InstanceManager.UsuarioCorrente.UsuarioLogado.CodigoUsuario, empresaDominio.EmpresaCorrente.CodigoEmpresa);

                    ContainerIoc.GetInstance<Bootstrap>().SemearDadosDaEmpresa(empresaDominio.EmpresaCorrente.CodigoEmpresa);

                    Empresa = new Entity.Empresa();
                    EmpresaRegistradaHandler?.Invoke();
                }, null));
            }
        }
    }
}
