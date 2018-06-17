using System;
using System.ComponentModel;
using System.Timers;

namespace Ticking.Essentials.Timers
{
    public class IntervalTimer : IDisposable
    {
        readonly object accessLock;
        readonly int interval;
        readonly ISynchronizeInvoke synchronizeObject;
        readonly Timer timer;

        public event ElapsedEventHandler Elapsed;

        bool disposed;

        public bool Disposed
        {
            get
            {
                lock (accessLock)
                    return disposed;
            }
        }

        public bool Enabled
        {
            get => timer.Enabled;
        }

        public IntervalTimer(TimeSpan interval, ISynchronizeInvoke synchronizeObject = null)
        {
            accessLock = new object();
            this.interval = (int)Math.Ceiling(interval.TotalMilliseconds);
            this.synchronizeObject = synchronizeObject;
            timer = new Timer(this.interval);
            timer.SynchronizingObject = synchronizeObject;
            timer.Elapsed += Timer_Elapsed;
        }

        void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (accessLock)
            {
                timer.Stop();

                if (timer.Interval != interval)
                    timer.Interval = interval;

                Elapsed?.Invoke(this, e);

                timer.Start();
            }
        }

        public void Start()
        {
            lock (accessLock)
            {
                if (disposed)
                    throw new InvalidOperationException();

                timer.Start();
            }
        }

        public void Stop()
        {
            lock (accessLock)
            {
                if (disposed)
                    throw new InvalidOperationException();

                timer.Stop();
            }
        }

        public void FastStart()
        {
            timer.Interval = 1;

            if (!timer.Enabled)
                Start();
        }

        public void Dispose()
        {
            lock (accessLock)
            {
                if (disposed)
                    return;

                timer.Stop();
                timer.Dispose();
                disposed = true;
            }
        }
    }
}
