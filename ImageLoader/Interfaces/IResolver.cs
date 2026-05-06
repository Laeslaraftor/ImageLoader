namespace ImageLoader
{
    public interface IResolver
    {
        public event EventHandler<ResolveEventArgs>? ResolveRequested;
    }
}
