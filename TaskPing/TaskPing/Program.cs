// See https://aka.ms/new-console-template for more information

using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Threading;

Stopwatch sw = Stopwatch.StartNew();

string path = "C:\\studia\\rokIII\\Vsemestr\\ProgramowanieWCSharp\\cwiczenia\\CW08\\List.txt";
List<string> addresses = new List<string>();
using (StreamReader sr = new StreamReader(path))
{
    while (!sr.EndOfStream)
    {
        string line = sr.ReadLine();
        string tmp = line.Substring(line.IndexOf(";") + 1);
        if(tmp == "Adres")
        {
            continue;
        }
        addresses.Add(tmp); 
    }

    SemaphoreSlim semaphore = new SemaphoreSlim(4);
    Task pingAddresses = Task.Run(async () =>
    {

        var pingTasks = addresses.Select(ip => PingWithSemaphoreAsync(ip, semaphore)).ToList();

        await Task.WhenAll(pingTasks);
    });

    await pingAddresses;

    Console.WriteLine("Main method finished.");
}

static async Task PingWithSemaphoreAsync(string ipAddress, SemaphoreSlim semaphore)
{
    await semaphore.WaitAsync(); 
    try
    {

        PingReply pingReply = await new Ping().SendPingAsync(ipAddress, 2000);


        await Console.Out.WriteLineAsync($"{ipAddress} {pingReply.Status}");
    }
    finally
    {
        semaphore.Release(); 
    }
}

sw.Stop();
Console.WriteLine(sw.Elapsed);




