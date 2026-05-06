using ImageLoader.Extensions;
using System.IO;
using System.Net.Http;
using System.Windows.Media.Imaging;

namespace ImageLoader.Services
{
    public class ImageDownloadService : IDisposable
    {
        public ImageDownloadService()
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new("image/*"));
        }
        ~ImageDownloadService()
        {
            Dispose();
        }

        private readonly HttpClient _httpClient = new();

        public async Task<BitmapImage?> Download(Uri imageUri, IProgress<double> progress, CancellationToken cancellationToken)
        {
            progress.Report(0);

            try
            {
                using var response = await _httpClient.GetAsync(imageUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

                response.EnsureSuccessStatusCode();

                long totalBytes = response.Content.Headers.ContentLength ?? -1;
                using Stream contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                MemoryStream memoryStream = new();

                if (totalBytes == -1)
                {
                    await contentStream.CopyToAsync(memoryStream, cancellationToken);
                    progress.Report(1);
                }
                else
                {
                    await Stream.ReadWithProgressAsync(contentStream, totalBytes, memoryStream, progress, cancellationToken);
                }

                memoryStream.Position = 0;

                return BitmapImage.Create(memoryStream);
            }
            catch (TaskCanceledException)
            {
                return null;
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
