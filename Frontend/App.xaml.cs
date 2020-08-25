using Editure.Backend.CopyMove;
using System.Windows;

namespace Editure.Frontend
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        protected override void OnExit(ExitEventArgs e)
        {
            CopyMoveFiles.Current.Stop();
            base.OnExit(e);
        }
    }
}
