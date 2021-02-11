using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Accord.Video.FFMPEG;
using AForge.Video;
using AForge.Video.DirectShow;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;

namespace VideoRecorder
{
    internal class MainWindowViewModel : ObservableObject, IDisposable
    {
        #region Private fields

        private FilterInfo _currentDevice;

        private BitmapImage _image;
        private string _ipCameraUrl;

        private bool _isDesktopSource;
        private bool _isIpCameraSource;
        private bool _isWebcamSource;

        private IVideoSource _videoSource;
        private VideoFileWriter _writer;
        private bool _recording;
        private DateTime? _firstFrameTime;

        #endregion

        #region Constructor

        public MainWindowViewModel()
        {
            VideoDevices = new ObservableCollection<FilterInfo>();
            GetVideoDevices();
            IsDesktopSource = true;
            StartSourceCommand = new RelayCommand(StartCamera);
            StopSourceCommand = new RelayCommand(StopCamera);
           // StartRecordingCommand = new RelayCommand(StartRecording);
          //  StopRecordingCommand = new RelayCommand(StopRecording);
           // SaveSnapshotCommand = new RelayCommand(SaveSnapshot);
            IpCameraUrl = "http://88.53.197.250/axis-cgi/mjpg/video.cgi?resolution=320×240";
        }

        #endregion

        #region Properties

        public ObservableCollection<FilterInfo> VideoDevices { get; set; }

        public BitmapImage Image
        {
            get { return _image; }
            set { Set(ref _image, value); }
        }

        public bool IsDesktopSource
        {
            get { return _isDesktopSource; }
            set { Set(ref _isDesktopSource, value); }
        }

        public bool IsWebcamSource
        {
            get { return _isWebcamSource; }
            set { Set(ref _isWebcamSource, value); }
        }

        public bool IsIpCameraSource
        {
            get { return _isIpCameraSource; }
            set { Set(ref _isIpCameraSource, value); }
        }

        public string IpCameraUrl
        {
            get { return _ipCameraUrl; }
            set { Set(ref _ipCameraUrl, value); }
        }

        public FilterInfo CurrentDevice
        {
            get { return _currentDevice; }
            set { Set(ref _currentDevice, value); }
        }

        public ICommand StartRecordingCommand { get; private set; }

        public ICommand StopRecordingCommand { get; private set; }

        public ICommand StartSourceCommand { get; private set; }

        public ICommand StopSourceCommand { get; private set; }

        public ICommand SaveSnapshotCommand { get; private set; }

        #endregion

        private void GetVideoDevices()
        {
            var devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo device in devices)
            {
                VideoDevices.Add(device);
            }
            if (VideoDevices.Any())
            {
                CurrentDevice = VideoDevices[0];
            }
            else
            {
                MessageBox.Show("No webcam found");
            }
        }

        private void StartCamera()
        {


            if (CurrentDevice != null)
                {
                    _videoSource = new VideoCaptureDevice(CurrentDevice.MonikerString);
                    _videoSource.NewFrame += video_NewFrame;
                    _videoSource.Start();
                }
                else
                {
                    MessageBox.Show("Current device can't be null");
                }

             System.Threading.Thread.Sleep(2000);

            new Thread(StartRecording).Start();

           
        }

        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {

                if (_recording)
                {
                    if (_firstFrameTime != null)
                    {
                        _writer.WriteVideoFrame(eventArgs.Frame, DateTime.Now - _firstFrameTime.Value);
                    }
                    else
                    {
                        _writer.WriteVideoFrame(eventArgs.Frame);
                        _firstFrameTime = DateTime.Now;
                    }
                }


               
                System.Windows.Threading.Dispatcher.CurrentDispatcher.Invoke((Action)(() =>
                {
                    using (var bitmap = (Bitmap)eventArgs.Frame.Clone())
                    {
                       // Image = bitmap.ToBitmapImage();
                        Application.Current.Dispatcher.Invoke(new Action(() => Image = bitmap.ToBitmapImage()));

                    }
                    Application.Current.Dispatcher.Invoke(new Action(() => Image.Freeze()));

                   // Image.Freeze(); // avoid cross thread operations and prevents leaks
                }));



            }
            catch (Exception)
            {

            }
        }

        private void StopCamera()
        {
            _recording = false;
            _writer.Close();
            _writer.Dispose();

            if (_videoSource != null && _videoSource.IsRunning)
            {
                _videoSource.SignalToStop();
                _videoSource.NewFrame -= video_NewFrame;
            }
            Image = null;
            
        }
        /*
        private void StopRecording()
        {
            _recording = false;
            _writer.Close();
            _writer.Dispose();
        }
        */
        
        private void StartRecording()
        {
              //System.Threading.Thread.Sleep(2000);

            System.Windows.Threading.Dispatcher.CurrentDispatcher.Invoke((Action)(() =>
            {
                Application.Current.Dispatcher.Invoke(new Action(() => _firstFrameTime = null));
                Application.Current.Dispatcher.Invoke(new Action(() => _writer = new VideoFileWriter()));

                Application.Current.Dispatcher.Invoke(new Action(() => _writer.Open(@"C:\Users\Uğurcan Soruç\Desktop\Flight_GCS\Flight_GCS\3.6.2019 Arayuz SON\FullArayüz-Yedek\TMUY2019_46701_VIDEO.mp4", (int)Math.Round(Image.Width, 0), (int)Math.Round(Image.Height, 0), 30, VideoCodec.MPEG4, 100000000)));

                Application.Current.Dispatcher.Invoke(new Action(() => _recording = true));
            /*
                _firstFrameTime = null;
                _writer = new VideoFileWriter();

                _writer.Open(@"C:\Users\Uğurcan Soruç\Desktop\Flight_GCS\Flight_GCS\3.6.2019 Arayuz SON\FullArayüz-Yedek\test.avi", (int)Math.Round(Image.Width, 0), (int)Math.Round(Image.Height, 0), 30, VideoCodec.MPEG4, 100000000);

                _recording = true;*/
            }));
            
        }
        
        /*
        private void SaveSnapshot()
        {
            var dialog = new SaveFileDialog();
            dialog.FileName = "Snapshot1";
            dialog.DefaultExt = ".png";
            var dialogresult = dialog.ShowDialog();
            if (dialogresult != true)
            {
                return;
            }
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(Image));
            using (var filestream = new FileStream(dialog.FileName, FileMode.Create))
            {
                encoder.Save(filestream);
            }
        }
        */
        public void Dispose()
        {
            if (_videoSource != null && _videoSource.IsRunning)
            {
                _videoSource.SignalToStop();
            }
            _writer?.Dispose();
        }
    }
}
