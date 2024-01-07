using System.Net.NetworkInformation;
using System.Text;
using System.Diagnostics;


Stopwatch sw = new Stopwatch();
sw.Start();
string filePath = "C:\\studia\\rokIII\\Vsemestr\\ProgramowanieWCSharp\\cwiczenia\\CW08\\List.txt";
string message = string.Empty;
string address = string.Empty;
try
{
    using (StreamReader reader = new StreamReader(filePath))
    {
        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            address = line.Substring(line.IndexOf(';') + 1);
            if (address == "Adres")
            {
                continue;
            }
            Ping pingSender = new Ping();
            PingReply reply = pingSender.Send(address);
            if (reply.Status == IPStatus.Success)
            {
                message = "Ping success";
            }
            else
            {
                message = "Ping failed";
            }
            Console.WriteLine(address + " -> " + message);

        }
    }
}
catch (Exception e)
{
    Console.WriteLine(e.ToString());
}

sw.Stop();

Console.WriteLine($"time elapsed {sw.Elapsed}");
