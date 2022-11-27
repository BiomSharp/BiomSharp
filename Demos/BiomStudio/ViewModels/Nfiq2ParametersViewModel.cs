// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using System.ComponentModel;

using BiomSharp.Nist.Nfiq;

namespace BiomStudio.ViewModels
{
    [TypeDescriptionProvider(typeof(ViewModelDescriptionProvider))]
    [DisplayName("NIST NFIQ2")]
    [Description("Fingerprint quality measure")]
    public class Nfiq2ParametersViewModel : ViewModel
    {
        [DisplayName("Version")]
        [Description("NFIQ2 implementation version.")]
        public string Version { get; set; } = "";

        [DisplayName("Score")]
        [Description("NFIQ2 global score (0 <= s <= 100)"
            + "\nExcellent: > 63; Good: > 37; Fair: > 13; Poor: <= 13")]
        public int Score { get; set; } = 0;

        [DisplayName("Actionable Feedback")]
        [Description("Actionable feed back parameters:")]
        [ReadOnly(true)]
        public KeyValueCollection<double> ActionableFeedback { get; set; }

        [DisplayName("Quality Features")]
        [Description("Quality feature parameters")]
        [ReadOnly(true)]
        public KeyValueCollection<double> QualityFeatures { get; set; }

        public Nfiq2ParametersViewModel(string version, Nfiq2Analysis nfiq2)
        {
            Version = version;
            Score = nfiq2.Score;
            QualityFeatures = new KeyValueCollection<double>(nfiq2.QualityFeatures, true);
            ActionableFeedback = new KeyValueCollection<double>(nfiq2.ActionableFeedback, true);
            ReadOnly = true;
        }
    }
}
