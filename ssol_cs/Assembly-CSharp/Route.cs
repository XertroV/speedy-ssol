using System;

// Token: 0x02000034 RID: 52
public abstract class Route
{
    public abstract string Name();

    public abstract int[] SplitOn();

    // Token: 0x0400017B RID: 379
    public abstract string[] SplitNames();

    // Token: 0x0400017C RID: 380
    public abstract float[] BenchmarkSplits();
}

class EmptyRoute : Route
{
    public override string Name()
    {
        return "<None>";
    }
    public override float[] BenchmarkSplits()
    {
        return new float[] { };
    }

    public override string[] SplitNames()
    {
        return new string[] { };
    }

    public override int[] SplitOn()
    {
        return new int[] { };
    }
}