using log4net;
using Syslaps.Pdv.Core;
using Syslaps.Pdv.Core.Dominio.Impressora;
using Syslaps.Pdv.Entity;
using System.Collections.Generic;
using Caixa = Syslaps.Pdv.Core.Dominio.Caixa.Caixa;
using Usuario = Syslaps.Pdv.Core.Dominio.Usuario.Usuario;

namespace Syslaps.Pdv.UI
{
    public static class InstanceManager
    {
        private static Core.Dominio.Empresa.Empresa empresaCorrente;

        public static Caixa CaixaCorrente { get; set; }
        public static Usuario UsuarioCorrente { get; set; }

        public static Core.Dominio.Empresa.Empresa EmpresaCorrente
        {
            get { return empresaCorrente; }
            set
            {
                empresaCorrente = value;
                ContextoAtual.CodigoEmpresaAtual = value?.EmpresaCorrente?.CodigoEmpresa;
            }
        }
        public static List<Produto> ListaDeProdutosDoPdv { get; set; }
        public static List<TipoPagamento> ListaDeTipoPagamentos { get; set; }
        public static ILog Logger { get; set; }
        public static Parametros Parametros { get; set; }
        public static Cupom Cupom { get; set; }
    }
}
