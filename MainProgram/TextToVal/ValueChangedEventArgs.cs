using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProgram
{
    public class ValueChangingEventArgs<T> : EventArgs
    {
        public bool Apply { get; set; }

        public T NewValue { get; private set; }

        public T OldValue { get; private set; }

        public ValueChangingEventArgs(T oldValue, T newValue)
        {
            Apply = true;

            NewValue = newValue;
            OldValue = oldValue;
        }
    }
}
