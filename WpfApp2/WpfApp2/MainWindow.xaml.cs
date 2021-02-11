using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HelixToolkit.Wpf;
using System.Windows.Media.Media3D;
using System.IO;
using System.IO.Ports;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;


namespace WpfApp2
{

    public partial class MainWindow : Window
    {
        #region Seriport - Dosya Tanımlamaları

        SerialPort sp;
        string port;
        int baudRate;
        int dataBit;
        Parity parityBit;
        StopBits stpBit;

        string dosya_yolu = "C:\\Users\\Uğurcan Soruç\\Desktop\\Flight_GCS\\Flight_GCS\\3.6.2019 Arayuz SON\\FullArayüz-Yedek\\TMUY2019_46701_TLM.csv";

        FileStream fs;
        StreamWriter sw;

        #endregion

        #region MainWindow

        public MainWindow()
        {
            InitializeComponent();

            this.Closing += (s, e) => (this.DataContext as IDisposable).Dispose();

            içeri_aktar.IsEnabled = false;


            Grid_Ust2.Visibility = Visibility.Visible;
            Grid_Charts.Visibility = Visibility.Visible;
            listview1.Visibility = Visibility.Hidden;
            mapView.Visibility = Visibility.Hidden;

            fs = new FileStream(dosya_yolu, FileMode.Create, FileAccess.Write);// file mode kontrol edilecek.
            sw = new StreamWriter(fs);

            stpBit = StopBits.One;

            combobox_Baudrate.Items.Add("19200");
            combobox_Databits.Items.Add("8");
            combobox_Parity.Items.Add("None");


            string[] portlar = SerialPort.GetPortNames();

            foreach (string portAdi in portlar)
            {
                combobox_Ports.Items.Add(portAdi);
            }

            load3dModel();
            startl1.Background = Brushes.Red;
            startl2.Background = Brushes.Red;
            startl3.Background = Brushes.Red;

            btn_Stop.IsEnabled = false;
            btn_Calibre.IsEnabled = false;

            sw.Write("Takim No,Paket Numarasi,Gonderme Zamani,Basinc,Yukseklik,Inis Hizi,Sicaklik,Pil Gerilimi,GPS Latitude,GPS Longitude,GPS Altitude,Uydu Statusu,Pitch,Roll,Yaw,Donus Sayisi \n");
        }

        #endregion

        #region 3D Model

        public void load3dModel()
        {
            ObjReader CurrentHelixObjReader = new ObjReader();
            Model3DGroup MyModel = CurrentHelixObjReader.Read(@"C:\Users\Uğurcan Soruç\Desktop\Flight_GCS\Flight_GCS\3.6.2019 Arayuz SON\FullArayüz-Yedek\uydu.obj");
            model.Content = MyModel;
            var hVp3D = new HelixViewport3D();
            var mode = new ModelVisual3D();
            hVp3D.Children.Add(mode);
        }

        #endregion

        #region Grafik - Harita - Liste - Kamera (Geçişler)

        private void Btn_GPS_Click(object sender, RoutedEventArgs e)
        {
            mapView.Visibility = Visibility.Visible;
            Grid_Charts.Visibility = Visibility.Hidden;
            listview1.Visibility = Visibility.Hidden;
            camera.Visibility = Visibility.Hidden;
            camera2.Visibility = Visibility.Visible;
            mapView1.Visibility = Visibility.Hidden;

            kameralabelsol.Visibility = Visibility.Hidden;
            kameralabelsag.Visibility = Visibility.Hidden;

            lbl1.Visibility = Visibility.Hidden;
            lbl2.Visibility = Visibility.Hidden;
            lbl3.Visibility = Visibility.Hidden;
            lbl4.Visibility = Visibility.Hidden;
            lbl5.Visibility = Visibility.Hidden;
            lbl6.Visibility = Visibility.Hidden;
            lbl7.Visibility = Visibility.Hidden;
            lbl8.Visibility = Visibility.Hidden;
            lbl9.Visibility = Visibility.Hidden;
            lbl10.Visibility = Visibility.Hidden;
            lbl11.Visibility = Visibility.Hidden;
            lbl12.Visibility = Visibility.Hidden;
        }

        private void Btn_Charts_Click(object sender, RoutedEventArgs e)
        {
            mapView.Visibility = Visibility.Hidden;
            Grid_Charts.Visibility = Visibility.Visible;
            listview1.Visibility = Visibility.Hidden;

            camera.Visibility = Visibility.Hidden;
            camera2.Visibility = Visibility.Visible;
            mapView1.Visibility = Visibility.Hidden;

            kameralabelsol.Visibility = Visibility.Hidden;
            kameralabelsag.Visibility = Visibility.Hidden;

            lbl1.Visibility = Visibility.Visible;
            lbl2.Visibility = Visibility.Visible;
            lbl3.Visibility = Visibility.Visible;
            lbl4.Visibility = Visibility.Visible;
            lbl5.Visibility = Visibility.Visible;
            lbl6.Visibility = Visibility.Visible;
            lbl7.Visibility = Visibility.Visible;
            lbl8.Visibility = Visibility.Visible;
            lbl9.Visibility = Visibility.Visible;
            lbl10.Visibility = Visibility.Visible;
            lbl11.Visibility = Visibility.Visible;
            lbl12.Visibility = Visibility.Visible;
        }


        private void Btn_ListView_Click(object sender, RoutedEventArgs e)
        {
            Grid_Charts.Visibility = Visibility.Hidden;
            listview1.Visibility = Visibility.Visible;
            mapView.Visibility = Visibility.Hidden;

            camera.Visibility = Visibility.Hidden;
            camera2.Visibility = Visibility.Visible;
            mapView1.Visibility = Visibility.Hidden;

            kameralabelsol.Visibility = Visibility.Hidden;
            kameralabelsag.Visibility = Visibility.Hidden;

        }
        private void Btn_camera_Click(object sender, RoutedEventArgs e)
        {
            camera.Visibility = Visibility.Visible;
            camera2.Visibility = Visibility.Hidden;
            Grid_Charts.Visibility = Visibility.Hidden;
            listview1.Visibility = Visibility.Hidden;
            mapView.Visibility = Visibility.Hidden;
            mapView1.Visibility = Visibility.Visible;

            kameralabelsol.Visibility = Visibility.Visible;
            kameralabelsag.Visibility = Visibility.Visible;

            lbl1.Visibility = Visibility.Hidden;
            lbl2.Visibility = Visibility.Hidden;
            lbl3.Visibility = Visibility.Hidden;
            lbl4.Visibility = Visibility.Hidden;
            lbl5.Visibility = Visibility.Hidden;
            lbl6.Visibility = Visibility.Hidden;
            lbl7.Visibility = Visibility.Hidden;
            lbl8.Visibility = Visibility.Hidden;
            lbl9.Visibility = Visibility.Hidden;
            lbl10.Visibility = Visibility.Hidden;
            lbl11.Visibility = Visibility.Hidden;
            lbl12.Visibility = Visibility.Hidden;
            
        }

        #endregion

        #region Başla - Bitir

        private void Btn_Start_Click(object sender, RoutedEventArgs e)
        {
            if (combobox_Ports.Text != "" && combobox_Baudrate.Text != "" && combobox_Databits.Text != "" && combobox_Parity.Text != "")
            {


                XBee_configure();
                UyduBar.Dispatcher.Invoke(() =>
                {
                    UyduBar.Value = 100;
                });
                startl1.Background = Brushes.LawnGreen;
                btn_Start.IsEnabled = false;
                btn_Stop.IsEnabled = true;

                if (calibra_ctrl == 0)
                {
                    btn_Calibre.IsEnabled = true;
                }
            }
            else
            {
                MessageBox.Show("XBee Ayarlarını kontrol ediniz !");
            }
        }

        private void Btn_Stop_Click(object sender, RoutedEventArgs e)
        {
            if (deneme_kontrol2 == 1)
            {
                sp.Close();
                startl1.Background = Brushes.Red;

                btn_Start.IsEnabled = true;
                btn_Stop.IsEnabled = false;
                btn_Calibre.IsEnabled = false;
            }
            UyduBar.Value = 700;
        }

        #endregion

        #region Yenile 

        private void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            combobox_Ports.Items.Clear();
            string[] portlar = SerialPort.GetPortNames();
            foreach (string portAdi in portlar)
            {
                combobox_Ports.Items.Add(portAdi);
            }
        }

        #endregion

        #region XBee ayarları

        private void XBee_configure()
        {
            port = combobox_Ports.SelectedItem.ToString();

            switch (combobox_Baudrate.SelectedIndex)
            {

                case 0:
                    baudRate = 19200;
                    break;
            }
            switch (combobox_Databits.SelectedIndex)
            {

                case 0:
                    dataBit = 8;
                    break;
            }

            switch (combobox_Parity.SelectedIndex)
            {
                case 0:
                    parityBit = Parity.None;
                    break;
            }
            sp = new SerialPort(port, baudRate, parityBit, dataBit, stpBit);
            sp.DataReceived += new SerialDataReceivedEventHandler(veri_AL);
            sp.Open();
        }

        #endregion

        #region Değişkenler

        string[] telemetri = new string[12000];
        string veri = "";

        string team_ID;         //0
        int packet_count = 0;   //1
        string mission_time;    //2
        double pressure = 0;     //3
        double altitude = 0;    //4
        double inishizi = 0;    //5
        double temperature = 0;  //6
        double voltage = 0;      //7
        double gps_latitude = 0; //8
        double gps_longitude = 0;//9
        double gps_altitude = 0; //10
        int software_state = 0; //11
        int pitch = 0;          //12
        int roll = 0;           //13
        int yaw = 0;            //14
        int blade_spin_rate = 0;  //15

        int pitch0 = 0;
        int roll0 = 0;
        char ayrac = ',';
        string[] ayrilmis_veriler;
        int sayac = 0;

        int deneme_kontrol = 0;
        int deneme_kontrol2 = 0;

        int deneme_kontrol3 = 0; // 3d için

        #endregion

        #region Seri Port veri okuma 

        private void veri_AL(object sender, SerialDataReceivedEventArgs e)
        {
            

            deneme_kontrol2 = 0;
            veri = sp.ReadLine();
            deneme_kontrol2 = 1;

            
            for (int i = 0; i < veri.Length; i++)
            {
                if (veri[i] == ',')
                {
                    deneme_kontrol++;
                }
            }
            if (deneme_kontrol == 15)
            {
                telemetri[sayac] = veri;
                sayac++;
                verileri_ayiklave_yazdir();

            }
            deneme_kontrol = 0;
            
        }

        #endregion

        #region Verilerin ayıklanıp yollandığı kısım

        string deneme = "";

        public void verileri_ayiklave_yazdir()
        {
            ayrilmis_veriler = veri.Split(ayrac);

            deneme += ayrilmis_veriler[0];
            deneme += ",";
            deneme += ayrilmis_veriler[1];
            deneme += ",";
            deneme += ayrilmis_veriler[2];
            deneme += ",";
            deneme += ayrilmis_veriler[3];
            deneme += ",";
            deneme += ayrilmis_veriler[4];
            deneme += ",";
            deneme += ayrilmis_veriler[5];
            deneme += ",";
            deneme += ayrilmis_veriler[6];
            deneme += ",";
            deneme += ayrilmis_veriler[7];
            deneme += ",";
            deneme += ayrilmis_veriler[8];
            deneme += ",";
            deneme += ayrilmis_veriler[9];
            deneme += ",";
            deneme += ayrilmis_veriler[10];
            deneme += ",";
            deneme += ayrilmis_veriler[11];
            deneme += ",";
            deneme += ayrilmis_veriler[12];
            deneme += ",";
            deneme += ayrilmis_veriler[13];
            deneme += ",";
            deneme += ayrilmis_veriler[14];
            deneme += ",";
            deneme += ayrilmis_veriler[15];


            /////////////////////////////////// burada deneme 
            if (deneme_kontrol3 == 0 && calibra_ctrl == 1) // kalibreden sonra veri basmasın diye
            {
                veri = null;
                deneme_kontrol3 = 1;
            }



            sw.Write(deneme);
            sw.Flush();

            deneme = "";

            try
            {
                team_ID = ayrilmis_veriler[0];
                lblTeam.Dispatcher.Invoke(() =>
                {
                    lblTeam.Content = team_ID;
                });
            }
            catch { }

            try
            {
                mission_time = ayrilmis_veriler[2];
                lblMissiontime.Dispatcher.Invoke(() =>
                {
                    lblMissiontime.Content = mission_time;
                });
            }
            catch { }

            try
            {
                packet_count = Convert.ToInt32(ayrilmis_veriler[1]);
                lblPacketcount.Dispatcher.Invoke(() =>
                {
                    lblPacketcount.Content = packet_count;
                });
            }
            catch { }

            

            try
            {
                altitude = double.Parse(string.Format("{0:0.#}", ayrilmis_veriler[4]));
                veri_Altitude.Dispatcher.Invoke(() =>
                {
                    if (calibra_ctrl != 1)
                    {
                        veri_Altitude.AddPoint(mission_time, altitude);
                    }
                    else
                    {
                        veri_Altitude2.AddPoint(mission_time, altitude);
                    }
                });

            }
            catch { }


            try
            {
                pressure = double.Parse(string.Format("{0:0.#}", ayrilmis_veriler[3]));
                veri_Pressure.Dispatcher.Invoke(() =>
                {
                    if (calibra_ctrl != 1)
                    {
                        veri_Pressure.AddPoint(mission_time, pressure);
                    }
                    else
                    {
                        veri_Pressure2.AddPoint(mission_time, pressure);
                    }

                });
            }
            catch { }


            try
            {
                temperature = double.Parse(string.Format("{0:0.#}", ayrilmis_veriler[6]));
                veri_Temperature.Dispatcher.Invoke(() =>
                {
                    if (calibra_ctrl != 1)
                    {
                        veri_Temperature.AddPoint(mission_time, temperature);
                    }
                    else
                    {
                        veri_Temperature2.AddPoint(mission_time, temperature);
                    }
                });
            }
            catch { }

            try
            {
                voltage = double.Parse(string.Format("{0:0.##}", ayrilmis_veriler[7]));
                veri_Voltage.Dispatcher.Invoke(() =>
                {
                    if (calibra_ctrl != 1)
                    {
                        veri_Voltage.AddPoint(mission_time, voltage);
                    }
                    else
                    {
                        veri_Voltage2.AddPoint(mission_time, voltage);
                    }
                });
            }
            catch { }


            try
            {
                gps_latitude = double.Parse(string.Format("{0:0.####}", ayrilmis_veriler[8]));
                gps_longitude = double.Parse(string.Format("{0:0.####}", ayrilmis_veriler[9]));

                // ana harita
                mapView.Dispatcher.Invoke(() =>
                {
                    marker = new GMap.NET.WindowsPresentation.GMapMarker(new GMap.NET.PointLatLng(gps_latitude, gps_longitude));
                    marker.Shape = new Ellipse
                    {
                        Width = 10,
                        Height = 10,
                        Stroke = Brushes.Red,
                        StrokeThickness = 2,
                        Visibility = Visibility.Visible,
                        Fill = Brushes.Yellow,

                    };
                    mapView.Zoom = 18;
                    mapView.Position = new GMap.NET.PointLatLng(gps_latitude, gps_longitude);
                    mapView.Markers.Clear();
                    mapView.Markers.Add(marker);
                });

                //yan harita

                mapView1.Dispatcher.Invoke(() =>
                {
                    marker = new GMap.NET.WindowsPresentation.GMapMarker(new GMap.NET.PointLatLng(gps_latitude, gps_longitude));
                    marker.Shape = new Ellipse
                    {
                        Width = 10,
                        Height = 10,
                        Stroke = Brushes.Red,
                        StrokeThickness = 2,
                        Visibility = Visibility.Visible,
                        Fill = Brushes.Yellow,

                    };
                    mapView1.Zoom = 18;
                    mapView1.Position = new GMap.NET.PointLatLng(gps_latitude, gps_longitude);
                    mapView1.Markers.Clear();
                    mapView1.Markers.Add(marker);
                });
            }
            catch { }

            try
            {
                gps_altitude = double.Parse(string.Format("{0:0.#}", ayrilmis_veriler[10]));
            }
            catch { }

            
            
                if (calibra_ctrl==1) //bu duruma göre değiştirilebilir drone ile atılacagı için
                {
                try
                {

                    pitch = Convert.ToInt32(ayrilmis_veriler[12]);
                    roll = Convert.ToInt32(ayrilmis_veriler[13]);

                    /*
                    if (deneme_kontrol3 == 0) // kalibrasyondan sonraki ilk veri sıkıntı yarattığı için
                    {
                        pitch = 0;
                        roll = 0;
                        deneme_kontrol3 = 1;
                    }
                    */


                    //if (roll < 65)
                    // {
                    this.Dispatcher.Invoke(() =>
                    {
                        var matrix = model.Transform.Value; //değer değiştirme için model e erişme
                        Vector3D vc3d1 = new Vector3D();

                        if (roll < 0)
                        {
                            roll += 360;
                        }

                        //sağsol--------
                        vc3d1.X = 0;
                        vc3d1.Y = 0;
                        vc3d1.Z = 1;
                        matrix.Rotate(new Quaternion(vc3d1, roll - roll0));
                        try
                        {
                            
                            lblroll.Dispatcher.Invoke(() =>
                            {
                                lblroll.Content = roll;
                            });
                        }
                        catch { }

                        model.Transform = new MatrixTransform3D(matrix);
                        //-----sağsol için

                        roll0 = roll;
                    });


                    this.Dispatcher.Invoke(() =>
                    {
                        var matrix = model.Transform.Value; //değer değiştirme için model e erişme
                        Vector3D vc3d2 = new Vector3D();

                        //yukariasaği-------
                        vc3d2.X = 1;
                        vc3d2.Y = 0;
                        vc3d2.Z = 0;
                        matrix.Rotate(new Quaternion(vc3d2, pitch - pitch0)); //buradada x y z ye açı değeri yollar. 
                        try
                        {

                            lblpitch.Dispatcher.Invoke(() =>
                            {
                                lblpitch.Content = pitch;
                            });
                        }
                        catch { }
                        model.Transform = new MatrixTransform3D(matrix);
                        //--------------yukariasagi

                        pitch0 = pitch;
                    });
                    
                }
                catch { }
                  }

            try
            {
                    yaw = Convert.ToInt32(ayrilmis_veriler[14]);
                    lblyaw.Dispatcher.Invoke(() =>
                    {
                        lblyaw.Content = yaw;
                    });
                }
                catch { }
          //  }

            

            try
            {
                blade_spin_rate = Convert.ToInt32(ayrilmis_veriler[15]);
                veri_BladeSpinRate.Dispatcher.Invoke(() =>
                {

                    if (calibra_ctrl != 1)
                    {
                        veri_BladeSpinRate.AddPoint(mission_time, blade_spin_rate);
                    }
                    else
                    {
                        veri_BladeSpinRate2.AddPoint(mission_time, blade_spin_rate);
                    }

                });
            }
            catch { }

            //Burasını Feyzullah düzeltirse silincek
            /////////////////////////////////////////////////////////////////////////////////////////////////
            if (packet_count != 0)
            {
                try
                { 
                    
                    software_state = Convert.ToInt32(ayrilmis_veriler[11]);
                    if (software_state == 1)
                    {
                        UyduBar.Dispatcher.Invoke(() =>
                        {
                            UyduBar.Value = 200;
                        });

                    }
                    else if (software_state == 2)
                    {
                        UyduBar.Dispatcher.Invoke(() =>
                        {
                            UyduBar.Value = 300;
                        });

                    }
                    else if (software_state == 3)
                    {
                        UyduBar.Dispatcher.Invoke(() =>
                        {
                            UyduBar.Value = 400;
                        });

                    }
                    else if (software_state == 4)
                    {
                        UyduBar.Dispatcher.Invoke(() =>
                        {
                            UyduBar.Value = 500;
                        });

                    }
                    if (software_state == 5)
                    {
                        UyduBar.Dispatcher.Invoke(() =>
                        {
                            UyduBar.Value = 600;
                        });

                    }
                    if(software_state == 6)
                    {
                        UyduBar.Dispatcher.Invoke(() =>
                        {
                            UyduBar.Value = 700;
                        });
                    }
                }
                catch { }
            }

            try
            {
                inishizi = double.Parse(string.Format("{0:0.#}", ayrilmis_veriler[5]));
                veri_inishizi.Dispatcher.Invoke(() =>
                {
                    if (calibra_ctrl != 1)
                    {
                        veri_inishizi.AddPoint(mission_time, inishizi);
                    }
                    else
                    {
                        veri_inishizi2.AddPoint(mission_time, inishizi);
                    }

                });
            }
            catch { }

            try
            {
                listview1.Dispatcher.Invoke(() =>
                {
                    try
                    {
                        listview1.Items.Add(new { TeamID = ayrilmis_veriler[0], PacketCount = ayrilmis_veriler[1], MissionTime = ayrilmis_veriler[2], Pressure = ayrilmis_veriler[3], Altitude = ayrilmis_veriler[4], InisHiz = ayrilmis_veriler[5], Temp = ayrilmis_veriler[6], Voltage = ayrilmis_veriler[7], GPSLatitude = ayrilmis_veriler[8], GPSLongitude = ayrilmis_veriler[9], GPSAltitude = ayrilmis_veriler[10], SoftwareState = ayrilmis_veriler[11], Pitch = ayrilmis_veriler[12], Roll = ayrilmis_veriler[13], Yaww = ayrilmis_veriler[14], BladeSpinRate = ayrilmis_veriler[15], });
                        listview1.SelectedIndex = listview1.Items.Count - 1;
                        listview1.ScrollIntoView(listview1.SelectedItem);
                    }
                    catch { }

                });
            }
            catch { }
            
            
        }

        #endregion

        #region Harita

        GMap.NET.WindowsPresentation.GMapMarker marker;
        private void mapView_Loaded(object sender, RoutedEventArgs e)
        {
            mapView.CacheLocation = @"C:\Users\Uğurcan Soruç\Desktop\Flight_GCS\Flight_GCS\3.6.2019 Arayuz SON\FullArayüz-Yedek\WpfApp2";
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            mapView.MapProvider = GMap.NET.MapProviders.GoogleSatelliteMapProvider.Instance;
            mapView.MaxZoom = 24;
            mapView.MinZoom = 6;
            mapView.CanDragMap = false;
            mapView.Position = new GMap.NET.PointLatLng(39.6413534, 32.8094251);
            mapView.ShowCenter = false;
            marker = new GMap.NET.WindowsPresentation.GMapMarker(new GMap.NET.PointLatLng(39.6413534, 32.8094251));

            marker.Shape = new Ellipse
            {
                Width = 10,
                Height = 10,
                Stroke = Brushes.Red,
                StrokeThickness = 2,
                Visibility = Visibility.Visible,
                Fill = Brushes.Yellow,

            };
            mapView.Markers.Add(marker);
            mapView.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            mapView.DragButton = MouseButton.Left;
            mapView.Zoom = 16;
        }

        private void mapView_Loaded1(object sender, RoutedEventArgs e)
        {
            mapView1.CacheLocation = @"C:\Users\Uğurcan Soruç\Desktop\Flight_GCS\Flight_GCS\3.6.2019 Arayuz SON\FullArayüz-Yedek\WpfApp2";
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            mapView1.MapProvider = GMap.NET.MapProviders.GoogleSatelliteMapProvider.Instance;
            mapView1.MaxZoom = 24;
            mapView1.MinZoom = 6;
            mapView1.CanDragMap = false;
            mapView1.Position = new GMap.NET.PointLatLng(39.6413534, 32.8094251);
            mapView1.ShowCenter = false;
            marker = new GMap.NET.WindowsPresentation.GMapMarker(new GMap.NET.PointLatLng(39.6413534, 32.8094251));

            marker.Shape = new Ellipse
            {
                Width = 10,
                Height = 10,
                Stroke = Brushes.Red,
                StrokeThickness = 2,
                Visibility = Visibility.Visible,
                Fill = Brushes.Yellow,

            };
            mapView1.Markers.Add(marker);
            mapView1.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            mapView1.DragButton = MouseButton.Left;
            mapView1.Zoom = 16;
        }

        #endregion

        #region Kalibrasyon

        int calibra_ctrl = 0;
        int tik_ctrl = 0;


        private void Btn_Calibre_Click(object sender, RoutedEventArgs e)
        {

            //SONRADAN EKLENDI
            ///////////////////////////////////////////////////////////////////////////////
            veri_Altitude.Points.Clear();
            veri_BladeSpinRate.Points.Clear();
            veri_inishizi.Points.Clear();
            veri_Pressure.Points.Clear();
            veri_Temperature.Points.Clear();
            veri_Voltage.Points.Clear();
            ////////////////////////////////////////////////////////////////////////////////////

            listview1.Items.Clear();

            //////////////////////////////////////////////////////////////////////

            calibra_ctrl = 1;
            sp.Write("1");
            tik_ctrl++;
            if (tik_ctrl % 2 == 0)
            {
                startl2.Background = Brushes.Red;
                startl3.Background = Brushes.LawnGreen;
            }
            else
            {
                startl2.Background = Brushes.LawnGreen;
                startl3.Background = Brushes.Red;
            }

            UyduBar.Dispatcher.Invoke(() => 
            {
                UyduBar.Value = 0;
            });


        }

        #endregion

        #region Telekomut, Ayrilma ve Yükseklik
        string deneme123=null;
        private void Telekomut_Gönder_Click(object sender, RoutedEventArgs e)
        {
            // sp.Write("4"+telekomuttextbox.Text);
            deneme123 ="4" +telekomuttextbox.Text;

            //türkçe karakter sorunu 

            deneme123 = deneme123.Replace("ş", "s");
            deneme123 = deneme123.Replace("Ş", "S");
            deneme123 = deneme123.Replace("İ", "I");
            deneme123 = deneme123.Replace("ı", "i");
            deneme123 = deneme123.Replace("Ğ", "G");
            deneme123 = deneme123.Replace("ğ", "g");
            deneme123 = deneme123.Replace("Ç", "C");
            deneme123 = deneme123.Replace("ç", "c");
            deneme123 = deneme123.Replace("Ü", "U");
            deneme123 = deneme123.Replace("ü", "u");
            deneme123 = deneme123.Replace("Ö", "O");
            deneme123 = deneme123.Replace("ö", "o");


            sp.Write(deneme123);

            telekomutlbl.Background = Brushes.LawnGreen;
        }
        private void ayir_click(object sender, RoutedEventArgs e)
        {
            sp.Write("2");
            ayir_renk.Background = Brushes.LawnGreen;
        }

        private void Yukseklik_Button_Click(object sender, RoutedEventArgs e)
        {
            sp.Write("3" + Yukseklik_Textbox.Text);
            Yukseklik_Renk.Background = Brushes.LawnGreen;
        }

        #endregion

        #region  Excel ile ilgili kodlar

        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        List<string> listA = new List<string>();

        public string filename;
        public int x = 1;
        int y = 16;

        private void Excel_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".csv"; // Default file extension
            dlg.Filter = "Text documents (.csv)|*.csv"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                filename = dlg.FileName;
            }

            if (filename != null)
            {
                içeri_aktar.IsEnabled = true;
                excel.IsEnabled = false;
                excel_renk.Background = Brushes.LawnGreen;
            }
        }

        private void İçeri_Aktar_Click(object sender, RoutedEventArgs e)
        {
            içeri_aktar.IsEnabled = false;
            içeri_aktar_renk.Background = Brushes.LawnGreen;

            var reader = new StreamReader(File.OpenRead(filename));
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                listA.Add(values[0]);
                listA.Add(values[1]);
                listA.Add(values[2]);
                listA.Add(values[3]);
                listA.Add(values[4]);
                listA.Add(values[5]);
                listA.Add(values[6]);
                listA.Add(values[7]);
                listA.Add(values[8]);
                listA.Add(values[9]);
                listA.Add(values[10]);
                listA.Add(values[11]);
                listA.Add(values[12]);
                listA.Add(values[13]);
                listA.Add(values[14]);
                listA.Add(values[15]);
            }

            reader.Close();
            test();
        }

        private void test()
        {
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0,(1000/x));
            try
            {
                x = Convert.ToInt32(textbox1.Text);
                try
                {
                    listview1.Dispatcher.Invoke(() =>
                    {
                        listview1.Items.Add(new { TeamID = listA[y], PacketCount = listA[y + 1], MissionTime = listA[y + 2], Pressure = listA[y + 3], Altitude = listA[y + 4], InisHiz = listA[y + 5], Temp = listA[y + 6], Voltage = listA[y + 7], GPSLatitude = listA[y + 8], GPSLongitude = listA[y + 9], GPSAltitude = listA[y + 10], SoftwareState = listA[y + 11], Pitch = listA[y + 12], Roll = listA[y + 13], Yaww = listA[y + 14], BladeSpinRate = listA[y + 15], });
                        listview1.SelectedIndex = listview1.Items.Count - 1;
                        listview1.ScrollIntoView(listview1.SelectedItem);
                    });

                    lblMissiontime.Content = listA[y + 2];
                    lblTeam.Content = listA[y];
                    lblPacketcount.Content = listA[y + 1];
                }
                catch { }
                ////////////////////////////////////////////////////////////////////////////////////////////////////

                try
                {
                    veri_inishizi.Dispatcher.Invoke(() =>
                    {
                        veri_inishizi.AddPoint(listA[y + 2], double.Parse(string.Format("{0:0.#}", listA[y + 5])));
                    });
                }
                catch { }

                try
                {
                    veri_Altitude.Dispatcher.Invoke(() =>
                    {
                        veri_Altitude.AddPoint(listA[y + 2], double.Parse(string.Format("{0:0.#}", listA[y + 4])));
                    });
                }
                catch { }

                try
                {
                    veri_Pressure.Dispatcher.Invoke(() =>
                    {
                        veri_Pressure.AddPoint(listA[y + 2], double.Parse(string.Format("{0:0.#}", listA[y + 3])));
                    });
                }
                catch { }

                try
                {
                    veri_Temperature.Dispatcher.Invoke(() =>
                    {
                        veri_Temperature.AddPoint(listA[y + 2], double.Parse(string.Format("{0:0.#}", listA[y + 6])));
                    });
                }
                catch { }

                try
                {
                    veri_Voltage.Dispatcher.Invoke(() =>
                    {
                        veri_Voltage.AddPoint(listA[y + 2], double.Parse(string.Format("{0:0.#}", listA[y + 7])));
                    });
                }
                catch { }

                try
                {
                    veri_BladeSpinRate.Dispatcher.Invoke(() =>
                    {
                        veri_BladeSpinRate.AddPoint(listA[y + 2], double.Parse(string.Format("{0:0.#}", listA[y + 15])));
                    });
                }
                catch { }

                try
                {

                    software_state = Convert.ToInt32(listA[y + 11]);
                    if (software_state == 1)
                    {
                        UyduBar.Dispatcher.Invoke(() =>
                        {
                            UyduBar.Value = 200;
                        });

                    }
                    else if (software_state == 2)
                    {
                        UyduBar.Dispatcher.Invoke(() =>
                        {
                            UyduBar.Value = 300;
                        });

                    }
                    else if (software_state == 3)
                    {
                        UyduBar.Dispatcher.Invoke(() =>
                        {
                            UyduBar.Value = 400;
                        });

                    }
                    else if (software_state == 4)
                    {
                        UyduBar.Dispatcher.Invoke(() =>
                        {
                            UyduBar.Value = 500;
                        });

                    }
                    if (software_state == 5)
                    {
                        UyduBar.Dispatcher.Invoke(() =>
                        {
                            UyduBar.Value = 600;
                        });

                    }
                    if (software_state == 6)
                    {
                        UyduBar.Dispatcher.Invoke(() =>
                        {
                            UyduBar.Value = 700;
                        });
                    }
                }
                catch { }


                try
                {
                    gps_latitude = double.Parse(string.Format("{0:0.####}", listA[y + 8]));
                    gps_longitude = double.Parse(string.Format("{0:0.####}", listA[y + 9]));

                    // ana harita
                    mapView.Dispatcher.Invoke(() =>
                    {
                        marker = new GMap.NET.WindowsPresentation.GMapMarker(new GMap.NET.PointLatLng(gps_latitude, gps_longitude));
                        marker.Shape = new Ellipse
                        {
                            Width = 10,
                            Height = 10,
                            Stroke = Brushes.Red,
                            StrokeThickness = 2,
                            Visibility = Visibility.Visible,
                            Fill = Brushes.Yellow,

                        };
                        mapView.Zoom = 18;
                        mapView.Position = new GMap.NET.PointLatLng(gps_latitude, gps_longitude);
                        mapView.Markers.Clear();
                        mapView.Markers.Add(marker);
                    });

                    //yan harita

                    mapView1.Dispatcher.Invoke(() =>
                    {
                        marker = new GMap.NET.WindowsPresentation.GMapMarker(new GMap.NET.PointLatLng(gps_latitude, gps_longitude));
                        marker.Shape = new Ellipse
                        {
                            Width = 10,
                            Height = 10,
                            Stroke = Brushes.Red,
                            StrokeThickness = 2,
                            Visibility = Visibility.Visible,
                            Fill = Brushes.Yellow,

                        };
                        mapView1.Zoom = 18;
                        mapView1.Position = new GMap.NET.PointLatLng(gps_latitude, gps_longitude);
                        mapView1.Markers.Clear();
                        mapView1.Markers.Add(marker);
                    });
                }
                catch { }

                if (Convert.ToInt32(listA[y + 11]) == 4) //bu duruma göre değiştirilebilir drone ile atılacagı için
                {
                    try
                    {

                        pitch = Convert.ToInt32(listA[y + 12]);
                        roll = Convert.ToInt32(listA[y + 13]);

                        //if (roll < 65)
                        // {
                        this.Dispatcher.Invoke(() =>
                        {
                            var matrix = model.Transform.Value; //değer değiştirme için model e erişme
                        Vector3D vc3d1 = new Vector3D();

                            if (roll < 0)
                            {
                                roll += 360;
                            }

                        //sağsol--------
                        vc3d1.X = 0;
                            vc3d1.Y = 0;
                            vc3d1.Z = 1;
                            matrix.Rotate(new Quaternion(vc3d1, roll - roll0));
                            try
                            {

                                lblroll.Dispatcher.Invoke(() =>
                                {
                                    lblroll.Content = roll;
                                });
                            }
                            catch { }

                            model.Transform = new MatrixTransform3D(matrix);
                        //-----sağsol için

                        roll0 = roll;
                        });


                        this.Dispatcher.Invoke(() =>
                        {
                            var matrix = model.Transform.Value; //değer değiştirme için model e erişme
                        Vector3D vc3d2 = new Vector3D();

                        //yukariasaği-------
                        vc3d2.X = 1;
                            vc3d2.Y = 0;
                            vc3d2.Z = 0;
                            matrix.Rotate(new Quaternion(vc3d2, pitch - pitch0)); //buradada x y z ye açı değeri yollar. 
                        try
                            {

                                lblpitch.Dispatcher.Invoke(() =>
                                {
                                    lblpitch.Content = pitch;
                                });
                            }
                            catch { }
                            model.Transform = new MatrixTransform3D(matrix);
                        //--------------yukariasagi

                        pitch0 = pitch;
                        });
                        //  }
                    }

                    catch { }

                    try
                    {
                        yaw = Convert.ToInt32(listA[y + 14]);
                        lblyaw.Dispatcher.Invoke(() =>
                        {
                            lblyaw.Content = yaw;
                        });
                    }
                    catch { }
                }


                y += 16;
            }
            catch { }
        }

        #endregion

        
    }
}
