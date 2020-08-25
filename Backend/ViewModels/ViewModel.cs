using System.ComponentModel;
using System.Windows;

namespace Editure.Backend.ViewModels
{
    public class ViewModel : INotifyPropertyChanged
    {
        private const string title = "Editure";

        private bool maximized = false;
        private double windowWidth = 1000, windowHeight = 500, windowLeft = 50, windowTop = 50;
        private string windowTitle;
        private ITitle titleViewModel;
        private readonly ViewModelSearch viewModelSearch;
        private readonly ViewModelChoose viewModelChoose;
        private readonly ViewModelEdit viewModelEdit;
        private readonly ViewModelCopy viewModelCopy;
        private readonly ViewModelMix viewModelMix;

        public WindowState WindowState
        {
            get => maximized ? WindowState.Maximized : WindowState.Normal;
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
            get => windowWidth;
            set
            {
                if (windowWidth == value || maximized) return;

                windowWidth = value;
                OnPropertyChanged("WindowWidth");
            }
        }

        public double WindowHeight
        {
            get => windowHeight;
            set
            {
                if (windowHeight == value || maximized) return;

                windowHeight = value;
                OnPropertyChanged("WindowHeight");
            }
        }

        public double WindowLeft
        {
            get => windowLeft;
            set
            {
                if (windowLeft == value || maximized) return;

                windowLeft = value;
                OnPropertyChanged("WindowLeft");
            }
        }

        public double WindowTop
        {
            get => windowTop;
            set
            {
                if (windowTop == value || maximized) return;

                windowTop = value;
                OnPropertyChanged("WindowTop");
            }
        }

        public string WindowTitle
        {
            get => windowTitle;
            private set
            {
                if (value == windowTitle) return;

                windowTitle = value;
                OnPropertyChanged("WindowTitle");
            }
        }

        public ViewModelSearch Search => viewModelSearch;

        public ViewModelChoose Choose => viewModelChoose;

        public ViewModelEdit Edit => viewModelEdit;

        public ViewModelCopy Copy => viewModelCopy;

        public ViewModelMix Mix => viewModelMix;

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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
