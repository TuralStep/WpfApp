using System.Diagnostics.Metrics;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using static System.Net.Mime.MediaTypeNames;


var listener = new Socket(AddressFamily.InterNetwork,
                                      SocketType.Dgram,
                                      ProtocolType.Udp);

var ip = IPAddress.Parse("127.0.0.1");
var port = 45678;

var ep = new IPEndPoint(ip, port);

listener.Bind(ep);

var buffer = new byte[ushort.MaxValue - 29];
EndPoint remoteEp = new IPEndPoint(IPAddress.Any, 0);

var count = 0;
string msg = string.Empty;

MemoryStream ms;
System.Drawing.Image img;

while (true)
{

    count = listener.ReceiveFrom(buffer, ref remoteEp);
    msg = Encoding.Default.GetString(buffer, 0, count);
    if (!msg.Equals("send"))
        continue;

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

}