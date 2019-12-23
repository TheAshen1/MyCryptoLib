using System.Windows.Navigation;

namespace CryptoSystems.UI
{
    public partial class MV2WizardNavigationWindow : NavigationWindow
    {
        public MV2WizardNavigationWindow()
        {
            var wizardLauncher = new MV2WizardLauncher();
            Navigate(wizardLauncher);
        }
    }
}
