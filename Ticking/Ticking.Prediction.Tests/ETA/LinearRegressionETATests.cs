using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Ticking.Prediction.Estimation;
using Ticking.Prediction.Estimation.Methods;

namespace Ticking.Prediction.Tests.ETA
{
    [TestClass]
    public class LinearRegressionETATests
    {
        [TestMethod]
        public void CalculatesCorrectEstimation()
        {
            var start = DateTime.Now;
            var provider = new LinearRegressionETA(start, new EstimationQualityRequirements());
            provider.Report(start.Add(TimeSpan.FromSeconds(1)), 0.1d);
            provider.Report(start.Add(TimeSpan.FromSeconds(2)), 0.2d);
            provider.Report(start.Add(TimeSpan.FromSeconds(3)), 0.3d);
            provider.Report(start.Add(TimeSpan.FromSeconds(4)), 0.4d);
            provider.Report(start.Add(TimeSpan.FromSeconds(5)), 0.5d);
            var result = provider.Calculate();

            Assert.AreEqual(5, result.Value.TotalSeconds);
            Assert.AreEqual(1d, provider.CorrelationCoefficient);
        }

        [TestMethod]
        public void CalculatesCorrectEstimation2()
        {
            var start = DateTime.Now;
            var provider = new LinearRegressionETA(start, new EstimationQualityRequirements());
            provider.Report(start.Add(TimeSpan.FromSeconds(1)), 0.1d);
            provider.Report(start.Add(TimeSpan.FromSeconds(2)), 0.1d);
            provider.Report(start.Add(TimeSpan.FromSeconds(3)), 0.15d);
            provider.Report(start.Add(TimeSpan.FromSeconds(4)), 0.98d);
            provider.Report(start.Add(TimeSpan.FromSeconds(5)), 0.99d);
            var result = provider.Calculate();

            Assert.AreEqual(28.5714, result.Value.TotalMilliseconds);
            Assert.AreNotEqual(1d, provider.CorrelationCoefficient);
        }
    }
}
