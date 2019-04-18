using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace CryptoSystems.UI
{
    class McElieseWizardLauncher : PageFunction<McElieseWizardResult>
    {
        private readonly McElieseWizardData _wizardData = new McElieseWizardData();

        protected override void Start()
        {
            base.Start();

            // So we remember the WizardCompleted event registration
            KeepAlive = true;

            // Launch the wizard
            var wizardPage1 = new McElieseWizardPage1(_wizardData);
            NavigationService?.Navigate(wizardPage1);
        }
    }
}
