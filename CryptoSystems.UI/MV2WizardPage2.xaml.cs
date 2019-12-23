using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace CryptoSystems.UI
{
    public partial class MV2WizardPage2 : Page
    {
        private readonly MV2WizardData _wizardData;
        public int Rounds { get; set; }
        public MV2WizardPage2(MV2WizardData mV2WizardData)
        {
            InitializeComponent();
            _wizardData = mV2WizardData;
            this.DataContext = this;
            Rounds = 16;
        }

        private void SelectFile(object sender, RoutedEventArgs e)
        {
            var fileDialog = new Microsoft.Win32.OpenFileDialog();
            var ok = fileDialog.ShowDialog();
            if (ok.HasValue && ok.Value)
            {
                _wizardData.FileToEncrypt = fileDialog.FileName;

                var encoder = new Mijyuoon.Crypto.MV2.Encoder(_wizardData.Key, rounds: Rounds);

                var encryptedData = encoder.Encode(File.ReadAllBytes(_wizardData.FileToEncrypt));

                string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                _wizardData.FlagFile = Path.Combine(assemblyFolder, "flags");
                _wizardData.KernelFile = Path.Combine(assemblyFolder, "kernel");
                File.WriteAllBytes(_wizardData.FlagFile, encryptedData.Flag);
                File.WriteAllBytes(_wizardData.KernelFile, encryptedData.Residual);

                FlagFile.Text = _wizardData.FlagFile;
                KernelFile.Text = _wizardData.KernelFile;
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            var wizardPage1 = new MV2WizardPage3(_wizardData);
            NavigationService?.Navigate(wizardPage1);
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            var wizardPage1 = new MV2WizardPage1(_wizardData);
            NavigationService?.Navigate(wizardPage1);
        }
    }
}
