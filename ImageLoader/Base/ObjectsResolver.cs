namespace ImageLoader.Base
{
    public class ObjectsResolver<T> : IResolver
    {
        public event EventHandler<ResolveEventArgs>? ResolveRequested;

        public IEnumerable<T> Resolve()
        {
            ResolveEventArgs args = new();

            ResolveRequested?.Invoke(this, args);

            return args.Values.Where(obj => obj is T).Cast<T>();
        }
    }
}
