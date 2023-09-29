namespace MachineSimulation.Models;
internal class AssemblyLine
{
    public double PistonPressure { get; private set; }

    public Relay? CurrentRelay { get; private set; }

    public int CounterOk { get; set; }
    public int CounterNOk { get; set; }

    public bool LastResultOk { get; set; }

    private readonly ILogger _logger;

    public double MaxCoilResistForTest { get; set; }
    public double MinCoilResistForTest { get; set; }

    public IList<Relay> ProcessedRelays { get; set; } = new List<Relay>();

    public AssemblyLine(ILogger logger, Random random, double minCoilResistForTest, double maxCoilResistForTest)
    {
        _logger = logger;
        PistonPressure = NewPistonPressure(random);

        MinCoilResistForTest = minCoilResistForTest;
        MaxCoilResistForTest = maxCoilResistForTest;
    }


    // new piece arrived
    public void LoadNewPiece(Relay relay)
    {
        if (CurrentRelay != null)
            ProcessedRelays.Add(CurrentRelay);
        CurrentRelay = relay;
    }

    public void AdjustPistonPressure(double pistonPressure)
    {
        PistonPressure = pistonPressure;
    }

    public void ProcessPiece()
    {
        CurrentRelay!.CalculateDebugValues(MinCoilResistForTest, MaxCoilResistForTest);

        var pressure = CurrentRelay.DebugMaxCorrectPressure -
                       (CurrentRelay.DebugMaxCorrectPressure - CurrentRelay.DebugMinCorrectPressure) / 2;
        PistonPressure = pressure;
        CurrentRelay!.PushContacts(PistonPressure);
    }

    public void TestPiece()
    {
        if (CurrentRelay == null)
            return;

        var ok = !(CurrentRelay.CurrentCoilResistance > MaxCoilResistForTest);

        if (CurrentRelay.CurrentCoilResistance < MinCoilResistForTest)
            ok = false;

        LastResultOk = ok;
        var debugMsg =
            $"PistonPressure: {PistonPressure}, Gap: {CurrentRelay.CurrentContactGap} Resist: {CurrentRelay.CurrentCoilResistance}, Result: {(ok ? "OK" : "nOK")}, DebugMin: {CurrentRelay.DebugMinCorrectPressure}, DebugMax: {CurrentRelay.DebugMaxCorrectPressure}";
        if (ok)
        {
            CounterOk++;
            _logger.LogInformation(debugMsg);
        }
        else
        {
            CounterNOk++;
            _logger.LogWarning(debugMsg);
        }
    }



    private double NewPistonPressure(Random random)
    {
        var pressure = 1d;
        //var pressure = random.Next(15,25);
        _logger.LogInformation($"Default piston presure for assembly line: {pressure}");
        return pressure;
    }
}
