using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Syslaps.Pdv.Cross;

namespace Syslaps.Pdv.UI.Controles
{
    /// <summary>
    /// Campo de texto com máscara de CPF ou CNPJ aplicada conforme o usuário digita:
    /// até 11 dígitos usa a máscara de CPF (000.000.000-00), acima disso passa
    /// automaticamente para a máscara de CNPJ (00.000.000/0000-00). Valida o
    /// documento ao perder o foco. Expõe <see cref="Documento"/> com os dígitos.
    /// </summary>
    public class TextBoxCpfCnpj : TextBoxPadrao
    {
        public static readonly DependencyProperty DocumentoProperty = DependencyProperty.Register(
            nameof(Documento), typeof(string), typeof(TextBoxCpfCnpj),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnDocumentoChanged));

        public string Documento
        {
            get => (string)GetValue(DocumentoProperty);
            set => SetValue(DocumentoProperty, value);
        }

        private const int TamanhoMaximoCnpj = 14;

        private string _digitos = string.Empty;
        private bool _atualizandoInternamente;

        public TextBoxCpfCnpj()
        {
            DataObject.AddPastingHandler(this, AoColar);
        }

        private static void OnDocumentoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = (TextBoxCpfCnpj)d;
            if (textBox._atualizandoInternamente) return;

            var digitosLimpos = SomenteDigitos((string)e.NewValue);
            textBox._digitos = digitosLimpos;
            textBox.AtualizarTextoFormatado();

            if (digitosLimpos != (string)e.NewValue)
            {
                textBox._atualizandoInternamente = true;
                textBox.Documento = digitosLimpos;
                textBox._atualizandoInternamente = false;
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            _digitos = SomenteDigitos(Text);
            base.OnTextChanged(e);
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            e.Handled = true;

            if (e.Text.Length == 1 && char.IsDigit(e.Text[0]) && _digitos.Length < TamanhoMaximoCnpj)
            {
                _digitos += e.Text[0];
                AtualizarDocumento();
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                e.Handled = true;
                if (_digitos.Length > 0)
                    _digitos = _digitos.Substring(0, _digitos.Length - 1);
                AtualizarDocumento();
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

            foreach (var caractere in SomenteDigitos((string)e.SourceDataObject.GetData(DataFormats.Text)))
                if (_digitos.Length < TamanhoMaximoCnpj)
                    _digitos += caractere;

            AtualizarDocumento();
        }

        private void AtualizarDocumento()
        {
            _atualizandoInternamente = true;
            Documento = _digitos;
            _atualizandoInternamente = false;
            AtualizarTextoFormatado();
        }

        private void AtualizarTextoFormatado()
        {
            Text = FormatarDocumento(_digitos);
            CaretIndex = Text.Length;
        }

        protected override void ValidarObrigatorio()
        {
            base.ValidarObrigatorio();

            if (!string.IsNullOrEmpty(_digitos) && !_digitos.IsCpfOrCnpj())
                ExibirErro("CPF/CNPJ inválido.");
        }

        private static string SomenteDigitos(string valor)
        {
            var digitos = new StringBuilder();
            foreach (var caractere in valor ?? string.Empty)
                if (char.IsDigit(caractere))
                    digitos.Append(caractere);
            return digitos.ToString();
        }

        private static string FormatarDocumento(string digitos)
        {
            return digitos.Length <= 11
                ? FormatarComMascara(digitos, "000.000.000-00")
                : FormatarComMascara(digitos, "00.000.000/0000-00");
        }

        private static string FormatarComMascara(string digitos, string mascara)
        {
            var resultado = new StringBuilder();
            var indiceDigito = 0;

            foreach (var caractereMascara in mascara)
            {
                if (indiceDigito >= digitos.Length)
                    break;

                if (caractereMascara == '0')
                {
                    resultado.Append(digitos[indiceDigito]);
                    indiceDigito++;
                }
                else
                {
                    resultado.Append(caractereMascara);
                }
            }

            return resultado.ToString();
        }
    }
}
