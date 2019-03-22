using System.Collections.Generic;

namespace MainProgram
{
    public class CurrentItemList<T> : List<T>
    {
        public TextInt CurrentIndexBase0 { get; private set; }

        public TextInt CurrentIndexBase1 { get; private set; }

        public T CurrentItem { get { return Count > 0 ? this[CurrentIndexBase0.Value] : DefaultItem; } }

        public T DefaultItem { get; set; }

        public CurrentItemList(T defaultItem) : base()
        {
            CurrentIndexBase0 = new TextInt(-1);
            CurrentIndexBase1 = new TextInt(0);

            DefaultItem = defaultItem;

            CurrentIndexBase0.ValueChangingEvent += CurrentIndexBase0_ValueChangingEvent;
            CurrentIndexBase1.ValueChangingEvent += CurrentIndexBase1_ValueChangingEvent;
        }

        public CurrentItemList(IEnumerable<T> collection, T defaultItem) : base(collection)
        {
            CurrentIndexBase0 = new TextInt(0);
            CurrentIndexBase1 = new TextInt(1);

            DefaultItem = defaultItem;

            CurrentIndexBase0.ValueChangingEvent += CurrentIndexBase0_ValueChangingEvent;
            CurrentIndexBase1.ValueChangingEvent += CurrentIndexBase1_ValueChangingEvent;
        }

        private void CurrentIndexBase0_ValueChangingEvent(object sender, ValueChangingEventArgs<int> e)
        {
            if (e.NewValue >= 0 && e.NewValue < Count && Count > 0) CurrentIndexBase1.Value = e.NewValue + 1;
            else if (e.NewValue == -1 && Count == 0) CurrentIndexBase1.Value = e.NewValue + 1;
            else e.Apply = false;
        }

        private void CurrentIndexBase1_ValueChangingEvent(object sender, ValueChangingEventArgs<int> e)
        {
            if (e.NewValue >= 1 && e.NewValue <= Count && Count > 0) CurrentIndexBase0.Value = e.NewValue - 1;
            else if (e.NewValue == 0 && Count == 0) CurrentIndexBase0.Value = e.NewValue - 1;
            else e.Apply = false;
        }

        public void SetNext()
        {
            CurrentIndexBase0.Value = (CurrentIndexBase0.Value + 1) % Count;
        }

        public void SetPrevious()
        {
            CurrentIndexBase0.Value = (CurrentIndexBase0.Value + Count - 1) % Count;
        }

        public void RemoveCurrent()
        {
            RemoveAt(CurrentIndexBase0.Value);

            if (Count == CurrentIndexBase0.Value) CurrentIndexBase0.Value--;
        }
    }
}
