using System.IO;
using System.Windows.Media.Imaging;

namespace ImageLoader.Extensions
{
    public static class BitmapExtensions
    {
        extension(BitmapImage bitmap)
        {
            public static BitmapImage Create(Stream stream)
            {
                BitmapImage result = new();
                result.BeginInit();
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();

                return result!;
            }
        }
    }
}
