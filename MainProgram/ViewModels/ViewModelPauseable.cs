using System.ComponentModel;

namespace MainProgram
{
    public class ViewModelPauseable : INotifyPropertyChanged
    {
        private bool isDoing, isPause;
        private int currentCount;
        private double totalCount;

        public bool IsDoing
        {
            get { return isDoing; }
            set
            {
                if (isDoing == value) return;

                isDoing = value;
                IsPause = false;

                OnPropertyChanged("IsDoing");
                OnPropertyChanged("IsDoingButtonText");
                OnPropertyChanged("Progress");
            }
        }


        public string IsDoingButtonText { get { return isDoing ? "Stop" : "Start"; } }

        public bool IsPause
        {
            get { return isPause; }
            set
            {
                if (isPause == value) return;

                isPause = value;
                OnPropertyChanged("PauseButtonText");
            }
        }

        public string PauseButtonText { get { return isPause ? "Weiter" : "Pause"; } }

        public double Progress { get { return isDoing && totalCount > 0 ? currentCount / totalCount : 0; } }

        public ViewModelPauseable()
        {
            IsDoing = false;
            IsPause = false;

            totalCount = 0;
        }

        public void BeginCount(int totalCount)
        {
            currentCount = 0;
            this.totalCount = totalCount;

            OnPropertyChanged("Progress");
        }

        public void IncreaseCurrentCount()
        {
            lock (this)
            {
                currentCount++;
            }

            OnPropertyChanged("Progress");

            if (currentCount != totalCount) return;

            IsDoing = false;
            IsPause = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged == null) return;

            var args = new PropertyChangedEventArgs(name);
            PropertyChanged(this, args);
            //if (Thread.CurrentThread == Application.Current.Dispatcher.Thread) PropertyChanged(this, args);
            //else Application.Current.Dispatcher.BeginInvoke(PropertyChanged, this, args);
        }
    }
}
