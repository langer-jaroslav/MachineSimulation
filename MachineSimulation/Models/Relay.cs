using System.Globalization;

namespace MachineSimulation.Models;
public class Relay
{
    public double OriginCoilResistance { get; private set; }
    public double OriginContactGap { get; private set; }

    public double CurrentCoilResistance => OriginCoilResistance * (1 / CurrentContactGap);
    public double CurrentContactGap { get; set; }

    public double UsedPressure { get; private set; }


    public double DebugMaxCorrectPressure { get; set; }
    public double DebugMinCorrectPressure { get; set; }

    public bool IsOk => UsedPressure <= DebugMaxCorrectPressure && UsedPressure >= DebugMinCorrectPressure; 


    public Relay(Random random)
    {
        OriginCoilResistance = NewCoilResistance(random);
        OriginContactGap = NewContactGap(random);
        
        CurrentContactGap = OriginContactGap;
    }

    private double NewCoilResistance(Random random)
    {
        return random.Next(170,230);
    }
    private double NewContactGap(Random random)
    {
        return random.Next(20, 60);
    }

    public void PushContacts(double pressure)
    {
        UsedPressure = pressure;

        var newGap = CurrentContactGap * (1 / pressure);
        if (newGap > CurrentContactGap)
            return;
        if(newGap < 0)
            return;
        CurrentContactGap = newGap;
    }

    public void CalculateDebugValues(double minResist, double maxResist)
    {
        DebugMaxCorrectPressure = (OriginContactGap * maxResist) / OriginCoilResistance;

        DebugMinCorrectPressure = (OriginContactGap * minResist) / OriginCoilResistance;
    }

    public static string GetCsvHeader()
    {
        return "OriginCoilResistance;OriginContactGap;PistonPressure;Result;DebugMinPressure;DebugMaxPressure";
    }
    public string GetCsv()
    {
        var data = GetCsvHeader();
        data = data.Replace("OriginCoilResistance", OriginCoilResistance.ToString(CultureInfo.InvariantCulture));
        data = data.Replace("OriginContactGap", OriginContactGap.ToString(CultureInfo.InvariantCulture));
        data = data.Replace("PistonPressure", UsedPressure.ToString(CultureInfo.InvariantCulture));
        data = data.Replace("Result", IsOk?"Ok":"nOk");
        data = data.Replace("DebugMinPressure", DebugMinCorrectPressure.ToString(CultureInfo.InvariantCulture));
        data = data.Replace("DebugMaxPressure", DebugMaxCorrectPressure.ToString(CultureInfo.InvariantCulture));
        return data;
    }
}
