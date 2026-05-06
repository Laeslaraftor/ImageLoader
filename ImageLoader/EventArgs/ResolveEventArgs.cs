namespace ImageLoader
{
    public class ResolveEventArgs : EventArgs
    {
        public HashSet<object> Values { get; } = [];
    }
}
