using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Syslaps.Pdv.UI.Telas.Empresa
{
    /// <summary>
    /// Janela simples exibida quando o usuário logado está vinculado a mais de
    /// uma empresa, para que ele escolha em qual empresa deseja operar.
    /// </summary>
    public partial class SelecaoDeEmpresaWindow : Window
    {
        public Entity.Empresa EmpresaSelecionada { get; private set; }

        public SelecaoDeEmpresaWindow(List<Entity.Empresa> empresas)
        {
            InitializeComponent();
            LstEmpresas.ItemsSource = empresas;
            if (empresas.Count > 0)
                LstEmpresas.SelectedIndex = 0;
        }

        private void BtnConfirmar_OnClick(object sender, RoutedEventArgs e)
        {
            Confirmar();
        }

        private void LstEmpresas_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Confirmar();
        }

        private void Confirmar()
        {
            EmpresaSelecionada = LstEmpresas.SelectedItem as Entity.Empresa;
            if (EmpresaSelecionada == null) return;

            DialogResult = true;
            Close();
        }
    }
}
