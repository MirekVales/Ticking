using System;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using Ticking.Essentials;

namespace Ticking.Prediction.Estimation
{
    public class EstimationProvider<T> : IDisposable
    {
        readonly object accessLock = new object();
        readonly Timer timer;
        readonly EstimationPool<T> estimationPool;
        readonly Action<T, Box<TimeSpan>> refreshAction;
        bool disposed;

        public bool Disposed
        {
            get
            {
                lock (accessLock)
                    return disposed;
            }
        }

        public EstimationProvider(
            TimeSpan refreshInterval,
            ISynchronizeInvoke synchronizingObject,
            EstimationPool<T> estimationPool,
            Action<T, Box<TimeSpan>> refreshAction)
        {
            timer = new Timer((int)refreshInterval.TotalMilliseconds);
            timer.SynchronizingObject = synchronizingObject;
            timer.Elapsed += Timer_Elapsed;
            this.estimationPool = estimationPool;
            this.refreshAction = refreshAction;

            timer.Start();
        }

        void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (accessLock)
            {
                if (disposed)
                    return;

                timer.Stop();

                estimationPool
                    .Ids
                    .ToList()
                    .ForEach(id => refreshAction(id, estimationPool.GetInterpolatedEstimation(id)));

                timer.Start();
            }
        }


        public void Dispose()
        {
            lock (accessLock)
            {
                timer.Stop();
                timer.Dispose();
                disposed = true;
            }
        }
    }
}
