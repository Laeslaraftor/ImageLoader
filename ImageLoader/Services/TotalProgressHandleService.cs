using ImageLoader.Base;

namespace ImageLoader.Services
{
    public class TotalProgressHandleService : ObservableObject
    {
        public event EventHandler<ValueChangedEventArgs<double>>? TotalProgressChanged;

        public int HandlersCount
        {
            get => field;
            set
            {
                if (field != value)
                {
                    field = value;
                    _progresses.Clear();
                    OnPropertyChanged(nameof(HandlersCount));
                }
            }
        }
        public double TotalProgress
        {
            get => field;
            private set
            {
                if (field != value)
                {
                    var oldValue = field;
                    field = value;
                    TotalProgressChanged?.Invoke(this, new(oldValue, value));
                    OnPropertyChanged(nameof(TotalProgress));
                }
            }
        }
        public IProgressHandler<double> ProgressHandler
        {
            get
            {
                field ??= new ObjectProgressHandler(this);
                return field;
            }
        }

        private readonly Dictionary<object, double> _progresses = [];

        #region Управление

        private void UpdateProgress()
        {
            var handlersCount = HandlersCount;

            if (handlersCount == 0)
            {
                TotalProgress = 0;
                return;
            }

            double sum = _progresses.Values.Sum();
            TotalProgress = Math.Min(Math.Max(sum / handlersCount, 0), 1);
        }

        #endregion

        #region Классы

        private class ObjectProgressHandler(TotalProgressHandleService service) : IProgressHandler<double>
        {
            private readonly TotalProgressHandleService _service = service;

            public void Report(object? sender, double value)
            {
                sender ??= this;

                if (!_service._progresses.TryAdd(sender, value))
                {
                    _service._progresses[sender] = value;
                }

                _service.UpdateProgress();
            }
        }

        #endregion
    }
}
