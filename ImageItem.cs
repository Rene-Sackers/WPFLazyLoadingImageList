using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Media;
using LazyLoading.Annotations;

namespace LazyLoading
{
    public class ImageItem : INotifyPropertyChanged
    {
        private string _imageUrl;
        private bool _isLoading;
        private ImageSource _imageSource;

        public string ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                if (Equals(value, _imageUrl)) return;

                _imageUrl = value;
                OnPropertyChanged();

            }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            private set
            {
                if (Equals(value, _isLoading)) return;

                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoaded => ImageSource != null;

        public ImageSource ImageSource
        {
            get
            {
                return _imageSource;
            }
            set
            {
                if (Equals(value, _imageSource)) return;

                _imageSource = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsLoaded));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void LoadImage()
        {
            if (IsLoading || _imageSource != null) return;

            Debug.WriteLine("Load image");

            IsLoading = true;

            var uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            ImageDownloader
                .DownloadImageAsync(ImageUrl)
                .ContinueWith(task =>
            {
                ImageSource = task.Result;
                IsLoading = false;
            }, uiScheduler);
        }

        public void UnloadImage()
        {
            ImageSource = null;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
