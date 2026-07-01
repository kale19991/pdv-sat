using System;
using System.Runtime.ExceptionServices;
using System.Threading;

namespace Syslaps.Pdv.Test.Controles
{
    /// <summary>
    /// Executa uma ação em uma thread STA dedicada, necessário para manipular
    /// controles WPF fora de uma aplicação em execução (o test runner do MSTest
    /// não garante apartment state STA na thread do teste).
    /// </summary>
    internal static class ExecutorSta
    {
        public static void Executar(Action acao)
        {
            ExceptionDispatchInfo excecaoCapturada = null;

            var thread = new Thread(() =>
            {
                try
                {
                    acao();
                }
                catch (Exception ex)
                {
                    excecaoCapturada = ExceptionDispatchInfo.Capture(ex);
                }
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            excecaoCapturada?.Throw();
        }
    }
}
