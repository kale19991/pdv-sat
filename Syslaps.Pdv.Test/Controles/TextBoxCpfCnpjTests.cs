using System.Windows;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Syslaps.Pdv.UI.Controles;

namespace Syslaps.Pdv.Test.Controles
{
    [TestClass]
    public class TextBoxCpfCnpjTests
    {
        [TestMethod]
        public void DevoIniciarComDocumentoVazioETextoVazio()
        {
            ExecutorSta.Executar(() =>
            {
                var textBox = new TextBoxCpfCnpj();

                Assert.AreEqual(string.Empty, textBox.Documento);
                Assert.AreEqual(string.Empty, textBox.Text);
            });
        }

        [TestMethod]
        public void DevoAplicarMascaraDeCpfComOnzeDigitos()
        {
            ExecutorSta.Executar(() =>
            {
                var textBox = new TextBoxCpfCnpj { Documento = "11144477735" };

                Assert.AreEqual("111.444.777-35", textBox.Text);
            });
        }

        [TestMethod]
        public void DevoAplicarMascaraDeCnpjComQuatorzeDigitos()
        {
            ExecutorSta.Executar(() =>
            {
                var textBox = new TextBoxCpfCnpj { Documento = "11222333000181" };

                Assert.AreEqual("11.222.333/0001-81", textBox.Text);
            });
        }

        [TestMethod]
        public void DevoAplicarMascaraParcialEnquantoDigitaMenosDeOnzeDigitos()
        {
            ExecutorSta.Executar(() =>
            {
                var textBox = new TextBoxCpfCnpj { Documento = "111444" };

                Assert.AreEqual("111.444", textBox.Text);
            });
        }

        [TestMethod]
        public void DevoNormalizarDocumentoParaApenasDigitosAoDefinirComPontuacao()
        {
            ExecutorSta.Executar(() =>
            {
                var textBox = new TextBoxCpfCnpj { Documento = "111.444.777-35" };

                Assert.AreEqual("11144477735", textBox.Documento);
                Assert.AreEqual("111.444.777-35", textBox.Text);
            });
        }

        [TestMethod]
        public void DevoExibirErroQuandoDocumentoInvalidoAoPerderFoco()
        {
            ExecutorSta.Executar(() =>
            {
                var textBox = new TextBoxCpfCnpj { Documento = "12345678900", BorderBrush = Brushes.Black };

                HospedagemWpf.ComFocoReal(textBox, outroFoco =>
                {
                    textBox.Focus();
                    outroFoco.Focus();

                    var borda = (SolidColorBrush)textBox.BorderBrush;
                    Assert.AreEqual(Colors.Red, borda.Color);
                });
            });
        }

        [TestMethod]
        public void DevoValidarDocumentoAtribuidoDiretamenteViaTextComoEmUmBinding()
        {
            ExecutorSta.Executar(() =>
            {
                var textBox = new TextBoxCpfCnpj { BorderBrush = Brushes.Black };
                textBox.Text = "123.456.789-00";

                HospedagemWpf.ComFocoReal(textBox, outroFoco =>
                {
                    textBox.Focus();
                    outroFoco.Focus();

                    var borda = (SolidColorBrush)textBox.BorderBrush;
                    Assert.AreEqual(Colors.Red, borda.Color);
                });
            });
        }
    }
}
