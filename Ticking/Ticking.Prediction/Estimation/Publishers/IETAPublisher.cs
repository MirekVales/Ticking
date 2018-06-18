using System;
using Ticking.Essentials;

namespace Ticking.Prediction.Estimation.Publishers
{
    public interface IETAPublisher
    {
        Box<TimeSpan> Publish((TimeSpan Estimation, DateTime Created) lastEstimation);
    }
}
