using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class MenuComponentSelectSplits
{
    public GUIStyle splitsStyle;
    private List<Route> routes = new List<Route>();
    public Route SelectedRoute { get; private set; }

    /* on a 1440p monitor */
    private static Vector2 scale = new Vector2(Screen.width / 2560f, Screen.height / 1440f);

    private static float arrowScale = 0.7f;
    public static Vector2 arrowDims = new Vector2(120 * arrowScale * scale.x, 200 * arrowScale * scale.y);
    private static float textYOffset = 20f * scale.y;

    private Texture2D[] arrowLTexs = { new Texture2D(1, 1), new Texture2D(1, 1), new Texture2D(1, 1) };
    private Texture2D[] arrowRTexs = { new Texture2D(1, 1), new Texture2D(1, 1), new Texture2D(1, 1) };

    private static float buttonScale = 1f;
    private static Vector2 buttonDims = new Vector2(350 * buttonScale * scale.x, 50f * buttonScale * scale.y);
    public GUIStyle buttonStyle;

    private Texture2D[] buttonTexs = { new Texture2D(1, 1), new Texture2D(1, 1), new Texture2D(1, 1) };

    private List<Action<Route>> cbs = new List<Action<Route>>();
    private GUIContent topText;
    private Vector2 ttDims;
    private int routeListPos;
    private GUIStyle smallStyle;

    public static float baseWidth = 800f;
    public static float width = baseWidth * scale.x;
    public static float baseHeight = 200f;
    public static float height = baseHeight * scale.y;
    public static float realHeight = (baseHeight + buttonDims.y) * scale.y;

    private static float optionsMenuYPos = 1300f;
    public static float OptionsMenuYPos { get => optionsMenuYPos; }

    public MenuComponentSelectSplits()
    {
        int padding = (int)(10 * scale.x);
        splitsStyle = new GUIStyle();
        splitsStyle.fontSize = (int)(40 * scale.y);
        splitsStyle.normal.textColor = Color.white;
        splitsStyle.padding = new RectOffset(0, 0, 0, 0);
        splitsStyle.alignment = TextAnchor.MiddleCenter;
        smallStyle = new GUIStyle(splitsStyle);
        smallStyle.fontSize = (int)(24 * scale.y);

        InitializeRoutes();

        this.arrowLTexs[0].LoadImage(File.ReadAllBytes(@"a-speedy-ssol_Data\Speedrun\arrow.png"));
        this.arrowLTexs[1].LoadImage(File.ReadAllBytes(@"a-speedy-ssol_Data\Speedrun\arrowHover.png"));
        this.arrowLTexs[2].LoadImage(File.ReadAllBytes(@"a-speedy-ssol_Data\Speedrun\arrowClick.png"));

        this.arrowRTexs[0] = FlipTexture(this.arrowLTexs[0]);
        this.arrowRTexs[1] = FlipTexture(this.arrowLTexs[1]);
        this.arrowRTexs[2] = FlipTexture(this.arrowLTexs[2]);

        this.buttonTexs[0].LoadImage(File.ReadAllBytes(@"a-speedy-ssol_Data\Speedrun\button.png"));
        this.buttonTexs[1].LoadImage(File.ReadAllBytes(@"a-speedy-ssol_Data\Speedrun\buttonHover.png"));
        this.buttonTexs[2].LoadImage(File.ReadAllBytes(@"a-speedy-ssol_Data\Speedrun\buttonClick.png"));

        foreach (var tex in buttonTexs)
        {
            TextureScale.Bilinear(tex, (int)buttonDims.x, (int)buttonDims.y);
        }

        buttonStyle = new GUIStyle();
        buttonStyle.fontSize = (int)(25 * scale.y);
        buttonStyle.normal.textColor = Color.white;
        buttonStyle.hover.textColor = buttonStyle.normal.textColor;
        buttonStyle.active.textColor = buttonStyle.normal.textColor;
        buttonStyle.padding = new RectOffset(0, 0, 0, 0);
        buttonStyle.margin = new RectOffset(0, 0, 0, 0);
        buttonStyle.alignment = TextAnchor.MiddleCenter;
        //buttonStyle.stretchHeight = true;
        //buttonStyle.stretchWidth = true;
        buttonStyle.wordWrap = true;
        buttonStyle.normal.background = buttonTexs[0];
        buttonStyle.hover.background = buttonTexs[1];
        buttonStyle.active.background = buttonTexs[2];


        topText = new GUIContent("Choose Splits for Comparison");
        ttDims = this.splitsStyle.CalcSize(topText);
    }

    public void DrawSelectSplitsFromMiddleCenter(float x, float y)
    {
        DrawSelectSplitsInOptionsRaw((x - width / 2), (y - height / 2));
    }

    public void DrawSelectSplitsFromTopLeft(float x, float y)
    {
        DrawSelectSplitsInOptionsRaw(x, y);
    }

    private void DrawSelectSplitsInOptionsRaw(float x, float y)
    {
        smallStyle.font = splitsStyle.font;

        GUI.BeginGroup(new Rect(x, y, width, realHeight), splitsStyle);

        // color and textures
        GUI.color = Color.white;

        GUI.skin.button.normal.background = this.arrowRTexs[0];
        GUI.skin.button.hover.background = this.arrowRTexs[1];
        GUI.skin.button.active.background = this.arrowRTexs[2];

        // right
        if (GUI.Button(new Rect((width - arrowDims.x), 0, arrowDims.x, arrowDims.y), ""))
        {
            UpdateRouteUI(+1);
        }

        GUI.skin.button.normal.background = this.arrowLTexs[0];
        GUI.skin.button.hover.background = this.arrowLTexs[1];
        GUI.skin.button.active.background = this.arrowLTexs[2];

        // left
        if (GUI.Button(new Rect(0, 0, arrowDims.x, arrowDims.y), ""))
        {
            UpdateRouteUI(-1);
        }

        // reload
        if (GUI.Button(new Rect((width - buttonDims.x) / 2, arrowDims.y, buttonDims.x, buttonDims.y), "Reload From Disk", buttonStyle))
        {
            InitializeRoutes();
        }

        // top text
        GUI.Label(new Rect((width - ttDims.x) / 2, textYOffset, ttDims.x, ttDims.y), topText, this.splitsStyle);
        GUI.Label(new Rect(0, (textYOffset + ttDims.y), width, ttDims.y), SelectedRoute.Name(), this.splitsStyle);

        var splitsRank = new GUIContent($"{routeListPos + 1} / {routes.Count}");
        var rankDims = this.smallStyle.CalcSize(splitsRank);
        GUI.Label(new Rect((width - rankDims.x) / 2, (textYOffset + 2 * ttDims.y), rankDims.x, rankDims.y), splitsRank, this.smallStyle);

        GUI.EndGroup();
    }

    public void UpdateRouteUI(int moveSpots)
    {
        routeListPos += moveSpots + routes.Count;
        routeListPos %= routes.Count;
        // routeListPos = routeListPos < 0 ? 0 : routeListPos >= routes.Count ? routes.Count - 1 : routeListPos;
        SelectedRoute = routes[routeListPos];
        TriggerCallbacks(SelectedRoute);
    }

    private void TriggerCallbacks(Route r)
    {
        foreach (var cb in cbs)
        {
            cb(r);
        }
    }

    public void AddCallback(Action<Route> cb)
    {
        cbs.Add(cb);
    }

    public void InitializeRoutes()
    {
        LoadSplitsFromLocalDir();
        // temp store empty route here
        SelectedRoute = new EmptyRoute();
        routes.Add(SelectedRoute);
        SelectFastestRoute();
        routeListPos = routes.FindIndex(r => r == SelectedRoute);
        TriggerCallbacks(SelectedRoute);
        Debug.Log($"Loaded Routes: {routes}");
    }

    public static string SplitsDir()
    {
        return Environment.CurrentDirectory + "\\splits";
    }

    private void LoadSplitsFromLocalDir()
    {
        routes.RemoveAll((r) => true);
        var splitsDir = SplitsDir();
        var dirInfo = new DirectoryInfo(splitsDir);
        Debug.Log($"Loading splits from directory `{dirInfo}`.\n Files:\n - {string.Join("\n - ", MapToString(dirInfo.GetFiles()))}");
        foreach (var splitFile in dirInfo.GetFiles())
        {
            Debug.Log("adding file: " + splitFile);
            routes.Add(new RouteFromFile(splitFile.FullName, true));
            Debug.Log(routes.Count);
        }

        var mySplitRoutes = new List<Route>();
        var mySplitsDir = SplitsDir() + "\\my-splits";
        var mySpDirInfo = new DirectoryInfo(mySplitsDir);
        foreach (var splitFile in mySpDirInfo.GetFiles())
        {
            Debug.Log("adding file: " + splitFile);
            mySplitRoutes.Add(new RouteFromFile(splitFile.FullName, false));
        }
        if (mySplitRoutes.Count > 0)
        {
            Route bestRoute = new EmptyRoute();
            List<Route> myRoutes = new List<Route>();
            foreach (var myRoute in mySplitRoutes)
            {
                Debug.Log($"testing route: {myRoute.Name()}");
                if (myRoute.IsWin())
                {
                    myRoutes.Add(myRoute);
                    if (myRoute.WinTime() < bestRoute.WinTime())
                    {
                        Debug.Log($"Found new bestRoute (from my-splits): {myRoute.Name()}");
                        bestRoute = myRoute;
                    }
                    else
                    {
                        Debug.Log($"Not bestRoute (from my-splits): {myRoute.Name()} with wintime: {myRoute.WinTime()}");
                    }
                }
            }
            bestRoute.SetName("PB: " + bestRoute.Name());
            routes.Insert(0, bestRoute);
            //routes.Add()
        }
    }

    private void SelectFastestRoute()
    {
        foreach (Route r in routes)
        {
            if (r.SplitOn().Length == 0) { continue; }
            var lastTime = Last(r.BenchmarkSplits());
            if (SelectedRoute.BenchmarkSplits().Length == 0 || lastTime < Last(SelectedRoute.BenchmarkSplits()))
            {
                SelectedRoute = r;
            }
        }
    }

    private static T Last<T>(T[] ts)
    {
        return ts[ts.Length - 1];
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

    // http://answers.unity.com/answers/1761721/view.html
    public static Texture2D FlipTexture(Texture2D original)
    {
        int textureWidth = original.width;
        int textureHeight = original.height;

        Color[] colorArray = original.GetPixels();

        for (int j = 0; j < textureHeight; j++)
        {
            int rowStart = 0;
            int rowEnd = textureWidth - 1;

            while (rowStart < rowEnd)
            {
                Color hold = colorArray[(j * textureWidth) + (rowStart)];
                colorArray[(j * textureWidth) + (rowStart)] = colorArray[(j * textureWidth) + (rowEnd)];
                colorArray[(j * textureWidth) + (rowEnd)] = hold;
                rowStart++;
                rowEnd--;
            }
        }

        Texture2D finalFlippedTexture = new Texture2D(original.width, original.height);
        finalFlippedTexture.SetPixels(colorArray);
        finalFlippedTexture.Apply();

        return finalFlippedTexture;
    }

}
