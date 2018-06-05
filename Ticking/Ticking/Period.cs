using System;

namespace Ticking.Essentials
{
    public struct Period
    {
        public DateTime From { get; }
        public TimeSpan Duration { get; }

        public DateTime To => From.Add(Duration);

        public Period(DateTime from, DateTime to)
        {
            From = from;
            Duration = to - from;
        }

        public Period(DateTime from, TimeSpan period)
        {
            From = from;
            Duration = period;
        }

        public bool Contains(Period period)
            => period.From >= From && period.To <= To;

        public Period Inflate(TimeSpan duration)
            => new Period(From.Subtract(duration), To.Add(duration));

        public Period Move(TimeSpan duration)
            => new Period(From.Add(duration), To.Add(duration));
    }
}
