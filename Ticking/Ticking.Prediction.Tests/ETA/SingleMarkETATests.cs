using System;
using Ticking.Prediction.Estimation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ticking.Prediction.Estimation.Methods;
using Ticking.Prediction.Estimation.Publishers;

namespace Ticking.Prediction.Tests.ETA
{
    [TestClass]
    public class SingleMarkETATests
    {
        [TestMethod]
        public void CalculatesCorrectEstimation()
        {
            var start = DateTime.Now;
            var provider = new SingleMarkETA(start, new EstimationQualityRequirements(), new DummyPublisher());
            provider.Report(start.Add(TimeSpan.FromSeconds(1)), 0.1d);
            provider.Report(start.Add(TimeSpan.FromSeconds(2)), 0.2d);
            provider.Report(start.Add(TimeSpan.FromSeconds(3)), 0.3d);
            provider.Report(start.Add(TimeSpan.FromSeconds(4)), 0.4d);
            provider.Report(start.Add(TimeSpan.FromSeconds(5)), 0.5d);
            var result = provider.Calculate();

            Assert.AreEqual(5, result.Value.TotalSeconds);
        }
    }
}
