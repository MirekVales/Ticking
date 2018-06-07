using System;
using System.Linq;
using Ticking.Essentials;

namespace Ticking.Prediction.Estimation.Methods
{
    public class LinearRegressionETA : ETABase
    {
        public double CorrelationCoefficient { get; private set; }

        public LinearRegressionETA()
            : base()
        {
        }

        public LinearRegressionETA(DateTime startTime, EstimationQualityRequirements qualityRequirements)
            : base(startTime, qualityRequirements)
        {
        }

        protected override Box<TimeSpan> CalculateInner()
        {
            lock (accessLock)
            {
                (double slope, double y) = CalculateSlope();

                var duration = (TargetValue - y) / slope;

                if (double.IsNaN(duration) || double.IsInfinity(duration))
                    return new Box<TimeSpan>();

                return new Box<TimeSpan>(TimeSpan.FromMilliseconds(duration));
            }
        }

        (double Slope, double Y) CalculateSlope()
        {
            if (!reportedSegments.Any())
            {
                CorrelationCoefficient = double.NaN;
                return (0, 0);
            }

            double count = reportedSegments.Count;
            var durationSum = 0d;
            var durationSumPow = 0d;
            var progressSum = 0d;
            var progressSumPow = 0d;
            var durationProgressSum = 0d;

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

            var durationSumPow2 = durationSum * durationSum;
            var progressSumPow2 = progressSum * progressSum;

            var slope = (count * durationProgressSum - durationSum * progressSum)
                / (count * durationSumPow - durationSumPow2);

            CorrelationCoefficient = (count * durationProgressSum - durationSum * progressSum)
                / Math.Sqrt((count * durationSumPow - durationSumPow2) * (count * progressSumPow - progressSumPow2));

            return (slope, reportedSegments.Last().Key);
        }
    }
}
