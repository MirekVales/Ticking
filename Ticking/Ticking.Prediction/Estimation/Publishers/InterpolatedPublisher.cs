using System;
using Ticking.Essentials;

namespace Ticking.Prediction.Estimation.Publishers
{
    public class InterpolatedPublisher : IETAPublisher
    {
        public Box<TimeSpan> Publish((TimeSpan Estimation, DateTime Created) lastEstimation)
        {
            if (lastEstimation.Equals(default((TimeSpan Estimation, DateTime Created))))
                return new Box<TimeSpan>();

            var diff = DateTime.Now - lastEstimation.Created;
            var remainder = Math.Max(lastEstimation.Estimation.TotalMilliseconds - diff.TotalMilliseconds, 0);
            return new Box<TimeSpan>(TimeSpan.FromMilliseconds(remainder));
        }
    }
}
