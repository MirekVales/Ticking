using System;
using System.Linq;
using Ticking.Essentials;

namespace Ticking.Prediction.ETA
{
    public class SingleMarkETA : ETABase
    {
        public SingleMarkETA()
        {
        }

        public SingleMarkETA(DateTime startTime, EstimationQualityRequirements qualityRequirements)
            : base(startTime, qualityRequirements)
        {
        }

        public override Box<TimeSpan> Calculate()
        {
            lock (accessLock)
            {
                if (!EstimationAvailable || !reportedSegments.Any())
                    return new Box<TimeSpan>();

                var last = reportedSegments.Last();
                var consumedTime = last.Value.Duration;
                var progress = last.Key;
                var remaining = Math.Ceiling(consumedTime.Ticks / progress * (TargetValue - progress));
                return new Box<TimeSpan>(TimeSpan.FromTicks((long)remaining));
            }
        }
    }
}