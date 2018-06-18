using System;
using System.Linq;
using Ticking.Essentials;
using Ticking.Prediction.Estimation.Publishers;

namespace Ticking.Prediction.Estimation.Methods
{
    public class SingleMarkETA : ETABase
    {
        public SingleMarkETA()
            : base()
        {
        }

        public SingleMarkETA(DateTime startTime, EstimationQualityRequirements qualityRequirements, IETAPublisher publisher)
            : base(startTime, qualityRequirements, publisher)
        {
        }

        protected override Box<TimeSpan> CalculateInner()
        {
            lock (accessLock)
            {
                if (!reportedSegments.Any())
                    return new Box<TimeSpan>();

                var last = reportedSegments.Last();
                var consumedTime = last.Value.Duration;
                var progress = last.Key;
                var remaining = Math.Ceiling(consumedTime.Ticks / progress * (TargetValue - progress));
                return new Box<TimeSpan>(TimeSpan.FromTicks(Convert.ToInt64(remaining)));
            }
        }
    }
}