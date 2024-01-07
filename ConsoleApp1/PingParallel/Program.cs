using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;

Stopwatch sw = Stopwatch.StartNew();

string filePath = "C:\\studia\\rokIII\\Vsemestr\\ProgramowanieWCSharp\\cwiczenia\\CW08\\List.txt";
List<string> addresses = new List<string>();
try
{
    using (StreamReader sr = new StreamReader(filePath))
    {
        while(!sr.EndOfStream)
        {
            string line  = sr.ReadLine();
            
            string tmp = line.Substring(line.IndexOf(";") + 1);
            if (tmp == "Adres")
            {
                continue;
            }
            addresses.Add(tmp);

        }
    }    
}
catch(Exception e)
{
    Console.WriteLine(e.ToString);
}
/*
Parallel.For(0, addresses.Count, i =>
{
    Ping pingSender = new Ping();

    PingReply reply = await pingSender.SendPingAsync(addresses[i], 150);
    if (reply.Status == IPStatus.Success)
    {
        Console.WriteLine(addresses[i] + " -> " + "Ping Success");
    }
    else
    {
        Console.WriteLine(addresses[i] + " -> " + "Ping failed");
    }
});
*/
var parsedDns = new ConcurrentQueue<string>();
var options = new ParallelOptions() { MaxDegreeOfParallelism = 4 };

Parallel.ForEachAsync(addresses, options, async (dnsStr, ct) =>
{
    Ping ping = new Ping();
    PingReply reply = await ping.SendPingAsync(dnsStr, 500);
    if (reply.Status == IPStatus.Success)
    {
        await Console.Out.WriteLineAsync(dnsStr + " -> " + "Ping succes, Roundtrip time:  " + reply.RoundtripTime );
    }
    else
    {
        await Console.Out.WriteLineAsync(dnsStr + " -> " + "Ping failed, Roundtrip time:" + reply.RoundtripTime );
    }
}).Wait();
sw.Stop();
Console.WriteLine(sw.Elapsed);


