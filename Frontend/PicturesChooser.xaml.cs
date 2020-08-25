using System.Windows;
using System.Windows.Controls;
using Editure.Backend.ViewModels;

namespace Editure.Frontend
{
    /// <summary>
    /// Interaktionslogik für PicturesChooser.xaml
    /// </summary>
    public partial class PicturesChooser : UserControl
    {
        private ViewModelChoose viewModel;

        public PicturesChooser()
        {
            InitializeComponent();
        }

        private void BtnDo_Click(object sender, RoutedEventArgs e)
        {
            if (!viewModel.IsDoing) viewModel.Chooser.Begin();
            else viewModel.Chooser.Stop();
        }

        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.IsPause) viewModel.Chooser.Resume();
            else viewModel.Chooser.Pause();
        }

        private void Control_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            viewModel = e.NewValue as ViewModelChoose;
        }
    }
}
