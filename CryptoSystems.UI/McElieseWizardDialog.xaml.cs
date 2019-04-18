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
    /// Interaction logic for McElieseWizardDialog.xaml
    /// </summary>
    public partial class McElieseWizardDialog : NavigationWindow
    {
        public McElieseWizardData WizardData { get; private set; }


        public McElieseWizardDialog()
        {
            InitializeComponent();

            // Launch the wizard
            var wizardLauncher = new McElieseWizardLauncher();
            Navigate(wizardLauncher);
        }
    }
}
