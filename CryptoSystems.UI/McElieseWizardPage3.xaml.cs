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
    public partial class McElieseWizardPage3 : Page
    {
        private readonly McElieseWizardData _mcElieseWizardData;

        public int N { get; set; }
        public int K { get; set; }
        public int D { get; set; }
        public int T { get; set; }

        public McElieseWizardPage3(McElieseWizardData mcElieseWizardData)
        {
            InitializeComponent();
            N = 8;
            K = 2;
            D = 6;
            T = 2;
            _mcElieseWizardData = mcElieseWizardData;
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            var wizardPage1 = new McElieseWizardPage1(_mcElieseWizardData);
            NavigationService?.Navigate(wizardPage1);
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
