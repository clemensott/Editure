using System;
using System.ComponentModel;

namespace MainProgram
{
    public abstract class Text<T> : INotifyPropertyChanged where T : IEquatable<T>, new()
    {
        public delegate void ValueChangingEventHandler(object sender, ValueChangingEventArgs<T> e);

        public event ValueChangingEventHandler ValueChangingEvent;

        private bool isChanging;
        private T value, changingValue;
        private string valueString;

        public T Value
        {
            get { return value; }
            set
            {
                if (this.value.Equals(value) || (isChanging && changingValue.Equals(value)) || !ApplyValue(value)) return;

                this.value = value;
                valueString = value.ToString();

                OnPropertyChanged("Value");
                OnPropertyChanged("String");
            }
        }

        public string String { get { return valueString; } set { TryParse(value); } }

        public Text(T value)
        {
            this.value = value;
            valueString = value.ToString();
        }

        public Text(string value)
        {
            String = value;
        }

        private bool ApplyValue(T newValue)
        {
            isChanging = true;
            changingValue = newValue;

            ValueChangingEventArgs<T> eventArgs = new ValueChangingEventArgs<T>(value, newValue);

            ValueChangingEvent?.Invoke(this, eventArgs);

            isChanging = false;

            return eventArgs.Apply;
        }

        public bool TryParse(string s)
        {
            if (s == valueString) return false;

            T newValue;

            if (TryConvert(s, out newValue))
            {
                if (!ApplyValue(newValue)) return false;

                value = newValue;
                OnPropertyChanged("Value");
            }

            valueString = s;
            OnPropertyChanged("String");

            return true;
        }

        public void SetOnlyValue(T value)
        {
            if (!ApplyValue(value)) return;

            this.value = value;
            OnPropertyChanged("Value");
        }

        public void ResetString()
        {
            valueString = value.ToString();

            OnPropertyChanged("String");
        }

        protected abstract bool TryConvert(string s, out T value);

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged == null) return;

            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public static implicit operator string(Text<T> t)
        {
            if (t == null) return "";

            return t.String;
        }

        public override string ToString()
        {
            return String;
        }
    }
}
