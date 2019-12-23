using System.Windows;

namespace CryptoSystems.UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartMcElieseWizard(object sender, RoutedEventArgs e)
        {
            var wizard = new McElieseWizardNavigationWindow();
            wizard.ShowDialog();
        }

        private void StartMV2Wizard(object sender, RoutedEventArgs e)
        {
            var wizard = new MV2WizardNavigationWindow();
            wizard.ShowDialog();
        }
    }
}
