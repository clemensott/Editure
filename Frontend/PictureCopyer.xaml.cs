using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Editure.Backend.ViewModels;

namespace Editure.Frontend
{
    /// <summary>
    /// Interaktionslogik für PictureCopyer.xaml
    /// </summary>
    public partial class PictureCopyer : UserControl
    {
        private ViewModelCopy viewModel;

        public PictureCopyer()
        {
            InitializeComponent();
        }

        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Copier.Open();
        }

        private async void BtnNextAndSave_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Copier.CopyCurrentPicture();
            await viewModel.SetNextPictureAsync();
        }

        private async void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            await viewModel.SetPreviousPictureAsync();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Copier.DeleteCurrentPicture();
        }

        private async void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            await viewModel.SetNextPictureAsync();
        }

        private void ImgShow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) imgShow.Focus();
            else if (e.ChangedButton == MouseButton.Right) ShowHelp();
        }

        private void ShowHelp()
        {
            string helpText = "";

            helpText += "Shortcuts:\n\n";
            helpText += "W: Delete\nS: Copy\n\n";
            helpText += "A: Previous\nD: Next\n\n";

            MessageBox.Show(helpText, "Help");
        }

        private async void ImgShow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D) await viewModel.SetNextPictureAsync();
            else if (e.Key == Key.A) await viewModel.SetPreviousPictureAsync();
            else if (e.Key == Key.W) viewModel.Copier.DeleteCurrentPicture();
            else if (e.Key == Key.S)
            {
                viewModel.Copier.CopyCurrentPicture();
                await viewModel.SetNextPictureAsync();
            }
        }

        private void Control_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            viewModel = e.NewValue as ViewModelCopy;
        }

        private void GdImg_GotFocus(object sender, RoutedEventArgs e)
        {
            ((Panel) sender).Background = Brushes.WhiteSmoke;
        }

        private void GdImg_LostFocus(object sender, RoutedEventArgs e)
        {
            ((Panel) sender).Background = Brushes.Transparent;
        }
    }
}