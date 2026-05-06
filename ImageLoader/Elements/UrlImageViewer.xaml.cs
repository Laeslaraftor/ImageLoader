using ImageLoader.Commands;
using ImageLoader.Elements.ViewModels;
using ImageLoader.Services;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ImageLoader.Elements
{
    public partial class UrlImageViewer : UserControl
    {
        public UrlImageViewer()
        {
            InitializeComponent();

            _viewModel = new()
            {
                DownloadCommand = new RelayCommand(_ => LoadImage()),
                CancelCommand = new RelayCommand(_ => CancelLoading())
            };
            _progressHandler = new(p => DownloadProgress = p);

            if (Content is FrameworkElement frameworkElement)
            {
                frameworkElement.DataContext = _viewModel;
            }
        }
        ~UrlImageViewer()
        {
            _imageDownloadService.Dispose();
        }

        public event EventHandler<ValueChangedEventArgs<double>>? DownloadProgressChanged;

        public string? Url
        {
            get => GetValue(UrlProperty) as string;
            set => SetValue(UrlProperty, value);
        }
        public double DownloadProgress
        {
            get => (double)GetValue(DownloadProgressProperty.DependencyProperty);
            private set => SetValue(DownloadProgressProperty, value);
        }
        public IProgressHandler<double>? ProgressHandler
        {
            get => GetValue(ProgressHandlerProperty) as IProgressHandler<double>;
            set => SetValue(ProgressHandlerProperty, value);
        }
        public IResolver? Resolver
        {
            get => GetValue(ResolverProperty) as IResolver;
            set => SetValue(ResolverProperty, value);
        }

        private readonly UrlImageViewerViewModel _viewModel;
        private readonly ImageDownloadService _imageDownloadService = new();
        private readonly Progress<double> _progressHandler;
        private CancellationTokenSource? _currentCancellationTokenSource;

        #region Управление

        public async void LoadImage()
        {
            string? imageUrl = _viewModel.Url;
            _viewModel.Image = null;

            if (string.IsNullOrEmpty(imageUrl))
            {
                return;
            }

            _viewModel.ExceptionMessage = null;
            _viewModel.DownloadFailed = false;
            Uri imageUri = new(imageUrl, UriKind.RelativeOrAbsolute);

            CancelLoading();

            CancellationTokenSource? cancellationTokenSource = _currentCancellationTokenSource;

            if (cancellationTokenSource == null ||
                !cancellationTokenSource.TryReset())
            {
                cancellationTokenSource = new();
            }

            _currentCancellationTokenSource = cancellationTokenSource;
            var cancellationToken = cancellationTokenSource.Token;
            BitmapImage? image = null;

            try
            {
                image = await _imageDownloadService.Download(imageUri, _progressHandler, cancellationToken);
            }
            catch (Exception exception)
            {
                _viewModel.ExceptionMessage = $"{exception.GetType().Name}: {exception.Message}";
                _viewModel.DownloadFailed = true;
            }

            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            _viewModel.Image = image;
        }
        public bool CancelLoading()
        {
            if (_currentCancellationTokenSource == null || 
                _currentCancellationTokenSource.IsCancellationRequested)
            {
                return true;
            }

            try
            {
                _currentCancellationTokenSource.Cancel();
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region События

        protected virtual void OnProgressChanged(double oldValue, double newValue)
        {
            ProgressHandler?.Report(this, newValue);
            DownloadProgressChanged?.Invoke(this, new(oldValue, newValue));
        }
        protected virtual void OnResolverResolveRequested(object? sender, ResolveEventArgs e)
        {
            e.Values.Add(this);
        }

        private void OnStartButtonClicked(object sender, RoutedEventArgs e)
        {
            LoadImage();
        }
        private void OnStopButtonClicked(object sender, RoutedEventArgs e)
        {
            CancelLoading();
        }

        private static void OnDownloadProgressChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UrlImageViewer view)
            {
                view.OnProgressChanged((double)e.OldValue, (double)e.NewValue);
            }
        }
        private static void OnResolverChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UrlImageViewer view)
            {
                if (e.OldValue is IResolver oldResolver)
                {
                    oldResolver.ResolveRequested -= view.OnResolverResolveRequested;
                }
                if (e.NewValue is IResolver newResolver)
                {
                    newResolver.ResolveRequested += view.OnResolverResolveRequested;
                }
            }
        }
        private static void OnUrlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UrlImageViewer view)
            {
                view._viewModel.Url = e.NewValue?.ToString();
            }
        }

        #endregion

        #region Dependency

        public static readonly DependencyProperty UrlProperty = DependencyProperty.Register(nameof(Url),
            typeof(string), typeof(UrlImageViewer), new(OnUrlChanged));
        public static readonly DependencyPropertyKey DownloadProgressProperty = DependencyProperty.RegisterReadOnly(nameof(DownloadProgress),
            typeof(double), typeof(UrlImageViewer), new(0d, OnDownloadProgressChanged));
        public static readonly DependencyProperty ProgressHandlerProperty = DependencyProperty.Register(nameof(ProgressHandler),
            typeof(IProgressHandler<double>), typeof(UrlImageViewer));
        public static readonly DependencyProperty ResolverProperty = DependencyProperty.Register(nameof(Resolver),
            typeof(IResolver), typeof(UrlImageViewer), new(OnResolverChanged));

        #endregion
    }
}
