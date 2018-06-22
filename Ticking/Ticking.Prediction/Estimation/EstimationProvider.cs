using System;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using Ticking.Essentials;
using Ticking.Essentials.Timers;

namespace Ticking.Prediction.Estimation
{
    public class EstimationProvider<T> : IDisposable
    {
        readonly EstimationPool<T> estimationPool;
        readonly Action<T, Box<TimeSpan>> refreshAction;
        readonly IntervalTimer timer;

        public bool Disposed
            => timer.Disposed;

        public bool Enabled
            => timer.Enabled;

        public EstimationProvider(
            TimeSpan refreshInterval,
            ISynchronizeInvoke synchronizingObject,
            EstimationPool<T> estimationPool,
            Action<T, Box<TimeSpan>> refreshAction)
        {
            timer = new IntervalTimer(refreshInterval, synchronizingObject);
            timer.Elapsed += Timer_Elapsed;
            this.estimationPool = estimationPool;
            this.refreshAction = refreshAction;

            timer.Start();
        }

        public void Enable()
            => timer.Start();

        public void Disable()
            => timer.Stop();

        void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            estimationPool
                .Ids
                .ToList()
                .ForEach(id => refreshAction(id, estimationPool.PublishEstimation(id)));
        }

        public void Dispose()
            => timer.Dispose();
    }
}
