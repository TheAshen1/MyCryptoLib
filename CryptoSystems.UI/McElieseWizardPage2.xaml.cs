using CryptoSystems.ParityCheckMatrixGenerators;
using CryptoSystems.UI.Util;
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
    public partial class McElieseWizardPage2 : Page
    {
        private readonly McElieseWizardData _mcElieseWizardData;

        public int N { get; set; }
        public int K { get; set; }
        public int D { get; set; }
        public int T { get; set; }

        public McElieseWizardPage2(McElieseWizardData mcElieseWizardData)
        {
            InitializeComponent();
            this.DataContext = this;
            N = 8;
            K = 2;
            D = 6;
            T = 2;
            try
            {
                _mcElieseWizardData = mcElieseWizardData;
                _mcElieseWizardData.McEliese = new McElieseEllyptic(N, K, D, T, _mcElieseWizardData.GaloisField);

                ScamblerMatrix_Preview.ItemsSource = BindingHelper.GetBindable2DArray(_mcElieseWizardData.McEliese.PrivateKey.ScramblerMatrix);
                InverseScamblerMatrix_Preview.ItemsSource = BindingHelper.GetBindable2DArray(_mcElieseWizardData.McEliese.PrivateKey.InverseScramblerMatrix);
                Permutation_Preview.ItemsSource = BindingHelper.GetBindable2DArray(_mcElieseWizardData.McEliese.PrivateKey.Permutation);
                Mask_Preview.ItemsSource = BindingHelper.GetBindable2DArray(_mcElieseWizardData.McEliese.PrivateKey.Mask);
                InversePermutation_Preview.ItemsSource = BindingHelper.GetBindable2DArray(_mcElieseWizardData.McEliese.PrivateKey.InversePermutation);
                InverseMask_Preview.ItemsSource = BindingHelper.GetBindable2DArray(_mcElieseWizardData.McEliese.PrivateKey.InverseMask);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            var wizardPage2 = new McElieseWizardPage2(_mcElieseWizardData);
            NavigationService?.Navigate(wizardPage2);
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            var wizardPage3 = new McElieseWizardPage3(_mcElieseWizardData);
            NavigationService?.Navigate(wizardPage3);
        }

        private void c_dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DataGridTextColumn column = e.Column as DataGridTextColumn;
            Binding binding = column.Binding as Binding;
            binding.Path = new PropertyPath(binding.Path.Path + ".Value");
        }
    }
}
