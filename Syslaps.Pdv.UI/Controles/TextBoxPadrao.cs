using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;

namespace Syslaps.Pdv.UI.Controles
{
    /// <summary>
    /// TextBox base reutilizável em todas as telas: destaca o fundo em amarelo claro
    /// quando está em foco e, quando <see cref="Obrigatorio"/>, exibe um tooltip de
    /// aviso caso o campo perca o foco vazio. Serve de base para os demais campos
    /// com máscara (<see cref="TextBoxDinheiro"/>, <see cref="TextBoxCpfCnpj"/>).
    /// </summary>
    public class TextBoxPadrao : TextBox
    {
        public static readonly DependencyProperty ObrigatorioProperty = DependencyProperty.Register(
            nameof(Obrigatorio), typeof(bool), typeof(TextBoxPadrao), new PropertyMetadata(false));

        public bool Obrigatorio
        {
            get => (bool)GetValue(ObrigatorioProperty);
            set => SetValue(ObrigatorioProperty, value);
        }

        private static readonly Brush BackgroundEmFoco = CriarBrushCongelado(Color.FromRgb(0xFF, 0xFA, 0xC8));
        private static readonly Brush BordaComErro = CriarBrushCongelado(Colors.Red);

        private Brush _backgroundOriginal;
        private Brush _bordaOriginal;
        private Thickness _espessuraBordaOriginal;
        private bool _valoresOriginaisCapturados;
        private ToolTip _toolTipDeErro;
        private DispatcherTimer _timerDeErro;

        private static Brush CriarBrushCongelado(Color cor)
        {
            var brush = new SolidColorBrush(cor);
            brush.Freeze();
            return brush;
        }

        private void CapturarValoresOriginais()
        {
            if (_valoresOriginaisCapturados) return;

            _backgroundOriginal = Background;
            _bordaOriginal = BorderBrush;
            _espessuraBordaOriginal = BorderThickness;
            _valoresOriginaisCapturados = true;
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            CapturarValoresOriginais();
            base.OnGotFocus(e);
            Background = BackgroundEmFoco;
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            Background = _backgroundOriginal;
            ValidarObrigatorio();
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            if (!string.IsNullOrWhiteSpace(Text))
                LimparErro();
        }

        /// <summary>
        /// Valida o preenchimento obrigatório do campo. Pode ser estendido pelos
        /// campos derivados para acrescentar validações próprias (ex.: CPF/CNPJ).
        /// </summary>
        protected virtual void ValidarObrigatorio()
        {
            if (Obrigatorio && string.IsNullOrWhiteSpace(Text))
                ExibirErro("Campo obrigatório.");
            else
                LimparErro();
        }

        protected void ExibirErro(string mensagem)
        {
            CapturarValoresOriginais();
            BorderBrush = BordaComErro;
            BorderThickness = new Thickness(2);

            if (_toolTipDeErro == null)
                _toolTipDeErro = new ToolTip { PlacementTarget = this, Placement = PlacementMode.Bottom };

            _toolTipDeErro.Content = mensagem;
            _toolTipDeErro.IsOpen = true;

            _timerDeErro?.Stop();
            _timerDeErro = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
            _timerDeErro.Tick += FecharTooltipDeErro;
            _timerDeErro.Start();
        }

        private void FecharTooltipDeErro(object sender, EventArgs e)
        {
            _timerDeErro.Stop();
            _timerDeErro.Tick -= FecharTooltipDeErro;

            if (_toolTipDeErro != null)
                _toolTipDeErro.IsOpen = false;
        }

        protected void LimparErro()
        {
            if (_valoresOriginaisCapturados)
            {
                BorderBrush = _bordaOriginal;
                BorderThickness = _espessuraBordaOriginal;
            }

            _timerDeErro?.Stop();
            if (_toolTipDeErro != null)
                _toolTipDeErro.IsOpen = false;
        }
    }
}
