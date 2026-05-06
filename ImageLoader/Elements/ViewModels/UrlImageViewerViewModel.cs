using ImageLoader.Base;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ImageLoader.Elements.ViewModels
{
    public class UrlImageViewerViewModel : ObservableObject
    {
        public BitmapImage? Image
        {
            get => field;
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged(nameof(Image));
                }
            }
        }
        public string? Url
        {
            get => field;
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged(nameof(Url));
                }
            }
        }
        public ICommand? DownloadCommand
        {
            get => field;
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged(nameof(DownloadCommand));
                }
            }
        }
        public ICommand? CancelCommand
        {
            get => field;
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged(nameof(CancelCommand));
                }
            }
        }
        public string? ExceptionMessage
        {
            get => field;
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged(nameof(ExceptionMessage));
                }
            }
        }
        public bool DownloadFailed
        {
            get => field;
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged(nameof(DownloadFailed));
                }
            }
        }
    }
}
