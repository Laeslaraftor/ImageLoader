namespace ImageLoader
{
    public interface IProgressHandler<T>
    {
        public void Report(object? sender, T value);
    }
}
