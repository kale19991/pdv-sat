using System.Windows;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Syslaps.Pdv.UI.Controles;

namespace Syslaps.Pdv.Test.Controles
{
    [TestClass]
    public class TextBoxPadraoTests
    {
        [TestMethod]
        public void DevoDestacarFundoAoReceberFoco()
        {
            ExecutorSta.Executar(() =>
            {
                var textBox = new TextBoxPadrao { Background = Brushes.White };

                HospedagemWpf.ComFocoReal(textBox, outroFoco =>
                {
                    textBox.Focus();

                    var fundoEmFoco = (SolidColorBrush)textBox.Background;
                    Assert.AreEqual(Color.FromRgb(0xFF, 0xFA, 0xC8), fundoEmFoco.Color);
                });
            });
        }

        [TestMethod]
        public void DevoRestaurarFundoOriginalAoPerderFoco()
        {
            ExecutorSta.Executar(() =>
            {
                var textBox = new TextBoxPadrao { Background = Brushes.White };

                HospedagemWpf.ComFocoReal(textBox, outroFoco =>
                {
                    textBox.Focus();
                    outroFoco.Focus();

                    Assert.AreEqual(Brushes.White, textBox.Background);
                });
            });
        }

        [TestMethod]
        public void DevoExibirErroQuandoObrigatorioEVazioAoPerderFoco()
        {
            ExecutorSta.Executar(() =>
            {
                var textBox = new TextBoxPadrao { Obrigatorio = true, BorderBrush = Brushes.Black };

                HospedagemWpf.ComFocoReal(textBox, outroFoco =>
                {
                    textBox.Focus();
                    outroFoco.Focus();

                    var borda = (SolidColorBrush)textBox.BorderBrush;
                    Assert.AreEqual(Colors.Red, borda.Color);
                    Assert.AreEqual(new Thickness(2), textBox.BorderThickness);
                });
            });
        }

        [TestMethod]
        public void NaoDevoExibirErroQuandoNaoObrigatorioEVazio()
        {
            ExecutorSta.Executar(() =>
            {
                var textBox = new TextBoxPadrao { Obrigatorio = false, BorderBrush = Brushes.Black };

                HospedagemWpf.ComFocoReal(textBox, outroFoco =>
                {
                    textBox.Focus();
                    outroFoco.Focus();

                    Assert.AreEqual(Brushes.Black, textBox.BorderBrush);
                });
            });
        }

        [TestMethod]
        public void DevoLimparErroAoPreencherOCampoObrigatorio()
        {
            ExecutorSta.Executar(() =>
            {
                var textBox = new TextBoxPadrao { Obrigatorio = true, BorderBrush = Brushes.Black, BorderThickness = new Thickness(1) };

                HospedagemWpf.ComFocoReal(textBox, outroFoco =>
                {
                    textBox.Focus();
                    outroFoco.Focus();
                    textBox.Text = "preenchido";

                    Assert.AreEqual(Brushes.Black, textBox.BorderBrush);
                    Assert.AreEqual(new Thickness(1), textBox.BorderThickness);
                });
            });
        }
    }
}
