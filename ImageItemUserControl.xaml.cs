using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace LazyLoading
{
    public partial class ImageItemUserControl
    {
        private const int MinimumTimeVisible = 2000;

        private ImageItem _imageItem;
        private Task _visibleTimerTask;
        private readonly CancellationTokenSource _visibleTimerTaskCancellationTokenSource = new CancellationTokenSource();
        private readonly Dispatcher _uiDispatcher = Dispatcher.CurrentDispatcher;

        public ImageItemUserControl()
        {
            InitializeComponent();

        }

        private void ImageItemUserControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            _imageItem = (ImageItem)DataContext;

            IsVisibleChanged += (s, e2) => OnIsVisibleChanged();
            OnIsVisibleChanged();
        }

        private void OnIsVisibleChanged()
        {
            if (_imageItem == null) return;

            if (IsVisible)
            {
                _visibleTimerTask = Task.Factory.StartNew(VisibleTimer, _visibleTimerTaskCancellationTokenSource.Token);
            }
            else
            {
                if (_visibleTimerTask != null)
                {
                    _visibleTimerTaskCancellationTokenSource.Cancel();
                }

                _imageItem.UnloadImage();
            }
        }

        private async void VisibleTimer()
        {
            try
            {
                await Task.Delay(MinimumTimeVisible, _visibleTimerTaskCancellationTokenSource.Token);
            }
            catch (TaskCanceledException)
            {
                return;
            }

            if (_visibleTimerTaskCancellationTokenSource.IsCancellationRequested) return;
            if (IsVisible) _uiDispatcher.Invoke(_imageItem.LoadImage);
        }
    }
}
