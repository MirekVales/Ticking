using System;

namespace Ticking.Essentials
{
    public class Box<T>
    {
        public T Value { get; }

        public bool HasValue => !default(T).Equals(Value);

        public Box()
            => Value = default(T);

        public Box(T value)
            => Value = value;

        public T Out(Func<T> noValue)
            => HasValue
            ? Value
            : noValue();
    }
}