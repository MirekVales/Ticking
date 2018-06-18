using System;
using System.Collections.Generic;
using System.Linq;
using Ticking.Essentials;
using Ticking.Prediction.Estimation.Publishers;

namespace Ticking.Prediction.Estimation.Methods
{
    public abstract class ETABase
    {
        protected readonly object accessLock;
        protected readonly SortedDictionary<double, Period> reportedSegments;
        protected readonly EstimationQualityRequirements qualityRequirements;

        public bool EstimationAvailable
        { get { lock (accessLock) return qualityRequirements.Satisfies(reportedSegments.Count, reportedSegments.LastOrDefault().Key); } }

        public DateTime Start { get; protected set; }
        public TimeSpan Elapsed => DateTime.Now - Start;
        public double TargetValue { get; protected set; }

        public (TimeSpan Estimation, DateTime Created) LastEstimation { get; private set; }

        IETAPublisher publisher;

        public IETAPublisher Publisher
        {
            get { lock (accessLock) return publisher; }
            set { lock (accessLock) publisher = value; }
        }

        protected ETABase()
        {
            accessLock = new object();
            reportedSegments = new SortedDictionary<double, Period>();
            Start = DateTime.Now;
            qualityRequirements = new EstimationQualityRequirements();
            TargetValue = 1d;
            publisher = new DummyPublisher();
        }

        protected ETABase(DateTime startTime, EstimationQualityRequirements qualityRequirements, IETAPublisher publisher, double targetValue = 1d)
        {
            accessLock = new object();
            reportedSegments = new SortedDictionary<double, Period>();
            Start = startTime;
            this.qualityRequirements = qualityRequirements;
            TargetValue = targetValue;
            this.publisher = publisher;
        }

        public virtual void Report(DateTime snapshotDate, double progress)
        {
            if (progress == 0 || double.IsNaN(progress) || double.IsInfinity(progress))
                return;

            lock (accessLock)
                reportedSegments[progress] = new Period(Start, snapshotDate);
        }

        public virtual Box<TimeSpan> ReportAndCalculate(DateTime snapshotDate, double progress)
        {
            Report(snapshotDate, progress);
            var value = Calculate();
            if (value.HasValue)
                LastEstimation = (value.Value, DateTime.Now);

            return value;
        }

        protected abstract Box<TimeSpan> CalculateInner();

        public Box<TimeSpan> Calculate()
        {
            if (!EstimationAvailable)
                return new Box<TimeSpan>();

            return CalculateInner();
        }

        public void Reset()
        {
            lock (accessLock)
            {
                reportedSegments.Clear();
                Start = DateTime.Now;
            }
        }

        public Box<TimeSpan> PublishEstimation()
        {
            lock (accessLock)
                return publisher.Publish(LastEstimation);
        }
    }
}
