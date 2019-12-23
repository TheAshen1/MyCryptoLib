using CryptoSystems.Exceptions;
using CryptoSystems.UI.Util;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CryptoSystems.UI
{
    public partial class McElieseWizardPage1 : Page
    {
        private readonly McElieseWizardData _mcElieseWizardData;

        public int FieldBase { get; set; }
        public int FieldPower { get; set; }

        public McElieseWizardPage1(McElieseWizardData mcElieseWizardData)
        {
            InitializeComponent();
            this.DataContext = this;
            FieldBase = 2;
            FieldPower = 2;
            _mcElieseWizardData = mcElieseWizardData;
        }

        private void GenerateGaloisField(object sender, RoutedEventArgs e)
        {
            try
            {
                _mcElieseWizardData.GaloisField = new GaloisField(FieldBase, FieldPower);

                GaloisFieldAdditionTable_Preview.ItemsSource = BindingHelper.GetBindable2DArray(_mcElieseWizardData.GaloisField.AdditionTable);
                GaloisFieldMultiplicationTable_Preview.ItemsSource = BindingHelper.GetBindable2DArray(_mcElieseWizardData.GaloisField.MultiplicationTable);
                GaloisFieldDivisionTable_Preview.ItemsSource = BindingHelper.GetBindable2DArray(_mcElieseWizardData.GaloisField.DivisionTable);
            }
            catch (GaloisFieldException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            var wizardPage2 = new McElieseWizardPage2(_mcElieseWizardData);
            NavigationService?.Navigate(wizardPage2);
        }

        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            _mcElieseWizardData.GaloisField = new GaloisField(FieldBase, FieldPower);
        }

        private void c_dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DataGridTextColumn column = e.Column as DataGridTextColumn;
            Binding binding = column.Binding as Binding;
            binding.Path = new PropertyPath(binding.Path.Path + ".Value");
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var newText = (e.Source as TextBox).Text;
            if (int.TryParse(newText, out int newValue))
            {
                (e.Source as TextBox).Text = newValue % 2 == 0 ? newValue.ToString() : (newValue - 1).ToString();
            }
        }
    }
}
