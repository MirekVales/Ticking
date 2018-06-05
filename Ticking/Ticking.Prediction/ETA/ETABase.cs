using System;
using System.Collections.Generic;
using System.Linq;
using Ticking.Essentials;

namespace Ticking.Prediction.ETA
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

        protected ETABase()
        {
            accessLock = new object();
            reportedSegments = new SortedDictionary<double, Period>();
            Start = DateTime.Now;
            qualityRequirements = new EstimationQualityRequirements();
            TargetValue = 1d;
        }

        protected ETABase(DateTime startTime, EstimationQualityRequirements qualityRequirements, double targetValue = 1d)
        {
            accessLock = new object();
            reportedSegments = new SortedDictionary<double, Period>();
            Start = startTime;
            this.qualityRequirements = qualityRequirements;
            TargetValue = targetValue;
        }


        public virtual void Report(DateTime snapshotDate, double progress)
        {
            if (progress == 0)
                return;

            lock (accessLock)
                reportedSegments[progress] = new Period(Start, snapshotDate);
        }

        public virtual Box<TimeSpan> ReportAndCalculate(DateTime snapshotDate, double progress)
        {
            Report(snapshotDate, progress);
            return Calculate();
        }

        public abstract Box<TimeSpan> Calculate();

        public void Reset()
        {
            lock (accessLock)
            {
                reportedSegments.Clear();
                Start = DateTime.Now;
            }
        }
    }
}
