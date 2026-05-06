using ImageLoader.Base;
using System.Windows.Input;

namespace ImageLoader.Elements.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        public IResolver? UrlImageViewerResolver
        {
            get => field;
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged(nameof(UrlImageViewerResolver));
                }
            }
        }
        public double TotalProgress
        {
            get => field;
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged(nameof(TotalProgress));
                }
            }
        }
        public ICommand? LoadAllCommand
        {
            get => field;
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged(nameof(LoadAllCommand));
                }
            }
        }
    }
}
