using System.Windows.Navigation;

namespace CryptoSystems.UI
{
    public partial class McElieseWizardNavigationWindow : NavigationWindow
    {
        public McElieseWizardNavigationWindow()
        {
            var wizardLauncher = new McElieseWizardLauncher();
            Navigate(wizardLauncher);
        }
    }
}
