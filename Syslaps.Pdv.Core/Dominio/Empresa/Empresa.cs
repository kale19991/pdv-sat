using System.Linq;
using Syslaps.Pdv.Core.Dominio.Base;
using Syslaps.Pdv.Core.Dominio.Usuario;
using Syslaps.Pdv.Cross;

namespace Syslaps.Pdv.Core.Dominio.Empresa
{
    public class Empresa : ModeloBase
    {
        private readonly IEmpresaRepositorio _repositorio;
        private readonly IUsuarioEmpresaRepositorio _usuarioEmpresaRepositorio;

        public Entity.Empresa EmpresaCorrente { get; private set; }

        public Empresa(IEmpresaRepositorio repositorio, IUsuarioEmpresaRepositorio usuarioEmpresaRepositorio)
        {
            _repositorio = repositorio;
            _usuarioEmpresaRepositorio = usuarioEmpresaRepositorio;
        }

        public void RegistrarNovaEmpresa(Entity.Empresa empresa)
        {
            EmpresaCorrente = empresa;
            EmpresaCorrente.CodigoEmpresa = GerarCodigoUnico();
            EmpresaCorrente.Ativa = true;

            EmpresaCorrente.TryValidateAnnotation().ToList().ForEach(item => AdicionarMensagem(item.ErrorMessage, EnumStatusDoResultado.RegraDeNegocioInvalida));
            if (Status != EnumStatusDoResultado.MensagemDeSucesso) return;

            _repositorio.Inserir(EmpresaCorrente);
            AdicionarMensagem("Empresa registrada com sucesso.");
        }

        /// <summary>
        /// Define a empresa corrente sem persistir nada, usado para restaurar o
        /// contexto de sessão (ex.: após login) a partir de uma empresa já existente.
        /// </summary>
        public void DefinirEmpresaCorrente(Entity.Empresa empresa)
        {
            EmpresaCorrente = empresa;
        }

        public void VincularUsuario(string codigoUsuario, string codigoEmpresa)
        {
            _usuarioEmpresaRepositorio.Inserir(new Entity.UsuarioEmpresa
            {
                Usuario_CodigoUsuario = codigoUsuario,
                Empresa_CodigoEmpresa = codigoEmpresa
            });

            AdicionarMensagem("Usuário vinculado à empresa com sucesso.");
        }
    }
}
