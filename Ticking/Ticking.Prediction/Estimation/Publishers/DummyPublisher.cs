using System;
using Ticking.Essentials;

namespace Ticking.Prediction.Estimation.Publishers
{
    public class DummyPublisher : IETAPublisher
    {
        public Box<TimeSpan> Publish((TimeSpan Estimation, DateTime Created) estimation)
        {
            if (estimation.Equals(default((TimeSpan Estimation, DateTime Created))))
                return new Box<TimeSpan>();

            return new Box<TimeSpan>(estimation.Estimation);
        }
    }
}
