using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LazyLoading.Annotations;

namespace LazyLoading
{
    public partial class MainWindow : INotifyPropertyChanged
    {
        private string _ramUsage = "";

        public ObservableCollection<ImageItem> ImageItems { get; } = new ObservableCollection<ImageItem>();

        public string RamUsage
        {
            get { return _ramUsage; }
            private set
            {
                if (Equals(value, _ramUsage)) return;

                _ramUsage = value;
                OnPropertyChanged();
            }
        }

        public MainWindow()
        {
            const int imageCount = 1000;

            var imgurImages = new[]
            {
                "http://i.imgur.com/rvPruL7.jpg",
                "http://i.imgur.com/g6iFHEl.jpg",
                "http://i.imgur.com/Kk0huRc.jpg",
                "http://i.imgur.com/vQRu3aw.jpg",
                "http://i.imgur.com/qi8dRKL.jpg",
                "http://i.imgur.com/Edtosw8.jpg",
                "http://i.imgur.com/WqLubmW.jpg",
                "http://i.imgur.com/z1rxmoY.jpg",
                "http://i.imgur.com/eRREs1p.jpg",
                "http://i.imgur.com/ZoFb1od.jpg",
                "http://i.imgur.com/vq0kIHm.jpg",
                "http://i.imgur.com/DEDRhZE.jpg",
                "http://i.imgur.com/y4V9BSq.jpg",
                "http://i.imgur.com/hJyymXl.jpg",
                "http://i.imgur.com/jWygvBP.jpg",
                "http://i.imgur.com/mWkOKqu.jpg",
                "http://i.imgur.com/tVCFb2W.jpg",
                "http://i.imgur.com/4fOFjeU.jpg",
                "http://i.imgur.com/bCp5xaN.jpg",
                "http://i.imgur.com/hO9EnHl.jpg",
                "http://i.imgur.com/ajHX6Xz.jpg",
                "http://i.imgur.com/3bPLJfy.jpg",
                "http://i.imgur.com/Y4ZK5zN.jpg",
                "http://i.imgur.com/bIbAYKL.jpg",
                "http://i.imgur.com/GqtqMpB.jpg",
                "http://i.imgur.com/NOVK2Yh.jpg",
                "http://i.imgur.com/U8Z97Kp.jpg",
                "http://i.imgur.com/1KCvWBP.jpg",
                "http://i.imgur.com/ufuccv5.jpg",
                "http://i.imgur.com/3VHcpXC.jpg",
                "http://i.imgur.com/iwl2Wih.jpg",
                "http://i.imgur.com/MtPXiLN.jpg",
                "http://i.imgur.com/HIOjLCd.jpg",
                "http://i.imgur.com/d3a3Qxb.jpg",
                "http://i.imgur.com/xS1SFFa.jpg",
                "http://i.imgur.com/1oMbXIr.jpg",
                "http://i.imgur.com/eGJRS2x.jpg",
                "http://i.imgur.com/eyqwOoV.jpg",
                "http://i.imgur.com/inZmSVU.jpg",
                "http://i.imgur.com/cmBzyXh.jpg",
                "http://i.imgur.com/Obs8OK4.jpg",
                "http://i.imgur.com/L7WXS5r.jpg",
                "http://i.imgur.com/XY36PJN.jpg",
                "http://i.imgur.com/7zZr65t.jpg",
                "http://i.imgur.com/VqkR9yM.jpg",
                "http://i.imgur.com/VX5gdUz.jpg",
                "http://i.imgur.com/WbdagVE.jpg",
                "http://i.imgur.com/ZoBJc3l.jpg",
                "http://i.imgur.com/mtTiWD3.jpg",
                "http://i.imgur.com/TJ9wB5L.jpg",
                "http://i.imgur.com/PkhuMfr.jpg",
                "http://i.imgur.com/KVk9FUj.jpg",
                "http://i.imgur.com/wa5pMkp.jpg",
                "http://i.imgur.com/wQS3NqT.jpg",
                "http://i.imgur.com/S3USBBF.jpg",
                "http://i.imgur.com/dW7oP1O.jpg",
                "http://i.imgur.com/mIOZrde.jpg",
                "http://i.imgur.com/Zqlt2D7.jpg",
                "http://i.imgur.com/LkRGYjf.jpg",
                "http://i.imgur.com/zE6L9vN.jpg"
            };

            var random = new Random();
            for (var i = 0; i < imageCount; i++)
            {
                ImageItems.Add(new ImageItem
                {
                    ImageUrl = imgurImages[random.Next(0, imgurImages.Length - 1)]
                });
            }

            InitializeComponent();

            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    var currentProcess = Process.GetCurrentProcess();
                    RamUsage = "In use: " + ToMbString(currentProcess.WorkingSet64) + ", Reserved: " + ToMbString(currentProcess.PrivateMemorySize64);
                    await Task.Delay(1000);
                }
            });
        }

        private string ToMbString(decimal bytes)
        {
            return Math.Round(bytes / 1024 / 1024, 2).ToString(CultureInfo.InvariantCulture) + "MB";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
