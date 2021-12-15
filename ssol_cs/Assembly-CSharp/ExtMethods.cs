using System;
using System.Collections.Generic;

public class IxVal<T>
{
    public readonly T val;
    public readonly int ix;

    public IxVal(T val, int ix)
    {
        this.val = val;
        this.ix = ix;
    }
}


public static class ExtMethods
{
    public static void Each<T>(IEnumerable<T> ie, Action<IxVal<T>> action)
    {
        var i = 0;
        foreach (var e in ie) action(new IxVal<T>(e, i++));
    }
}
