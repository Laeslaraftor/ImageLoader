using ImageLoader.Base;
using ImageLoader.Commands;
using ImageLoader.Elements;
using ImageLoader.Elements.ViewModels;
using System.Windows;

namespace ImageLoader
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new()
            {
                UrlImageViewerResolver = _imageViewersResolver,
                LoadAllCommand = new RelayCommand(_ => LoadAllImages())
            };

            if (Content is FrameworkElement frameworkElement)
            {
                frameworkElement.DataContext = _viewModel;
            }
        }

        private readonly MainWindowViewModel _viewModel;
        private readonly ObjectsResolver<UrlImageViewer> _imageViewersResolver = new();

        #region Управление

        public void LoadAllImages()
        {
            foreach (var imageViewer in _imageViewersResolver.Resolve())
            {
                imageViewer.LoadImage();
            }
        }

        #endregion

        #region События

        private void OnTotalProgressChanged(object sender, ValueChangedEventArgs<double> e)
        {
            _viewModel.TotalProgress = e.NewValue * 100;
        }

        #endregion
    }
}