// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

namespace BiomSharp.Nist.Nfiq
{
    public class Nfiq2Analysis
    {
        public int Score { get; private set; }

        public bool AllMeasures { get; private set; }

        public Dictionary<string, double> ActionableFeedback { get; private set; } = new Dictionary<string, double>();

        public Dictionary<string, double> QualityFeatures { get; private set; } = new Dictionary<string, double>();

        public Nfiq2Analysis(int score, bool allMeasures = false)
        {
            Score = score;
            AllMeasures = allMeasures;
        }

        public void SetActionableFeedback(List<string> ids, double[] values)
        {
            ActionableFeedback.Clear();
            int i = 0;
            ids.ForEach(aid => ActionableFeedback.Add(aid, values[i++]));
        }

        public void SetQualityFeatures(List<string> ids, double[] values)
        {
            QualityFeatures.Clear();
            int i = 0;
            ids.ForEach(aid => QualityFeatures.Add(aid, values[i++]));
        }
    }
}
