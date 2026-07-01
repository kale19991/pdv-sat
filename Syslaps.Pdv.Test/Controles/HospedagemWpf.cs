using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace Syslaps.Pdv.Test.Controles
{
    /// <summary>
    /// Hospeda um elemento num HwndSource real (sem exibir janela visível), para que
    /// Focus()/perda de foco disparem de verdade os eventos roteados GotFocus/LostFocus
    /// do WPF — RaiseEvent isolado não é suficiente sem um PresentationSource.
    /// </summary>
    internal static class HospedagemWpf
    {
        public static void ComFocoReal(FrameworkElement elemento, Action<TextBox> comOutroElementoFocavel)
        {
            var outroFoco = new TextBox();
            var painel = new StackPanel();
            painel.Children.Add(elemento);
            painel.Children.Add(outroFoco);

            var parametros = new HwndSourceParameters("teste-headless") { Width = 200, Height = 200 };
            using (var hwndSource = new HwndSource(parametros))
            {
                hwndSource.RootVisual = painel;
                painel.Measure(new Size(200, 200));
                painel.Arrange(new Rect(0, 0, 200, 200));
                painel.UpdateLayout();

                comOutroElementoFocavel(outroFoco);
            }
        }
    }
}
