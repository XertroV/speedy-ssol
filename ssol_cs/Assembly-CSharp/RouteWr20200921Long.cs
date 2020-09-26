using System;

// Token: 0x02000038 RID: 56
internal class RouteWr20200921Long : Route
{
    // Token: 0x04000182 RID: 386
    public int[] splitOn = new int[]
    {
        94,
        23,
        56,
        74,
        33,
        60,
        44,
        27,
        77,
        3,
        55,
        66,
        49,
        84,
        45,
        59,
        47,
        58,
        41,
        6,
        92,
        14,
        10,
        11,
        2,
        19,
        90,
        53,
        57,
        54,
        62,
        68,
        67,
        75,
        81,
        15,
        71,
        39,
        40,
        91,
        99
    };

    // Token: 0x04000183 RID: 387
    public string[] splitNames = new string[]
    {
        "Tutorial - 7",
        "Donut - 22",
        "U-Turn - 44",
        "74",
        "33",
        "60",
        "44",
        "27",
        "77",
        "03",
        "55",
        "66",
        "49",
        "84",
        "45",
        "-> Hallway",
        "Hallway ->",
        "58",
        "41",
        "06",
        "-> Park",
        "14",
        "10",
        "11",
        "02",
        "19",
        "Park ->",
        "53",
        "57",
        "54",
        "62",
        "68",
        "67",
        "75",
        "81",
        "15",
        "Final Countdown",
        "39",
        "40",
        "91",
        "End - 100"
    };

    // Token: 0x04000184 RID: 388
    public float[] wrSplits = new float[]
    {
        10.36f,
        30.49f,
        48.29f,
        50.78f,
        52.08f,
        53.58f,
        55.15f,
        56.81f,
        58.34f,
        59.64f,
        62f,
        63.16f,
        65.48f,
        67.39f,
        69.54f,
        70.35f,
        74.95f,
        77.88f,
        78.42f,
        79.71f,
        83.33f,
        85.6f,
        86.83f,
        88.18f,
        89.78f,
        91.38f,
        93.85f,
        99.86f,
        100.42f,
        101.89f,
        103.19f,
        104.21f,
        104.9f,
        105.36f,
        105.78f,
        109.04f,
        111.94f,
        112.65f,
        114.23f,
        116.79f,
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
