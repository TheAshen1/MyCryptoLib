using System.Windows.Navigation;

namespace CryptoSystems.UI
{
    public partial class MV2WizardLauncher : PageFunction<string>
    {
        private readonly MV2WizardData _wizardData = new MV2WizardData();

        protected override void Start()
        {
            base.Start();
            KeepAlive = true;

            var wizardPage1 = new MV2WizardPage1(_wizardData);
            NavigationService?.Navigate(wizardPage1);
        }
    }
}
