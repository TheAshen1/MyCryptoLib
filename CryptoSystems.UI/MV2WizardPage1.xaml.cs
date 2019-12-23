using System;
using System.Windows;
using System.Windows.Controls;

namespace CryptoSystems.UI
{
    public partial class MV2WizardPage1 : Page
    {
        private readonly MV2WizardData _wizardData;

        public MV2WizardPage1(MV2WizardData mV2WizardData)
        {
            InitializeComponent();
            _wizardData = mV2WizardData;
        }

        private void GenerateNewKey(object sender, RoutedEventArgs e)
        {
            var key = Mijyuoon.Crypto.MV2.Key.Generate(Mijyuoon.Crypto.MV2.KeySize.K256);
            KeyValue.Text = BitConverter.ToString(key);
            _wizardData.Key = new Mijyuoon.Crypto.MV2.Key(key);
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            var wizardPage2 = new MV2WizardPage2(_wizardData);
            NavigationService?.Navigate(wizardPage2);
        }
    }
}
