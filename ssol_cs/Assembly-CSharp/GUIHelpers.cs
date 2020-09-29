using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GUIHelpers
{
    private static Texture2D speedyTex = LoadTextureFromPng(@"a-speedy-ssol_Data\Speedrun\speedy.png");

    public static Vector2 SpeedyTexDimensions { get => new Vector2(speedyTex.width, speedyTex.height); }

    public static void DrawControlsInfo()
    {
    }

    public static Texture2D LoadTextureFromPng(string filepath)
    {
        var tex = new Texture2D(2, 2);
        tex.LoadImage(File.ReadAllBytes(filepath));
        return tex;
    }

    private static float FloatID(float x)
    {
        return x;
    }

    public delegate TResult Func<in T, out TResult>(T arg);

    public static void DrawSpeedyTex(Rect r)
    {
        DrawSpeedyTex(r, FloatID);
    }

    public static void DrawSpeedyTex(Rect r, Func<float, float> f, float stretchFactor = 2f, float scaleCloserToOne = 0.5f)
    {
        var lscale = f(ScaleCloserToOne(TriangleWave(Stretch(Time.realtimeSinceStartup, stretchFactor)), scaleCloserToOne));
        /*
        Debug.Log(string.Join("\n", new string[] {
            "input value: " + Time.realtimeSinceStartup,
            "output tri wave: " + TriangleWave(Time.realtimeSinceStartup),
            "output scale 2/3: " + ScaleCloserToOne(TriangleWave(Time.realtimeSinceStartup), scaleCloserToOne),
            "output from f: " + lscale,
        }));
        */
        var ls = new Vector2(lscale, lscale);
        var ls_ = new Vector2(1 - lscale, 1 - lscale);
        var widthDiff = r.width * ls_.x;
        var heightDiff = r.height * ls_.y;
        var r2 = new Rect(r.xMin + widthDiff / 2, r.yMin + heightDiff / 2, r.width * ls.x, r.height * ls.y);
        GUI.DrawTexture(r2, speedyTex);
    }

    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 1f / 45f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.SetColors(color, color);
        lr.SetWidth(0.001f, 0.001f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }

    // https://forum.unity.com/threads/outlined-text.43698/#post-278019
    public static void DrawOutline(Rect position, GUIContent text, GUIStyle style, Color outColor, float strokeWidth = 1)
    {
        var backupStyle = style;
        var oldColor = style.normal.textColor;
        style.normal.textColor = outColor;
        position.x -= strokeWidth;
        GUI.Label(position, text, style);
        position.y -= strokeWidth;
        GUI.Label(position, text, style);
        position.x += strokeWidth;
        GUI.Label(position, text, style);
        position.x += strokeWidth;
        GUI.Label(position, text, style);
        position.y += strokeWidth;
        GUI.Label(position, text, style);
        position.y += strokeWidth;
        GUI.Label(position, text, style);
        position.x -= strokeWidth;
        GUI.Label(position, text, style);
        position.x -= strokeWidth;
        GUI.Label(position, text, style);
        position.y -= strokeWidth;
        GUI.Label(position, text, style);
        position.x += strokeWidth;
        style.normal.textColor = oldColor;
        GUI.Label(position, text, style);
        style = backupStyle;
    }

    public static float EaseInOutBounce(float x)
    {
        return x < 0.5f ? (1 - EaseOutBounce(1 - 2 * x)) / 2 : (1 + EaseOutBounce(2 * x - 1)) / 2;
    }

    public static float EaseInBounce(float x)
    {
        return 1 - EaseOutBounce(1 - x);
    }

    private const float n1 = 7.5625f;
    private const float d1 = 2.75f;

    public static float EaseOutBounce(float x)
    {
        if (x < 1 / d1)
        {
            return n1 * x * x;
        }
        else if (x < 2 / d1)
        {
            return n1 * (float)Math.Pow(x - 1.5f / d1, 2) + 0.75f;
        }
        else if (x < 2.5 / d1)
        {
            return n1 * (float)Math.Pow(x - 2.25f / d1, 2) + 0.9375f;
        }
        else
        {
            return n1 * (float)Math.Pow(x - 2.625f / d1, 2) + 0.984375f;
        }
    }

    public static float EaseOutElastic(float x)
    {
        var c4 = (2 * Math.PI) / 3;
        var ret = x == 0
              ? 0
              : x == 1
              ? 1
              : Math.Pow(2, -10 * x) * Math.Sin((x * 10 - 0.75) * c4) + 1;
        return (float)ret;
    }

    private const float twp = 1;
    public static float TriangleWave(float x)
    {
        // https://en.wikipedia.org/wiki/Triangle_wave#Floor_function
        var floored = Math.Floor(2 * x / twp + 0.5f);
        var triWave = (float)(4 / twp * (x - twp / 2f * floored) * Math.Pow(-1, floored));
        // normalize to go between 0 and 1
        return triWave / 2f + 0.5f;
    }

    public static float ScaleCloserToOne(float x, float k)
    {
        return (x * (1 - k)) + k;
    }

    public static float Stretch(float x, float factor)
    {
        return x / factor;
    }
}
