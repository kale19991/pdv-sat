using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Syslaps.Pdv.UI.Controles;

namespace Syslaps.Pdv.Test.Controles
{
    [TestClass]
    public class TextBoxDinheiroTests
    {
        private static readonly CultureInfo CulturaPtBr = CultureInfo.GetCultureInfo("pt-BR");

        [TestMethod]
        public void DevoIniciarComValorZeroETextoVazio()
        {
            ExecutorSta.Executar(() =>
            {
                var textBox = new TextBoxDinheiro();

                Assert.AreEqual(0m, textBox.Valor);
                Assert.AreEqual(string.Empty, textBox.Text);
            });
        }

        [TestMethod]
        public void DevoFormatarComoMoedaAoDefinirValor()
        {
            ExecutorSta.Executar(() =>
            {
                var textBox = new TextBoxDinheiro { Valor = 1234.56m };

                Assert.AreEqual(1234.56m.ToString("C", CulturaPtBr), textBox.Text);
            });
        }

        [TestMethod]
        public void DevoAtualizarTextoAoAlterarValorNovamente()
        {
            ExecutorSta.Executar(() =>
            {
                var textBox = new TextBoxDinheiro { Valor = 10m };

                textBox.Valor = 5.9m;

                Assert.AreEqual(5.9m.ToString("C", CulturaPtBr), textBox.Text);
            });
        }

        [TestMethod]
        public void DevoArredondarParaDuasCasasDecimais()
        {
            ExecutorSta.Executar(() =>
            {
                var textBox = new TextBoxDinheiro { Valor = 9.999m };

                Assert.AreEqual(10m.ToString("C", CulturaPtBr), textBox.Text);
            });
        }
    }
}
