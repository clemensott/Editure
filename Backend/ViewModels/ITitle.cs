using System.ComponentModel;

namespace Editure.Backend.ViewModels
{
    public interface ITitle : INotifyPropertyChanged
    {
        string Title { get; }

        string CompleteTitle { get; }
    }
}
