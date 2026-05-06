using System.IO;

namespace ImageLoader.Extensions
{
    public static class StreamExtensions
    {
        extension(Stream stream)
        {
            public static async Task ReadWithProgressAsync(Stream source, long size, Stream destination, IProgress<double> progress, CancellationToken cancellationToken)
            {
                byte[] buffer = new byte[8192];
                long readBytes = 0;

                while (!cancellationToken.IsCancellationRequested)
                {
                    int bytesRead = await source.ReadAsync(buffer, cancellationToken);

                    if (bytesRead == 0)
                    {
                        break;
                    }

                    await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken);

                    readBytes += bytesRead;

                    progress.Report((double)readBytes / size);
                }
            }
        }
    }
}
