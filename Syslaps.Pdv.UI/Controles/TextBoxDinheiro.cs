using System;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Syslaps.Pdv.UI.Controles
{
    /// <summary>
    /// Campo de texto com máscara de dinheiro (pt-BR) aplicada conforme o usuário
    /// digita, no estilo calculadora: os dígitos entram da direita para a esquerda.
    /// Expõe <see cref="Valor"/> para binding do valor numérico.
    /// </summary>
    public class TextBoxDinheiro : TextBoxPadrao
    {
        public static readonly DependencyProperty ValorProperty = DependencyProperty.Register(
            nameof(Valor), typeof(decimal), typeof(TextBoxDinheiro),
            new FrameworkPropertyMetadata(0m, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValorChanged));

        public decimal Valor
        {
            get => (decimal)GetValue(ValorProperty);
            set => SetValue(ValorProperty, value);
        }

        private static readonly CultureInfo CulturaPtBr = CultureInfo.GetCultureInfo("pt-BR");
        private const int QuantidadeMaximaDeDigitos = 15;

        private string _digitos = string.Empty;
        private bool _atualizandoInternamente;

        public TextBoxDinheiro()
        {
            DataObject.AddPastingHandler(this, AoColar);
        }

        private static void OnValorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = (TextBoxDinheiro)d;
            if (textBox._atualizandoInternamente) return;

            var centavos = Math.Abs((long)(Math.Round((decimal)e.NewValue, 2) * 100));
            textBox._digitos = centavos.ToString(CultureInfo.InvariantCulture).TrimStart('0');
            textBox.AtualizarTextoFormatado();
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            _digitos = ExtrairDigitos(Text);
            base.OnTextChanged(e);
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            e.Handled = true;

            if (e.Text.Length == 1 && char.IsDigit(e.Text[0]) && _digitos.Length < QuantidadeMaximaDeDigitos)
            {
                _digitos = (_digitos + e.Text[0]).TrimStart('0');
                AtualizarValor();
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                e.Handled = true;
                if (_digitos.Length > 0)
                    _digitos = _digitos.Substring(0, _digitos.Length - 1);
                AtualizarValor();
            }
            else
            {
                base.OnPreviewKeyDown(e);
            }
        }

        private void AoColar(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
            if (!e.SourceDataObject.GetDataPresent(DataFormats.Text)) return;

            foreach (var caractere in (string)e.SourceDataObject.GetData(DataFormats.Text))
                if (char.IsDigit(caractere) && _digitos.Length < QuantidadeMaximaDeDigitos)
                    _digitos += caractere;

            _digitos = _digitos.TrimStart('0');
            AtualizarValor();
        }

        private void AtualizarValor()
        {
            _atualizandoInternamente = true;
            Valor = ObterValorAtual();
            _atualizandoInternamente = false;
            AtualizarTextoFormatado();
        }

        private decimal ObterValorAtual()
        {
            return string.IsNullOrEmpty(_digitos)
                ? 0m
                : decimal.Parse(_digitos, CultureInfo.InvariantCulture) / 100m;
        }

        private void AtualizarTextoFormatado()
        {
            Text = ObterValorAtual().ToString("C", CulturaPtBr);
            CaretIndex = Text.Length;
        }

        private static string ExtrairDigitos(string valor)
        {
            var digitos = new StringBuilder();
            foreach (var caractere in valor ?? string.Empty)
                if (char.IsDigit(caractere))
                    digitos.Append(caractere);
            return digitos.ToString().TrimStart('0');
        }
    }
}
