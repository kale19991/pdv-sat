using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Syslaps.Pdv.Core;
using Syslaps.Pdv.Core.Dominio;
using Syslaps.Pdv.Core.Dominio.Usuario;

namespace Syslaps.Pdv.Test.Model
{
    [TestClass]
    public class MultiEmpresa
    {
        private void RecriarBancoVazio()
        {
            var bootstrap = ContainerIoc.GetInstance<Bootstrap>();
            bootstrap.ExcluirTabelas();
            bootstrap.CriarBancoDeDados();
            bootstrap.CriarDadosIniciais();
        }

        private void RegistrarEmpresaEUsuarioAdministrador(string razaoSocial, string cpfCnpj, string nomeUsuario, string senha, out Entity.Empresa empresa, out Entity.Usuario usuario)
        {
            var empresaDominio = ContainerIoc.GetInstance<Core.Dominio.Empresa.Empresa>();
            empresaDominio.RegistrarNovaEmpresa(new Entity.Empresa { RazaoSocial = razaoSocial, NomeFantasia = razaoSocial, CpfCnpj = cpfCnpj });
            Assert.IsTrue(empresaDominio.Status == EnumStatusDoResultado.MensagemDeSucesso, empresaDominio.Mensagem);

            ContextoAtual.CodigoEmpresaAtual = empresaDominio.EmpresaCorrente.CodigoEmpresa;

            var usuarioDominio = ContainerIoc.GetInstance<Core.Dominio.Usuario.Usuario>();
            usuarioDominio.RegistrarNovoUsuario(nomeUsuario, senha, EnumTipoUsuario.Administrador);
            Assert.IsTrue(usuarioDominio.Status == EnumStatusDoResultado.MensagemDeSucesso, usuarioDominio.Mensagem);

            empresaDominio.VincularUsuario(usuarioDominio.UsuarioLogado.CodigoUsuario, empresaDominio.EmpresaCorrente.CodigoEmpresa);

            empresa = empresaDominio.EmpresaCorrente;
            usuario = usuarioDominio.UsuarioLogado;
        }

        [TestMethod]
        public void DevoCriarPrimeiraEmpresaEUsuarioAdministrador()
        {
            RecriarBancoVazio();

            Entity.Empresa empresa;
            Entity.Usuario usuario;
            RegistrarEmpresaEUsuarioAdministrador("Loja Um", "11.111.111/1111-11", "admin1", "senha123", out empresa, out usuario);

            var empresasDoUsuario = ContainerIoc.GetInstance<IUsuarioEmpresaRepositorio>().RecuperarEmpresasDoUsuario(usuario.CodigoUsuario);
            Assert.AreEqual(1, empresasDoUsuario.Count);
            Assert.AreEqual(empresa.CodigoEmpresa, empresasDoUsuario[0].CodigoEmpresa);

            var login = ContainerIoc.GetInstance<Core.Dominio.Usuario.Usuario>();
            login.LogarUsuario("admin1", "senha123");
            Assert.IsTrue(login.Status == EnumStatusDoResultado.MensagemDeSucesso);
        }

        [TestMethod]
        public void DevoRegistrarSegundaEmpresaEVincularAoMesmoUsuario()
        {
            RecriarBancoVazio();

            Entity.Empresa primeiraEmpresa;
            Entity.Usuario usuario;
            RegistrarEmpresaEUsuarioAdministrador("Loja Um", "11.111.111/1111-11", "admin2", "senha123", out primeiraEmpresa, out usuario);

            var usuarioEmpresaRepositorio = ContainerIoc.GetInstance<IUsuarioEmpresaRepositorio>();
            Assert.AreEqual(1, usuarioEmpresaRepositorio.RecuperarEmpresasDoUsuario(usuario.CodigoUsuario).Count);

            var segundaEmpresaDominio = ContainerIoc.GetInstance<Core.Dominio.Empresa.Empresa>();
            segundaEmpresaDominio.RegistrarNovaEmpresa(new Entity.Empresa { RazaoSocial = "Loja Dois", NomeFantasia = "Loja Dois", CpfCnpj = "22.222.222/2222-22" });
            Assert.IsTrue(segundaEmpresaDominio.Status == EnumStatusDoResultado.MensagemDeSucesso, segundaEmpresaDominio.Mensagem);

            segundaEmpresaDominio.VincularUsuario(usuario.CodigoUsuario, segundaEmpresaDominio.EmpresaCorrente.CodigoEmpresa);

            var empresasDoUsuario = usuarioEmpresaRepositorio.RecuperarEmpresasDoUsuario(usuario.CodigoUsuario);
            Assert.AreEqual(2, empresasDoUsuario.Count);
        }

        [TestMethod]
        public void NaoDevoEnxergarProdutoDeOutraEmpresa()
        {
            RecriarBancoVazio();

            Entity.Empresa empresaA;
            Entity.Usuario usuarioA;
            RegistrarEmpresaEUsuarioAdministrador("Loja A", "11.111.111/1111-11", "adminA", "senha123", out empresaA, out usuarioA);

            Entity.Empresa empresaB;
            Entity.Usuario usuarioB;
            RegistrarEmpresaEUsuarioAdministrador("Loja B", "22.222.222/2222-22", "adminB", "senha123", out empresaB, out usuarioB);

            ContextoAtual.CodigoEmpresaAtual = empresaA.CodigoEmpresa;
            var produtoRepositorio = ContainerIoc.GetInstance<Core.Dominio.Produto.IProdutoRepositorio>();
            produtoRepositorio.Inserir(new Entity.Produto
            {
                CodigoDeBarra = Guid.NewGuid().ToString("N"),
                Empresa_CodigoEmpresa = empresaA.CodigoEmpresa,
                TipoProduto = "Produto",
                Modelo = "Modelo",
                Descricao = "Produto da Loja A",
                DescricaoBusca = "produtodalojaa",
                TipoUnidade = "Unidade",
                Marca = "Marca",
                Categoria = "Categoria",
                SubCategoria = "SubCategoria",
                Ativo = true,
                ExibirNoPdv = true,
                CodigoParaCupom = "0001"
            });

            ContextoAtual.CodigoEmpresaAtual = empresaB.CodigoEmpresa;
            Assert.AreEqual(0, produtoRepositorio.RecuperarListaDeProdutosDoPdv().Count);

            ContextoAtual.CodigoEmpresaAtual = empresaA.CodigoEmpresa;
            Assert.AreEqual(1, produtoRepositorio.RecuperarListaDeProdutosDoPdv().Count);
        }

        [TestMethod]
        public void SenhaDeveSerArmazenadaComHashENaoEmTextoPlano()
        {
            RecriarBancoVazio();

            Entity.Empresa empresa;
            Entity.Usuario usuario;
            RegistrarEmpresaEUsuarioAdministrador("Loja Um", "11.111.111/1111-11", "adminSenha", "minhaSenha", out empresa, out usuario);

            var usuarioRepositorio = ContainerIoc.GetInstance<IUsuarioRepositorio>();
            var usuarioPersistido = usuarioRepositorio.RecuperarUsuarioPorNome("adminSenha");

            Assert.AreNotEqual("minhaSenha", usuarioPersistido.Senha);
            StringAssert.StartsWith(usuarioPersistido.Senha, "PBKDF2-SHA256$");

            var loginCorreto = ContainerIoc.GetInstance<Core.Dominio.Usuario.Usuario>();
            loginCorreto.LogarUsuario("adminSenha", "minhaSenha");
            Assert.IsTrue(loginCorreto.Status == EnumStatusDoResultado.MensagemDeSucesso);

            var loginErrado = ContainerIoc.GetInstance<Core.Dominio.Usuario.Usuario>();
            loginErrado.LogarUsuario("adminSenha", "senhaErrada");
            Assert.IsFalse(loginErrado.Status == EnumStatusDoResultado.MensagemDeSucesso);
        }

        [TestMethod]
        public void ExisteAlgumUsuarioDeveRefletirEstadoDoBanco()
        {
            RecriarBancoVazio();

            var usuarioRepositorio = ContainerIoc.GetInstance<IUsuarioRepositorio>();
            Assert.IsFalse(usuarioRepositorio.ExisteAlgumUsuario());

            Entity.Empresa empresa;
            Entity.Usuario usuario;
            RegistrarEmpresaEUsuarioAdministrador("Loja Um", "11.111.111/1111-11", "adminExiste", "senha123", out empresa, out usuario);

            Assert.IsTrue(usuarioRepositorio.ExisteAlgumUsuario());
        }
    }
}
