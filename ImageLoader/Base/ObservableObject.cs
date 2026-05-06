using System.ComponentModel;

namespace ImageLoader.Base
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        #region События

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (!_propertyChangedEventArgs.TryGetValue(propertyName, out var args))
            {
                args = new(propertyName);
                _propertyChangedEventArgs.Add(propertyName, args);
            }

            OnPropertyChanged(args);
        }
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        #endregion

        #region Статика

        private static readonly Dictionary<string, PropertyChangedEventArgs> _propertyChangedEventArgs = [];

        #endregion
    }
}
