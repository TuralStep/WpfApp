using System;
using System.Net.Sockets;
using System.Net;
using System.Windows;
using System.Drawing;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var client = new Socket(AddressFamily.InterNetwork,
                          SocketType.Dgram,
                          ProtocolType.Udp);

            var ip = IPAddress.Parse("127.0.0.1");
            var port = 45678;

            var ep = new IPEndPoint(ip, port);

            System.Drawing.Image img;
            var buffer = Array.Empty<byte>();


            while (true)
            {
                using var bitmap = new Bitmap(1280, 720);
                using (var g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(0, 0, 0, 0,
                        bitmap.Size, CopyPixelOperation.SourceCopy);
                }

                img = (System.Drawing.Image)bitmap;
                MemoryStream memoS;

                using (memoS = new MemoryStream())
                {
                    img.Save(memoS, img.RawFormat);
                    memoS.ToArray();
                }

                buffer = memoS.GetBuffer();
                client.SendTo(buffer, ep);
            }



        }

    }
}
