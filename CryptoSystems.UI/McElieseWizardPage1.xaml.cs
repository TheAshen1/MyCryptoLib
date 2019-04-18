using CryptoSystems.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CryptoSystems.UI
{
    /// <summary>
    /// Interaction logic for McElieseWizardPage1.xaml
    /// </summary>
    public partial class McElieseWizardPage1 : Page
    {
        public int FieldBase { get; set; }
        public int FieldPower { get; set; }

        public McElieseWizardData McElieseWizardData { get; }

        public McElieseWizardPage1(McElieseWizardData mcElieseWizardData)
        {
            InitializeComponent();
            this.DataContext = this;
            McElieseWizardData = mcElieseWizardData;
            FieldBase = 2;
            FieldPower = 3;
        }

        private void Input_Changed(object sender, TextChangedEventArgs e)
        {
            try
            {
                McElieseWizardData.GaloisField = new GaloisField(FieldBase, FieldPower);
            }
            catch (GaloisFieldException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            McElieseWizardData.GaloisField = new GaloisField(FieldBase, FieldPower);
        }
    }
}
