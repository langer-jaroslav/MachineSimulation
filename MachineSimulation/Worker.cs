using MachineSimulation.Models;
using MachineSimulation.Services;

namespace MachineSimulation;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly CsvService _csvService;

    public Worker(ILogger<Worker> logger, CsvService csvService)
    {
        _logger = logger;
        _csvService = csvService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(1000, stoppingToken);
        var random = new Random();
        var minCoilResistForTest = 45;
        var maxCoilResistForTest = 50;


        var assemblyLine = new AssemblyLine(_logger, random, minCoilResistForTest, maxCoilResistForTest);

        await _csvService.WriteToFileHeaderAsync();
        while (!stoppingToken.IsCancellationRequested)
        {
            var newRelay = new Relay(random);
            assemblyLine.LoadNewPiece(newRelay);
            assemblyLine.AdjustPistonPressure(assemblyLine.PistonPressure);
            assemblyLine.ProcessPiece();
            assemblyLine.TestPiece();

            await _csvService.WriteToFileAsync(newRelay);

            //await Task.Delay(1000, stoppingToken);
        }

        

    }
}
