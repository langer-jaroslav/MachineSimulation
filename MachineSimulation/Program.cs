using MachineSimulation;
using MachineSimulation.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<CsvService>(new CsvService("./data.csv"));
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
