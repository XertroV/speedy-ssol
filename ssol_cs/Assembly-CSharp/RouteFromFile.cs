using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime;

public class RouteFromFile : Route
{
    public RouteFromFile(string filename)
    {
        using (TextReader file = new StreamReader(filename))
        {
            List<float[]> times = new List<float[]>();
            var lines = file.ReadToEnd();
            foreach (var line in lines.Split('\n'))
            {
                var row = line.Split(',');
                float[] pair = { float.Parse(row[0]), float.Parse(row[1]) };
                times.Add(pair);
            }
        }
    }

    public static Dictionary<int, string> knownOrbNames = new Dictionary<int, string>
    {
        {94,"Tutorial - 7"},
        {23,"Donut - 22"},
        {56,"U-Turn - 44"},
        {59,"-> Hallway"},
        {47,"Hallway ->"},
        {92,"-> Park"},
        {90,"Park ->"},
        {71,"Final Countdown" },
        {99,"End - 100" }
    };

    public override int[] SplitOn()
    {
        throw new NotImplementedException();
    }

    public override string[] SplitNames()
    {
        throw new NotImplementedException();
    }

    public override float[] BenchmarkSplits()
    {
        throw new NotImplementedException();
    }
}
