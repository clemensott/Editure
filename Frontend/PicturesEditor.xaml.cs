using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Editure.Backend;
using Editure.Backend.ViewModels;

namespace Editure.Frontend
{
    /// <summary>
    /// Interaktionslogik für PicturesEditor.xaml
    /// </summary>
    public partial class PicturesEditor : UserControl
    {
        private ViewModelEdit viewModel;

        public PicturesEditor()
        {
            InitializeComponent();
        }

        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Editor.Open();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Pictures.SetPrevious();
        }

        private void BtnSaveAndNext_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Editor.SaveCurrentPicture();
            viewModel.Pictures.SetNext();
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Pictures.SetNext();
        }

        private void BtnAllDo_Click(object sender, RoutedEventArgs e)
        {
            if (!viewModel.IsDoing) viewModel.Editor.Begin();
            else viewModel.Editor.Stop();
        }

        private void BtnAllPause_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.IsPause) viewModel.Editor.Resume();
            else viewModel.Editor.Pause();

        }

        private void ImgShow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                IntPoint currentPoint = Mouse.GetPosition(Application.Current.MainWindow);

                viewModel.Editor.BeginMove(currentPoint);
                imgShow.Focus();
            }
            else if (e.ChangedButton == MouseButton.Right) ShowHelp();
        }

        private void ShowHelp()
        {
            string helpText = "";

            helpText += "Shortcuts:\n\n";
            helpText += "W: Bild nach Oben\nA: Bild nach Links\nS: Bild nach Unten\nD: Bild nach Rechts\n\n";
            helpText += "Q: Vorheriges Bild\nE: Nächstes Bild\nEnter: Bild speichen und nächstes Bild\n\n";
            helpText += "Shift: Verschieben mal 5\nAlt: Verschieben mal 10\nStrg: Verschieben mal 20";

            MessageBox.Show(helpText, "Hilfe");
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) viewModel.Editor.EndMove();
            else
            {
                IntPoint currentPoint = Mouse.GetPosition(Application.Current.MainWindow);

                viewModel.Editor.Move(currentPoint, imgShow.ActualWidth);
            }
        }

        private void imgShow_KeyDown(object sender, KeyEventArgs e)
        {
            int factor = GetMoveFactor();

            if (Keyboard.IsKeyDown(Key.D)) viewModel.Editor.Move(new IntPoint(1 * factor, 0));
            else if (Keyboard.IsKeyDown(Key.A)) viewModel.Editor.Move(new IntPoint(-1 * factor, 0));
            else if (Keyboard.IsKeyDown(Key.W)) viewModel.Editor.Move(new IntPoint(0, 1 * factor));
            else if (Keyboard.IsKeyDown(Key.S)) viewModel.Editor.Move(new IntPoint(0, -1 * factor));
            else if (e.Key == Key.Q) viewModel.Pictures.SetPrevious();
            else if (e.Key == Key.E) viewModel.Pictures.SetNext();
            else if (e.Key == Key.Enter)
            {
                viewModel.Editor.SaveCurrentPicture();
                viewModel.Pictures.SetNext();
            }
        }

        private int GetMoveFactor()
        {
            int shift = 4, alt = 9, ctrl = 19;

            shift = 1 + shift * ((Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) ? 1 : 0);
            alt = 1 + alt * ((Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt)) ? 1 : 0);
            ctrl = 1 + ctrl * ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) ? 1 : 0);

            return shift * alt * ctrl;
        }

        private void Control_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            viewModel = e.NewValue as ViewModelEdit;
        }

        private void GdImg_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as Panel).Background = Brushes.Gray;
        }

        private void GdImg_LostFocus(object sender, RoutedEventArgs e)
        {
            (sender as Panel).Background = Brushes.Transparent;
        }
    }
}
