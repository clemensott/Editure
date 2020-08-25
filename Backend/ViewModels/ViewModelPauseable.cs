using System.ComponentModel;

namespace Editure.Backend.ViewModels
{
    public class ViewModelPauseable : INotifyPropertyChanged
    {
        private bool isDoing, isPause;
        private int currentCount;
        private double totalCount;

        public bool IsDoing
        {
            get => isDoing;
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


        public string IsDoingButtonText => isDoing ? "Stop" : "Start";

        public bool IsPause
        {
            get => isPause;
            set
            {
                if (isPause == value) return;

                isPause = value;
                OnPropertyChanged("PauseButtonText");
            }
        }

        public string PauseButtonText => isPause ? "Resume" : "Pause";

        public double Progress => isDoing && totalCount > 0 ? currentCount / totalCount : 0;

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

            PropertyChangedEventArgs args = new PropertyChangedEventArgs(name);
            PropertyChanged(this, args);
            //if (Thread.CurrentThread == Application.Current.Dispatcher.Thread) PropertyChanged(this, args);
            //else Application.Current.Dispatcher.BeginInvoke(PropertyChanged, this, args);
        }
    }
}
