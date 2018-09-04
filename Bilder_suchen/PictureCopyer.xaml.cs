using MainProgram;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Bilder_suchen
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

        private void BtnNextAndSave_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Copier.CopyCurrentPicture();
            viewModel.Pictures.SetNext();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Pictures.SetPrevious();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Copier.DeleteCurrentPicture();
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Pictures.SetNext();
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
            helpText += "W: Bild löschen\nS: Bild kopieren\n\n";
            helpText += "A: Vorheriges Bild\nD: Nächstes Bild\n\n";

            MessageBox.Show(helpText, "Hilfe");
        }

        private void ImgShow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D) viewModel.Pictures.SetNext();
            else if (e.Key == Key.A) viewModel.Pictures.SetPrevious();
            else if (e.Key == Key.W) viewModel.Copier.DeleteCurrentPicture();
            else if (e.Key == Key.S)
            {
                viewModel.Copier.CopyCurrentPicture();
                viewModel.Pictures.SetNext();
            }
        }

        private void Control_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            viewModel = e.NewValue as ViewModelCopy;
        }

        private void GdImg_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as Panel).Background = Brushes.WhiteSmoke;
        }

        private void GdImg_LostFocus(object sender, RoutedEventArgs e)
        {
            (sender as Panel).Background = Brushes.Transparent;
        }
    }
}
