using System;

namespace Ticking.Essentials
{
    public static class WorkflowExtensions
    {
        public static TResult LockedAction<TResult>(this object @lock, Func<TResult> action)
        {
            lock (@lock)
                return action();
        }

        public static void LockedAction(this object @lock, Action action)
        {
            lock (@lock)
                action();
        }
    }
}
