using Mijyuoon.Crypto.MV2;

namespace CryptoSystems.UI
{
    public class MV2WizardData
    {
        public Key Key { get; internal set; }
        public EncodeResult EncodedFile { get; internal set; }
        public string FlagFile { get; internal set; }
        public string KernelFile { get; internal set; }
        public string FileToEncrypt { get; internal set; }
    }
}
