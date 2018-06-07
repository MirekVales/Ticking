using System;
using System.Collections.Generic;
using System.Linq;
using Ticking.Essentials;

namespace Ticking.Prediction.Estimation.Methods
{
    public class LinearDiscountedETA : ETABase
    {
        readonly float imminenceFactor;

        public LinearDiscountedETA()
        {
            imminenceFactor = 0.75f;
        }

        public LinearDiscountedETA(DateTime startTime, float imminenceFactor, EstimationQualityRequirements qualityRequirements)
            : base(startTime, qualityRequirements)
        {
            this.imminenceFactor = imminenceFactor;
        }

        protected override Box<TimeSpan> CalculateInner()
        {
            var weights = GetWeights(reportedSegments.Count).Reverse().ToArray();
            var speed = GetSegmentSpeed().Select((s, i) => s * weights[i]).Sum();

            if (speed == 0)
                return new Box<TimeSpan>();

            var remainder = TargetValue - reportedSegments.LastOrDefault().Key;
            return new Box<TimeSpan>(TimeSpan.FromMilliseconds(remainder * speed));
        }

        IEnumerable<double> GetWeights(int count)
        {
            yield return imminenceFactor;

            var remainder = 1 - imminenceFactor;
            for (var i = count - 1; i > 0; i--)
            {
                var part = remainder / i;
                remainder -= part;
                yield return part;
            }
        }

        IEnumerable<double> GetSegmentSpeed()
        {
            lock (accessLock)
                foreach (var pair in reportedSegments)
                {
                    yield return GetSpeed(pair.Value, pair.Key);
                }
        }

        double GetSpeed(Period period, double progress)
            => period.Duration.TotalMilliseconds / progress;
    }
}