using System;
using System.Collections.Generic;
using System.Linq;
using Ticking.Essentials;
using Ticking.Prediction.Estimation.Methods;

namespace Ticking.Prediction.Estimation
{
    public class EstimationPool<T>
    {
        readonly object accessLock = new object();
        readonly Dictionary<T, ETABase> estimationMethods;

        public IEnumerable<T> Ids {
            get
            {
                lock (accessLock)
                    return estimationMethods.Keys;
            }
        }

        public EstimationPool()
        {
            estimationMethods = new Dictionary<T, ETABase>();
        }

        public EstimationPool(IEnumerable<(T Id, ETABase Eta)> methods)
        {
            estimationMethods = methods.ToDictionary(k => k.Id, v => v.Eta);
        }

        public EstimationPool(T id, ETABase estimationMethod)
        {
            estimationMethods = new Dictionary<T, ETABase>();
            estimationMethods[id] = estimationMethod;
        }

        public void Add(T id, ETABase estimationMethod)
        {
            lock (accessLock)
                estimationMethods[id] = estimationMethod;
        }

        public void Report(T id, DateTime dateTime, double progress)
            => Apply(id, m => m.Report(dateTime, progress));

        public Box<TimeSpan> ReportAndCalculate(T id, DateTime dateTime, double progress)
            => Apply(id, m => m.ReportAndCalculate(dateTime, progress));

        public Box<TimeSpan> GetInterpolatedEstimation(T id)
            => Apply(id, m => m.GetInterpolatedEstimation());

        public bool EstimationAvailable(T id)
            => Apply(id, m => m.EstimationAvailable);

        public void Reset(T id)
            => Apply(id, m => m.Reset());

        TResult Apply<TResult>(T id, Func<ETABase, TResult> func)
        {
            lock (accessLock)
                return func(estimationMethods[id]);
        }

        void Apply(T id, Action<ETABase> action)
        {
            lock (accessLock)
                action(estimationMethods[id]);
        }
    }
}
