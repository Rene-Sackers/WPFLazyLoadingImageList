using System;
using System.Windows;

namespace LazyLoading
{
    public partial class ImageItemUserControl
    {
        private ImageItem ImageItem => DataContext as ImageItem;

        public ImageItemUserControl()
        {
            InitializeComponent();

            IsVisibleChanged += OnIsVisibleChanged;
        }

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (ImageItem == null) return;

            if (IsVisible) ImageItem.LoadImage();
            else ImageItem.UnloadImage();
        }
    }
}
