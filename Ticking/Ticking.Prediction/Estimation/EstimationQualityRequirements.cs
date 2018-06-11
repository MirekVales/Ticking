namespace Ticking.Prediction.Estimation
{
    public struct EstimationQualityRequirements
    {
        public int? MinimumDataCount { get; }
        public float? MinimumProgressLevel { get; }

        public EstimationQualityRequirements(int? minimumData, float? minimumProgress)
        {
            MinimumDataCount = minimumData;
            MinimumProgressLevel = minimumProgress;
        }

        public bool Satisfies(int count, double progress)
            => (!MinimumDataCount.HasValue || (MinimumDataCount.HasValue && count >= MinimumDataCount.Value))
            && (!MinimumProgressLevel.HasValue || (MinimumProgressLevel.HasValue && progress >= MinimumProgressLevel.Value));
    }
}