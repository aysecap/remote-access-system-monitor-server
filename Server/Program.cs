using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;
using System.Threading;

class Server
{
    static void Main(string[] args)
    {
        // Sunucu için IP ve port ayarlarını yapalım
        string ip = "127.0.0.1";
        int port = 8080;

        // Sunucu soketi oluşturuyoruz
        TcpListener server = new TcpListener(IPAddress.Parse(ip), port);
        server.Start();
        Console.WriteLine("Sunucu başlatıldı, istemciler bağlanıyor...");

        // İstemci bağlantısını bekliyoruz
        TcpClient client = server.AcceptTcpClient();
        Console.WriteLine("İstemci bağlandı!");

        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];

        // 5 saniyede bir CPU kullanım bilgisini göndericem
        while (true)
        {
            string cpuUsage = GetCpuUsage();
            buffer = Encoding.ASCII.GetBytes(cpuUsage);

            // Veriyi istemciye göndericem
            stream.Write(buffer, 0, buffer.Length);
            Console.WriteLine("Veri gönderildi: " + cpuUsage);

            Thread.Sleep(5000); // 5 saniye bekliyoruz
        }
    }

    // CPU kullanımını almak için basit bir fonksiyon
    static string GetCpuUsage()
    {
        // CPU kullanımını almak için performans sayacını kullanıyoruz
        var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        cpuCounter.NextValue(); // İlk okuma hemen doğru değeri vermez
        Thread.Sleep(1000); // Bir saniye bekleyip doğru sonucu alıyoruz
        float cpuValue = cpuCounter.NextValue();
        return $"CPU Kullanimi: {cpuValue}%";
    }
}


