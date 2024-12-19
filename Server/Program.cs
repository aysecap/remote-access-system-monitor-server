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
        // Sunucu için IP ve port ayarları
        string ip = "127.0.0.1";
        int port = 8080;

        // Sunucu soketi oluşturma
        TcpListener server = new TcpListener(IPAddress.Parse(ip), port);
        server.Start();
        Console.WriteLine("Sunucu çalışıyor, istemciler bekleniyor...");

        // İstemci bağlantısını kabul et
        TcpClient client = server.AcceptTcpClient();
        Console.WriteLine("Bir istemci bağlandı.");

        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];

        // Sistem verilerini her 5 saniyede bir gönder
        while (true)
        {
            string cpuUsage = GetCpuUsage();
            buffer = Encoding.ASCII.GetBytes(cpuUsage);

            // İstemciye veriyi gönder
            stream.Write(buffer, 0, buffer.Length);
            Console.WriteLine("İstemciye veri gönderildi: " + cpuUsage);

            Thread.Sleep(5000); // 5 saniye bekle
        }
    }

    // CPU kullanım oranını almak için bir yöntem
    static string GetCpuUsage()
    {
        // CPU kullanımı bilgisini al
        var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        cpuCounter.NextValue(); // İlk değer hemen çıkmayabilir, ikinci okuma daha doğru olur.
        Thread.Sleep(1000); // 1 saniye bekleyerek doğru veriyi al
        float cpuValue = cpuCounter.NextValue();
        return $"CPU Kullanımı: {cpuValue}%";
    }
}

