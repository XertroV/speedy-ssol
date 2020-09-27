using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public abstract class TextureLineGraph<T>
{
    private int width;
    private int height;
    private int nSeries = 1;
    private int historySize = 3600;
    private int entryNumber = 0;
    private float[,] history;
    private Color? tbOutlineC;
    private int? dashPeriod;
    private Color32 clear;
    private Color32 green;
    private Color32[] pixels;
    private float[] last;
    private Color32[] colors;

    public int Width { get; private set; }
    public int Height { get; private set; }
    public float MaxForScale { get; private set; } = float.MinValue;
    public Texture2D Texture { get; private set; }
    public uint ScaleFactor { get; private set; }

    private void Init(int w, int h, Color c, uint scaleFactor, float initMax = float.MinValue)
    {
        if (scaleFactor < 1)
        {
            throw new Exception("TextureLineGraph: scaleFactor must be >= 1");
        }

        Width = w;
        Height = h;
        colors = new Color32[50];
        colors[0] = c;
        Texture = new Texture2D(w, h, TextureFormat.RGBA32, true);
        ConfigureTopBottomOutline(new Color(1f, 1f, 1f, 0.5f), rebuildTexture: false);
        ScaleFactor = scaleFactor;
        MaxForScale = initMax;
        if (scaleFactor * Width > historySize)
        {
            historySize = (int)scaleFactor * Width;
        }
        history = new float[nSeries, historySize];
        clear = new Color32(255, 255, 255, 0);
        green = new Color32(0, 255, 0, 255);
        pixels = new Color32[w * h];
        SetPixelsClear();
        Texture.Apply();

        last = new float[nSeries];
    }

    public void AddSeries(Color32 c)
    {
        colors[nSeries] = c;
        nSeries++;
        history = new float[nSeries, historySize];
        last = new float[nSeries];
    }

    public TextureLineGraph()
    {
        Init((int)(Screen.width * 0.1f), (int)(Screen.height * 0.1f), Color.red, 1);
    }

    public TextureLineGraph(int w, int h, Color color, uint scaleFactor, float initMax = float.MinValue)
    {
        Init(w, h, color, scaleFactor, initMax);
    }

    private void SetPixelsClear()
    {
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = clear;
        }
        Texture.SetPixels32(pixels);
    }

    public void ConfigureTopBottomOutline(Color c, int dashPeriod = 5, bool rebuildTexture = true)
    {
        tbOutlineC = c;
        this.dashPeriod = dashPeriod;
        if (rebuildTexture)
        {
            RebuildTexture();
        }
    }

    private int XYToIndex(int x, int y, bool debugLog = false)
    {
        var ret = ((y % Height) * Width) + (x % Width);
        if (debugLog)
        {
            //Debug.Log(x + "," + y + " -> " + ret);
        }
        return ret;

    }

    public void AddDataPoint(T value, int series = 0)
    {
        AddDataPointAt(value, entryNumber, series: series);
        entryNumber++;
    }

    private void UpdateTexture()
    {
        Texture.SetPixels32(pixels, 0);
        Texture.Apply();
    }

    public void AddDataPointAt(T value, int atEntry, int series = 0)
    {
        if (AddDataPointInner(value, atEntry, series))
        {
            RebuildTexture();
        }
        UpdateTexture();
    }

    public void AddMultipleDataPointsAt(T[] values, int atEntry)
    {
        bool rebuild = false;
        bool r2;
        for (int s = values.Length - 1; s >= 0; s--)
        {
            r2 = AddDataPointInner(values[s], atEntry, s, rebuild);
            rebuild = rebuild || r2;
        }
        if (rebuild)
        {
            RebuildTexture();
        }
        UpdateTexture();
    }

    private bool AddDataPointInner(T value, int atEntry, int s, bool justAdd = false)
    {
        var v = Math.Abs(ValueToFloat(value));
        history[s, atEntry % historySize] = v;

        if (v > MaxForScale || justAdd)
        {
            MaxForScale = v == 0f ? 0.0001f : v;
            return true;
        }
        else
        {
            var scaledEntry = atEntry / (int)ScaleFactor;
            pixels[XYToIndex(scaledEntry, (int)(v * Height / MaxForScale), true)] = colors[s];
            if (s == 0)
            {
                EnusreWhiteOriginTick();
                SetColumnToColor((scaledEntry + 1), clear);
                SetColumnToColor((scaledEntry + 2), clear);
            }
            FillBetween(last[s], v, scaledEntry, series: s);
        }
        if (atEntry > entryNumber)
        {
            entryNumber = atEntry;
        }
        last[s] = v;
        return false;
    }

    public abstract float ValueToFloat(T v);

    private void SetColumnToColor(int x, Color c)
    {
        for (var i = 0; i < Height; i++)
        {
            pixels[XYToIndex(x, i)] = c;
        }
    }

    private void FillBetween(float thisLast, float v, int scaledEntry, int series = 0)
    {
        var last_ = (int)(thisLast * Height / MaxForScale);
        var v_ = (int)(v * Height / MaxForScale);
        var len = v_ - last_;
        // if len > 0 we go *below* v, otherwise *above*
        if (len == 0) { return; }
        int start = len > 0 ? last_ : v_;
        int end = len > 0 ? v_ : last_;
        for (var i = start; i <= end; i++)
        {
            pixels[XYToIndex(scaledEntry, i, true)] = colors[series];
        }
    }

    public void RebuildTexture()
    {
        SetPixelsClear();
        var minVal = Math.Max(entryNumber - Width * (int)ScaleFactor, 0);
        var maxVal = entryNumber;
        int p;
        for (int s = nSeries - 1; s >= 0; s--)
        {
            float last_ = 0;
            float v_;
            int scaledEntry;
            for (int i = minVal; i <= maxVal; i++)
            {
                scaledEntry = (int)(i / ScaleFactor);
                v_ = history[s, i % historySize];
                p = XYToIndex(scaledEntry, (int)(v_ * Height / MaxForScale));
                pixels[p] = colors[s];
                FillBetween(last_, v_, scaledEntry, series: s);
                last_ = v_;
            }
            /* if (dashPeriod.HasValue && tbOutlineC.HasValue)
            {
                for (int i = 0; i < Width; i++)
                {
                    pixels[XYToIndex(i, (i / dashPeriod.Value) % 2)] = tbOutlineC.Value;
                }
            } */
        }
        EnusreWhiteOriginTick();
    }

    private void EnusreWhiteOriginTick()
    {
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                //Debug.Log("coords: " + (i * Width + j) + " ;; " + (i * Width + (Width - j - 1)) + " :: " + Width + " :: " + j);
                //Debug.Log(pixels.Length);
                pixels[i * Width + j] = Color.white;
                pixels[i * Width + (Width - j - 1)] = Color.red;
            }
        }
    }

    public void GUIDraw()
    {
        GUIDraw(0, 0);
    }

    public void GUIDraw(float x, float y)
    {
        GUI.BeginGroup(new Rect(x, y, Width, Height));
        float scaledEntry = entryNumber / ScaleFactor;
        scaledEntry += (entryNumber % ScaleFactor) / ScaleFactor;
        float xOffset = ((scaledEntry + 3) % Width);
        //var xOffset = ((scaledEntry + Width / 2) % Width);
        GUI.DrawTexture(new Rect(scaledEntry < xOffset ? 0 : 0 - xOffset, 0, Width, Height), Texture);
        GUI.DrawTexture(new Rect(scaledEntry < xOffset ? Width : Width - xOffset, 0, Width, Height), Texture);
        GUI.DrawTexture(new Rect(scaledEntry < xOffset ? 2 * Width : 2 * Width - xOffset, 0, Width, Height), Texture);
        GUI.EndGroup();
    }
}


public class TextureLineGraphOfFloat : TextureLineGraph<float>
{
    public TextureLineGraphOfFloat(int w, int h, Color c, uint sf, float initMax) : base(w, h, c, sf, initMax) { }
    public TextureLineGraphOfFloat() : base() { }

    public override float ValueToFloat(float v)
    {
        return v;
    }
}


public class TextureLineGraphOfInt : TextureLineGraph<int>
{
    public TextureLineGraphOfInt(int w, int h, Color c, uint sf, float initMax) : base(w, h, c, sf, initMax) { }
    public TextureLineGraphOfInt() : base() { }

    public override float ValueToFloat(int v)
    {
        return (float)v;
    }
}