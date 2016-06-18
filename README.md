# WPFLazyLoadingImageList

Basically a test project for creating a lazy-loading `ListBox` of web images.

It uses a `ListBox` with a `UserControl` in it, which listents to the `IsVisibleChanged` event.
When this event triggers, and IsVisible is `true`, it creates a new `BitmapImage`, sets its `UriSource`.

Then, if `IsDownloading` is true on the `BitmapImage`, it awaits a `TaskCompletionSource` which gets set to completed when the `DownloadCompleted` event gets triggered on the `BitmapImage`.

This project was simply a test project. If you want to use this, you should probably generalize the implementation.
