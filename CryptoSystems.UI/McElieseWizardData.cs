namespace CryptoSystems.UI
{
    public class McElieseWizardData
    {
        public GaloisField GaloisField { get; set; }
        public LinearCode LinearCode { get; set; }
        public McElieseEllyptic McEliese { get; internal set; }
    }
}
