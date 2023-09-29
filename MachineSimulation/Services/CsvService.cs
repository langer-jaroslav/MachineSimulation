using MachineSimulation.Models;
using System.Text;

namespace MachineSimulation.Services;
public class CsvService
{
    public string Path { get; private set; }
    public CsvService(string path)
    {
        Path = path;
    }
    public async Task WriteToFileAsync(IList<Relay> relays)
    {
        var utf8WithoutBom = new UTF8Encoding(false);
        //var win1250 = Encoding.GetEncoding("Windows-1250");
        await using StreamWriter file = new(Path, true, utf8WithoutBom);
        await file.WriteLineAsync(Relay.GetCsvHeader());
        foreach (var relay in relays)
        {
            await file.WriteLineAsync(relay.GetCsv());
        }
        file.Close();
    }


    public async Task WriteToFileHeaderAsync()
    {
        var utf8WithoutBom = new UTF8Encoding(false);
        //var win1250 = Encoding.GetEncoding("Windows-1250");
        await using StreamWriter file = new(Path, true, utf8WithoutBom);
        await file.WriteLineAsync(Relay.GetCsvHeader());
        file.Close();
    }
    public async Task WriteToFileAsync(Relay relay)
    {
        var utf8WithoutBom = new UTF8Encoding(false);
        //var win1250 = Encoding.GetEncoding("Windows-1250");
        await using StreamWriter file = new(Path, true, utf8WithoutBom);
        await file.WriteLineAsync(relay.GetCsv());
        file.Close();
    }
}
