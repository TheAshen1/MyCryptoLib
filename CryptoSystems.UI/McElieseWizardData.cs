using CryptoSystems.ParityCheckMatrixGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSystems.UI
{
    public class McElieseWizardData
    {
        public GaloisField GaloisField { get; set; }
        public ParityCheckMatrixGeneratorEllyptic Generator { get; set; }
        public LinearCode LinearCode { get; set; }
    }
}
