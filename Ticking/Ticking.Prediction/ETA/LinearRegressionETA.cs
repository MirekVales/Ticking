using System;
using System.Linq;
using Ticking.Essentials;

namespace Ticking.Prediction.ETA
{
    public class LinearRegressionETA : ETABase
    {
        public double CorrelationCoefficient { get; private set; }

        public LinearRegressionETA()
        {
        }

        public LinearRegressionETA(DateTime startTime, EstimationQualityRequirements qualityRequirements)
            : base(startTime, qualityRequirements)
        {
        }

        public override Box<TimeSpan> Calculate()
        {
            if (!EstimationAvailable)
                return new Box<TimeSpan>();

            lock (accessLock)
            {
                (double slope, double y) = CalculateCorrelation();

                var duration = (TargetValue - y) / slope;

                if (double.IsNaN(duration) || double.IsInfinity(duration))
                    return new Box<TimeSpan>();

                return new Box<TimeSpan>(TimeSpan.FromMilliseconds(duration));
            }
        }

        (double Slope, double Y) CalculateCorrelation()
        {
            if (!reportedSegments.Any())
            {
                CorrelationCoefficient = double.NaN;
                return (0, 0);
            }

            double count = reportedSegments.Count;
            double durationSum = 0.0;
            double durationSumPow = 0.0;
            double progressSum = 0.0;
            double progressSumPow = 0.0;
            double durationProgressSum = 0.0;

            foreach (var pair in reportedSegments)
            {
                var duration = pair.Value.Duration.TotalMilliseconds;
                durationSum += duration;
                durationSumPow += duration * duration;
                var progress = pair.Key;
                progressSum += progress;
                progressSumPow += progress * progress;
                durationProgressSum += progress * duration;
            }

            double durationSumPow2 = durationSum * durationSum;
            double progressSumPow2 = progressSum * progressSum;

            var slope = (count * durationProgressSum - durationSum * progressSum)
                / (count * durationSumPow - durationSumPow2);

            CorrelationCoefficient = (count * durationProgressSum - durationSum * progressSum)
                / Math.Sqrt((count * durationSumPow - durationSumPow2) * (count * progressSumPow - progressSumPow2));

            return (slope, reportedSegments.Last().Key);
        }
    }
}
