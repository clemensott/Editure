using System.Windows;
using MainProgram;
using System.Windows.Media.Imaging;
using System;
using System.IO;
using MainProgram.Save;

namespace Bilder_suchen
{
    public partial class MainWindow : Window
    {
        private ViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();

            viewModel = SaveClass.Load();

            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;

            if (viewModel.WindowLeft > screenWidth)
            {
                viewModel.WindowLeft = screenWidth - viewModel.WindowWidth;
            }
            else if (viewModel.WindowLeft + viewModel.WindowWidth < 0) viewModel.WindowLeft = 0;

            if (viewModel.WindowTop > screenHeight)
            {
                viewModel.WindowTop = screenHeight - viewModel.WindowHeight;
            }
            else if (viewModel.WindowTop + viewModel.WindowHeight < 0) viewModel.WindowTop = 0;

            DataContext = viewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string relPath = "Editure.ico";

                if (File.Exists(relPath)) Icon = new BitmapImage(new Uri(relPath, UriKind.Relative));
            }
            catch { }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveClass.Save(viewModel);
        }

        private void Tbi_GotFocus(object sender, RoutedEventArgs e)
        {
            ITitle subViewModel = (sender as FrameworkElement).DataContext as ITitle;

            if (subViewModel != null) viewModel.ChangeTitleViewModel(subViewModel);
        }
    }
}
