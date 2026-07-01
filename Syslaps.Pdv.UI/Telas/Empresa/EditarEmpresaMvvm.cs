using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Syslaps.Pdv.Core;
using Syslaps.Pdv.Core.Dominio;

namespace Syslaps.Pdv.UI.Telas.Empresa
{
    public class EditarEmpresaMvvm : INotifyPropertyChanged
    {
        public System.Action EmpresaAtualizadaHandler;
        public System.Action<string> ErroAoAtualizarHandler;

        public EditarEmpresaMvvm()
        {
            Empresa = InstanceManager.EmpresaCorrente.EmpresaCorrente;
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

        private ICommand _atualizarEmpresaCommand;
        public ICommand AtualizarEmpresaCommand
        {
            get
            {
                return _atualizarEmpresaCommand ?? (_atualizarEmpresaCommand = new RelayCommandMvvm(param =>
                {
                    var empresaDominio = ContainerIoc.GetInstance<Core.Dominio.Empresa.Empresa>();
                    empresaDominio.AtualizarEmpresa(Empresa);
                    if (empresaDominio.Status != EnumStatusDoResultado.MensagemDeSucesso)
                    {
                        ErroAoAtualizarHandler?.Invoke(empresaDominio.Mensagem);
                        return;
                    }

                    InstanceManager.EmpresaCorrente = empresaDominio;
                    EmpresaAtualizadaHandler?.Invoke();
                }, null));
            }
        }
    }
}
