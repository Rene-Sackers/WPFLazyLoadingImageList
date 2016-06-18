using System;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace LazyLoading
{
    public class ImageDownloader
    {
        private readonly string _uri;

        private TaskCompletionSource<object> _downloadTaskCompletionSource;

        private ImageDownloader(string uri)
        {
            _uri = uri;
        }

        private async Task<BitmapImage> DownloadImageAsync()
        {
            var bitmapImage = new BitmapImage();

            _downloadTaskCompletionSource = new TaskCompletionSource<object>(null);

            bitmapImage.DownloadCompleted += BitmapImageOnDownloadCompleted;
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(_uri, UriKind.RelativeOrAbsolute);
            bitmapImage.CacheOption = BitmapCacheOption.None;
            bitmapImage.EndInit();

            if (bitmapImage.IsDownloading) await _downloadTaskCompletionSource.Task;
            if (bitmapImage.CanFreeze) bitmapImage.Freeze();

            return bitmapImage;
        }

        private void BitmapImageOnDownloadCompleted(object sender, EventArgs eventArgs)
        {
            _downloadTaskCompletionSource.TrySetResult(null);
        }

        public static Task<BitmapImage> DownloadImageAsync(string uri)
        {
            var imageDownloader = new ImageDownloader(uri);

            return imageDownloader.DownloadImageAsync();
        }
    }
}
