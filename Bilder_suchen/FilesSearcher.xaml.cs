using MainProgram;
using System.Windows;
using System.Windows.Controls;

namespace Bilder_suchen
{
    /// <summary>
    /// Interaktionslogik für FilesSearcher.xaml
    /// </summary>
    public partial class FilesSearcher : UserControl
    {
        private ViewModelSearch viewModel;

        public FilesSearcher()
        {
            InitializeComponent();
        }

        private void BtnDo_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.IsDoing) viewModel.Searcher.Stop();
            else viewModel.Searcher.Begin();
        }

        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.IsPause) viewModel.Searcher.Resume();
            else viewModel.Searcher.Pause();
        }

        private void Control_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            viewModel = e.NewValue as ViewModelSearch;
        }
    }
}
