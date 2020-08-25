using System;

namespace Editure.Backend.TextToVal
{
    public class ValueChangingEventArgs<T> : EventArgs
    {
        public bool Apply { get; set; }

        public T NewValue { get; }

        public T OldValue { get; }

        public ValueChangingEventArgs(T oldValue, T newValue)
        {
            Apply = true;

            NewValue = newValue;
            OldValue = oldValue;
        }
    }
}
