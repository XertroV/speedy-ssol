using System;

// Token: 0x02000035 RID: 53
internal class RouteWr20200921Short : Route
{
    // Token: 0x0400017D RID: 381
    private int[] splitOn = new int[]
    {
        94,
        23,
        56,
        59,
        47,
        92,
        90,
        71,
        99
    };

    // Token: 0x0400017E RID: 382
    private string[] splitNames = new string[]
    {
        "Tutorial - 7",
        "Donut - 22",
        "U-Turn - 44",
        "-> Hallway",
        "Hallway ->",
        "-> Park",
        "Park ->",
        "Final Countdown",
        "End - 100"
    };

    // Token: 0x0400017F RID: 383
    private float[] wrSplits = new float[]
    {
        10.36f,
        30.49f,
        48.26f,
        70.35f,
        74.95f,
        83.33f,
        93.85f,
        111.94f,
        118.02f
    };

    public override float[] BenchmarkSplits()
    {
        return this.wrSplits;
    }

    public override string[] SplitNames()
    {
        return this.splitNames;
    }

    public override int[] SplitOn()
    {
        return this.splitOn;
    }
}
