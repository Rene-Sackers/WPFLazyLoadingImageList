using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using LazyLoading.Annotations;

namespace LazyLoading
{
    public partial class ImageItemUserControl : INotifyPropertyChanged
    {
        private const int MinimumTimeVisible = 2000;

        public bool IsInTimeout => _visibleTimerTask != null && !_visibleTimerTask.IsCanceled && _visibleTimerTask.IsCompleted == false;

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
                OnPropertyChanged(nameof(IsInTimeout));
            }
            else
            {
                if (_visibleTimerTask != null)
                {
                    _visibleTimerTaskCancellationTokenSource.Cancel();
                    OnPropertyChanged(nameof(IsInTimeout));
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
