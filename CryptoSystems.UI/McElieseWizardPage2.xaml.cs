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
    /// <summary>
    /// Interaction logic for McElieseWizardPage2.xaml
    /// </summary>
    public partial class McElieseWizardPage2 : Page
    {
        public McElieseWizardData McElieseWizardData { get; }

        public int Degree { get; set; }

        public McElieseWizardPage2(McElieseWizardData mcElieseWizardData)
        {
            InitializeComponent();
            McElieseWizardData = mcElieseWizardData;
            Degree = 2;
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            var wizardPage1 = new McElieseWizardPage1(McElieseWizardData);
            NavigationService?.Navigate(wizardPage1);
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GenerateEllypticCurve(object sender, RoutedEventArgs e)
        {
            McElieseWizardData.Generator = new ParityCheckMatrixGeneratorEllyptic(Degree);
            McElieseWizardData.Generator.Generate(McElieseWizardData.LinearCode);
            Points_Preview.ItemsSource = BindingHelper.GetBindable2DArray(McElieseWizardData.Generator.Points);
        }
    }
}
