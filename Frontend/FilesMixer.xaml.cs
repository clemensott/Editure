using System.Windows;
using System.Windows.Controls;
using Editure.Backend.ViewModels;

namespace Editure.Frontend
{
    /// <summary>
    /// Interaktionslogik für FilesMixer.xaml
    /// </summary>
    public partial class FilesMixer : UserControl
    {
        private ViewModelMix viewModel;

        public FilesMixer()
        {
            InitializeComponent();
        }

        private void btnDo_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.IsMix) viewModel.Mixer.Mix();
            else viewModel.Mixer.Demix();
        }

        private void Control_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            viewModel = e.NewValue as ViewModelMix;
        }
    }
}
