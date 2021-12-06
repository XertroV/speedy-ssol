using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime;
using UnityEngine;

public class RouteFromFile : Route
{
    private LoadedRoute r;

    List<float> splits = new List<float>();
    List<float> rawSplits = new List<float>();
    List<int> splitOrbs = new List<int>();
    List<string> splitNames = new List<string>();
    bool isWin = false;

    public RouteFromFile(string filename, bool isWinAlways)
    {
        Debug.Log("loading file: " + filename);
        isWin = isWinAlways;
        using (TextReader file = new StreamReader(filename))
        {
            string line;
            var filenameParts = filename.Split('\\');
            string name = filenameParts[filenameParts.Length - 1];
            string currReading = null;
            var opts = new Dictionary<string, string>() { { "loadRoute", "none" } };
            while ((line = file.ReadLine()) != null)
            {
                if (line.Length == 0) { continue; }
                else if (line.StartsWith(">"))
                {
                    name = line.Remove(0, 1);
                    continue;
                }
                else if (line.StartsWith("#") || line.StartsWith(";"))
                {
                    // comment
                    continue;
                }
                else if (line.Contains("="))
                {
                    string[] ov = line.Split('=');
                    string opt = ov[0];
                    string val = ov[1];
                    opts[opt] = val;
                }
                else if (line.Contains(":"))
                {
                    currReading = line.Replace(":", "");
                    continue;
                }
                else
                {
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
                            rawSplits.Add(float.Parse(line));
                            break;
                        default:
                            Debug.LogWarning(string.Join("\n", new string[] {
                                $"WARNING -- Reading route from file named `{filename}`.",
                                "I am meant to be reading section `{currReading}` -- but I don't know what that is.",
                                "Is the section name typoed?",
                                $"Otherwise, there might be a problem with this line:",
                                $"    `{line}`",
                                "-- WARNING END --"
                            }));
                            break;
                    }
                }
            }

            var routeName = opts["loadRoute"];
            Dictionary<int, string> routeDict = null;

            // filter lists and load names from a route; do by default if no names are provided and 100 splits are included
            if (routeName == "2020-12" || splitNames.Count == 0 && splits.Count == 100)
            {
                routeDict = route_2020_12;
            }
            else if (routeName != "none")
            {
                Debug.LogWarning($"Warning: cannot load unknown route: `{routeName}` in file `{filename}`");
            }

            if (routeDict != null)
            {
                var newSplits = new List<float>();
                var newSplitNames = new List<string>();
                var newSplitOrbs = new List<int>();
                for (int i = 0; i < splits.Count; i++)
                {
                    var orb = splitOrbs[i];
                    if (routeDict.ContainsKey(orb))
                    {
                        var split = splits[i];
                        newSplits.Add(split);
                        newSplitOrbs.Add(orb);
                        newSplitNames.Add(route_2020_12[orb]);
                    }

                }
                splits = newSplits;
                splitOrbs = newSplitOrbs;
                splitNames = newSplitNames;
            }
            
            // splitNames is an optional section
            if (splitNames.Count == 0)
            {
                foreach (var o in splitOrbs)
                {
                    splitNames.Add(o.ToString());
                }
            }

            if (!isWinAlways)
            {
                isWin = rawSplits.Count >= 100;
            }

            r = new LoadedRoute();
            r.name = name;
            r.splits = splits.ToArray();
            r.splitNames = splitNames.ToArray();
            r.splitOrbs = splitOrbs.ToArray();
            r.rawSplits = rawSplits.ToArray();
            if (r.splits.Length != r.splitNames.Length || r.splits.Length != r.splitOrbs.Length)
            {
                Debug.LogWarning(string.Join("\n", new string[]
                {
                    $"WARNING -- Reading roue from `{filename}` and ended up with mismatching numbers of splits, names, and orbs:",
                    $"splits (times): {r.splits.Length} total",
                    $"splits (names): {r.splitNames.Length} total",
                    $"splits (orbs) : {r.splitOrbs.Length} total",
                    "You probably want to check this split file to make sure it's all okay."
                }));
            }
            Debug.Log($"RouteFromFile loaded: {r}");
        }
    }

    public static Dictionary<int, string> route_2020_12 = new Dictionary<int, string>
    {
        {48,"1/2 Tut - 4"},
        {94,"Tutorial - 7"},
        {23,"Donut - 22"},
        {6,"-> Park - 42"},
        {47,"Park -> - 50"},
        {45,"-> Household - 62"},
        {56,"U-Turn - 78"},
        {71,"Final Countdown - 87" },
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

    public override float WinTime()
    {
        if (r.splits.Length == 0)
            return 999f;
        return Last(r.splits);
    }
    private static T Last<T>(T[] ts)
    {
        return ts[ts.Length - 1];
    }

    public override void SetName(string newName)
    {
        r.name = newName;
    }

    public override bool IsWin()
    {
        return isWin;
    }
}


class LoadedRoute
{
    public string name;
    public int[] splitOrbs;
    public string[] splitNames;
    public float[] splits;
    public float[] rawSplits;

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