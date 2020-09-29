using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime;
using UnityEngine;

public class RouteFromFile : Route
{
    private LoadedRoute r;

    public RouteFromFile(string filename)
    {
        Debug.Log("loading file: " + filename);
        using (TextReader file = new StreamReader(filename))
        {
            string line;
            List<float> splits = new List<float>();
            List<int> splitOrbs = new List<int>();
            List<string> splitNames = new List<string>();
            var filenameParts = filename.Split('\\');
            string name = filenameParts[filenameParts.Length - 1];
            string currReading = null;
            while ((line = file.ReadLine()) != null)
            {
                if (line.Length == 0) { continue; }
                if (line.StartsWith(">"))
                {
                    name = line.Remove(0, 1);
                    continue;
                }
                if (line.Contains(":"))
                {
                    currReading = line.Replace(":", "");
                    continue;
                }

                switch (currReading)
                {
                    case "splitOrbs":
                        splitOrbs.Add(int.Parse(line));
                        break;
                    case "splitNames":
                        splitNames.Add(line);
                        break;
                    case "splits":
                        splits.Add(float.Parse(line));
                        break;
                }
            }
            r = new LoadedRoute();
            r.name = name;
            r.splits = splits.ToArray();
            r.splitNames = splitNames.ToArray();
            r.splitOrbs = splitOrbs.ToArray();
            Debug.Log($"RouteFromFile loaded: {r}");
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

    public override string Name()
    {
        return r.name;
    }

    public override int[] SplitOn()
    {
        return r.splitOrbs;
    }

    public override string[] SplitNames()
    {
        return r.splitNames;
    }

    public override float[] BenchmarkSplits()
    {
        return r.splits;
    }

    public override string ToString()
    {
        return $"RouteFromFile>{this.r.ToString()}";
    }
}


class LoadedRoute
{
    public string name;
    public int[] splitOrbs;
    public string[] splitNames;
    public float[] splits;

    //public override float[] BenchmarkSplits()
    //{
    //    return this.splits;
    //}

    //public override string[] SplitNames()
    //{
    //    return this.splitNames;
    //}

    //public override int[] SplitOn()
    //{
    //    return this.splitOn;
    //}

    public override string ToString()
    {
        return $"LoadedRoute ({name}) | splits: {PP(splits)} | splitOn: {PP(splitOrbs)} | splitNames: {PP(splitNames)}";
    }

    private static string[] MapToString<T>(T[] ts)
    {
        var ret = new string[ts.Length];
        for (int i = 0; i < ts.Length; i++)
        {
            ret[i] = ts[i].ToString();
        }
        return ret;
    }

    private static string PP<T>(T[] ss)
    {
        return "[" + string.Join(",", MapToString(ss)) + "]";
    }
}