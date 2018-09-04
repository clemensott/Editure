using System.ComponentModel;
using System.Windows;

namespace MainProgram
{
    public class ViewModel : INotifyPropertyChanged
    {
        private const string title = "Editure";

        private bool maximized = false;
        private double windowWidth = 1000, windowHeight = 500, windowLeft = 50, windowTop = 50;
        private string windowTitle;
        private ITitle titleViewModel;
        private ViewModelSearch viewModelSearch;
        private ViewModelChoose viewModelChoose;
        private ViewModelEdit viewModelEdit;
        private ViewModelCopy viewModelCopy;
        private ViewModelMix viewModelMix;

        public WindowState WindowState
        {
            get { return maximized ? WindowState.Maximized : WindowState.Normal; }
            set
            {
                if (value == WindowState.Maximized) maximized = true;
                else if (value == WindowState.Normal) maximized = false;
                else return;

                OnPropertyChanged("WindowState");
            }
        }

        public double WindowWidth
        {
            get { return windowWidth; }
            set
            {
                if (windowWidth == value || maximized) return;

                windowWidth = value;
                OnPropertyChanged("WindowWidth");
            }
        }

        public double WindowHeight
        {
            get { return windowHeight; }
            set
            {
                if (windowHeight == value || maximized) return;

                windowHeight = value;
                OnPropertyChanged("WindowHeight");
            }
        }

        public double WindowLeft
        {
            get { return windowLeft; }
            set
            {
                if (windowLeft == value || maximized) return;

                windowLeft = value;
                OnPropertyChanged("WindowLeft");
            }
        }

        public double WindowTop
        {
            get { return windowTop; }
            set
            {
                if (windowTop == value || maximized) return;

                windowTop = value;
                OnPropertyChanged("WindowTop");
            }
        }

        public string WindowTitle
        {
            get { return windowTitle; }
            private set
            {
                if (value == windowTitle) return;

                windowTitle = value;
                OnPropertyChanged("WindowTitle");
            }
        }

        public ViewModelSearch Search { get { return viewModelSearch; } }

        public ViewModelChoose Choose { get { return viewModelChoose; } }

        public ViewModelEdit Edit { get { return viewModelEdit; } }

        public ViewModelCopy Copy { get { return viewModelCopy; } }

        public ViewModelMix Mix { get { return viewModelMix; } }

        public ViewModel()
        {
            WindowTitle = title;

            viewModelSearch = new ViewModelSearch();
            viewModelChoose = new ViewModelChoose();
            viewModelEdit = new ViewModelEdit();
            viewModelCopy = new ViewModelCopy();
            viewModelMix = new ViewModelMix();

            ChangeTitleViewModel(viewModelSearch);
        }

        public void ChangeTitleViewModel(ITitle viewModel)
        {
            if (titleViewModel != null) titleViewModel.PropertyChanged -= TitleViewModel_PropertyChanged;
            titleViewModel = viewModel;
            if (titleViewModel != null) titleViewModel.PropertyChanged += TitleViewModel_PropertyChanged;

            SetWindowTitle();
        }

        private void TitleViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "CompleteTitle") return;

            SetWindowTitle();
        }

        private void SetWindowTitle()
        {
            if (titleViewModel == null) WindowTitle = title;

            WindowTitle = titleViewModel.CompleteTitle + " - " + title;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged == null) return;

            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
