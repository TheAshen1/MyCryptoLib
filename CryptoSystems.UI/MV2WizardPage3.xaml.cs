using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace CryptoSystems.UI
{
    public partial class MV2WizardPage3 : Page
    {
        private readonly MV2WizardData _wizardData;

        public MV2WizardPage3(MV2WizardData mV2WizardData)
        {
            InitializeComponent();
            _wizardData = mV2WizardData;
            this.DataContext = this;
            KernelFile.Text = _wizardData.KernelFile;
            FlagFile.Text = _wizardData.FlagFile;
        }

        private void DecryptFile(object sender, System.Windows.RoutedEventArgs e)
        {
            var decoder = new Mijyuoon.Crypto.MV2.Decoder(_wizardData.Key);

            var flag = File.ReadAllBytes(_wizardData.FlagFile);
            var kernel = File.ReadAllBytes(_wizardData.KernelFile);

            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var decryptedFile = Path.Combine(assemblyFolder, Path.GetFileName(_wizardData.FileToEncrypt));
            File.WriteAllBytes(decryptedFile, decoder.Decode(flag, kernel));
            DecryptedFile.Text = decryptedFile;
        }

        private void PrevButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var wizardPage2 = new MV2WizardPage2(_wizardData);
            NavigationService?.Navigate(wizardPage2);
        }

    }
}
