namespace ImageLoader
{
    public readonly struct ValueChangedEventArgs<T>(T oldValue, T newValue)
    {
        public T OldValue { get; } = oldValue;
        public T NewValue { get; } = newValue;
    }
}
