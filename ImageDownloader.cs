using System;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace LazyLoading
{
    public class ImageDownloader
    {
        private readonly string _uri;

        private TaskCompletionSource<BitmapImage> _downloadTaskCompletionSource;

        private ImageDownloader(string uri)
        {
            _uri = uri;
        }

        private async Task<BitmapImage> DownloadImageAsync()
        {
            var bitmapImage = new BitmapImage();

            _downloadTaskCompletionSource = new TaskCompletionSource<BitmapImage>(bitmapImage);

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
            _downloadTaskCompletionSource.TrySetResult((BitmapImage)_downloadTaskCompletionSource.Task.AsyncState);
        }

        public static Task<BitmapImage> DownloadImageAsync(string uri)
        {
            var imageDownloader = new ImageDownloader(uri);

            return imageDownloader.DownloadImageAsync();
        }
    }
}
