using System;
using UnityEngine;

// Token: 0x0200000A RID: 10
public class EndingScript : MonoBehaviour
{
    // Token: 0x06000059 RID: 89 RVA: 0x00008F00 File Offset: 0x00007100
    private void Start()
    {
        this.whiteTexture = (Texture2D)Resources.Load("Images/whiteTexture", typeof(Texture2D));
        if (!this.timeTexture)
        {
            this.timeTexture = this.whiteTexture;
        }
        this.fontSizes = new Vector2(100f, 65.454544f) * (float)Screen.width / 1101f;
        Screen.lockCursor = false;
        Screen.showCursor = true;
        GameObject gameObject = GameObject.FindGameObjectWithTag("Audio");
        if (gameObject != null)
        {
            MyUnitySingleton component = gameObject.GetComponent<MyUnitySingleton>();
            this.seconds = new Vector2(component.playerTime, component.worldTime);
            component.PlayEndMusic();
        }
        while (this.seconds.x > 60f)
        {
            this.seconds.x = this.seconds.x - 60f;
            this.minutes.x = this.minutes.x + 1f;
            if (this.minutes.x > 60f)
            {
                this.minutes.x = this.minutes.x - 60f;
                this.hours.x = this.hours.x + 1f;
            }
        }
        while (this.seconds.y > 60f)
        {
            this.seconds.y = this.seconds.y - 60f;
            this.minutes.y = this.minutes.y + 1f;
            if (this.minutes.y > 60f)
            {
                this.minutes.y = this.minutes.y - 60f;
                this.hours.y = this.hours.y + 1f;
            }
        }
        this.times[0] = string.Concat(new string[]
        {
            this.hours.x.ToString("00"),
            ":",
            this.minutes.x.ToString("00"),
            ":",
            this.seconds.x.ToString("00.000")
        });
        this.times[1] = string.Concat(new string[]
        {
            this.hours.y.ToString("00"),
            ":",
            this.minutes.y.ToString("00"),
            ":",
            this.seconds.y.ToString("00.000")
        });
        this.size = new Vector2((float)Screen.width, (float)Screen.height);
        this.scale = new Vector2(this.size.x / 2560f, this.size.y / 1440f);
    }

    // Token: 0x0600005A RID: 90 RVA: 0x00009208 File Offset: 0x00007408
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.mouseDown = true;
            if (this.tweeting)
            {
                Screen.SetResolution((int)this.resolution.x, (int)this.resolution.y, true);
                this.tweeting = false;
            }
        }
        if (this.fadeOut)
        {
            this.alphaFadeValue -= Time.deltaTime;
            if (this.alphaFadeValue <= 0f)
            {
                Debug.Log("load level 5 -- EndingScript.Update/1");
                Application.LoadLevel(5);
            }
        }
        if (this.tweeting)
        {
            this.counter++;
            if (this.counter > 2 && this.counter < 5)
            {
                int num = UnityEngine.Random.Range(0, this.winTexts.Length);
                Application.OpenURL(this.URLtext + this.winTexts[num].Replace("%wintime", this.times[0]));
                this.counter = 6;
            }
        }
    }

    // Token: 0x0600005B RID: 91 RVA: 0x000092F0 File Offset: 0x000074F0
    private void OnGUI()
    {
        GUI.skin.label = this.myStyle;
        if (this.alphaFadeValue > 0f && !this.fadeOut)
        {
            GUI.depth = 2;
            this.alphaFadeValue -= Mathf.Clamp01(Time.deltaTime / 5f);
        }
        if (this.endScreen)
        {
            GUI.depth = 3;
            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1f);
            GUI.BeginGroup(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height));
            GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.whiteTexture);
            GUI.EndGroup();
            GUI.depth = 1;
            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1f - this.alphaFadeValue);
            GUI.BeginGroup(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height));
            GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.endTexture);
            GUI.EndGroup();
            if (this.mouseDown)
            {
                this.endScreen = false;
                this.alphaFadeValue = 1f;
                this.mouseDown = false;
                return;
            }
        }
        else
        {
            GUI.depth = 3;
            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1f);
            GUI.BeginGroup(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height));
            GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.whiteTexture);
            GUI.EndGroup();
            GUI.depth = 2;
            GUI.BeginGroup(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height));
            if (!this.fadeOut)
            {
                GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, this.alphaFadeValue);
                GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.endTexture);
                GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1f - this.alphaFadeValue);
            }
            else
            {
                GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, this.alphaFadeValue);
            }
            GUI.DrawTexture(new Rect(0f, 0f, this.size.x, this.size.y), this.timeTexture);
            GUI.EndGroup();
            GUI.depth = 1;
            GUI.skin.label.fontSize = (int)this.fontSizes.x;
            Vector2 vector = GUI.skin.label.CalcSize(new GUIContent(this.times[0]));
            GUI.Label(new Rect((float)Screen.width * 0.52f - vector.x / 2f, (float)Screen.height * 0.21f, (float)Screen.width, this.fontSizes.x), this.times[0]);
            GUI.skin.label.fontSize = (int)this.fontSizes.y;
            vector = GUI.skin.label.CalcSize(new GUIContent(this.times[1]));
            GUI.Label(new Rect((float)Screen.width * 0.52f - vector.x / 2f, (float)Screen.height * 0.5f, (float)Screen.width, this.fontSizes.y), this.times[1]);
            GUI.skin.button.normal.background = this.mainTextures[0];
            GUI.skin.button.hover.background = this.mainTextures[1];
            GUI.skin.button.active.background = this.mainTextures[0];
            if (GUI.Button(new Rect(1194f * this.scale.x, 1170f * this.scale.y, this.mainTextureSize.x * this.scale.x, this.mainTextureSize.y * this.scale.y), string.Empty))
            {
                Debug.Log("load level 1 -- EndingScript.OnGUI/1");
                Application.LoadLevel(1);
            }
            GUI.skin.button.normal.background = this.whatTextures[0];
            GUI.skin.button.hover.background = this.whatTextures[1];
            GUI.skin.button.active.background = this.whatTextures[0];
            if (GUI.Button(new Rect(1121f * this.scale.x, 1084f * this.scale.y, this.whatTextureSize.x * this.scale.x, this.whatTextureSize.y * this.scale.y), string.Empty))
            {
                this.fadeOut = true;
                this.alphaFadeValue = 1f;
            }
            GUI.skin.button.normal.background = this.twitterTextures[0];
            GUI.skin.button.hover.background = this.twitterTextures[1];
            GUI.skin.button.active.background = this.twitterTextures[0];
            if (GUI.Button(new Rect(1241f * this.scale.x, 890f * this.scale.y, this.twitterTextureSize.x * this.scale.x, this.twitterTextureSize.y * this.scale.y), string.Empty))
            {
                if (Application.platform == RuntimePlatform.OSXPlayer && Screen.fullScreen)
                {
                    this.resolution = new Vector2((float)Screen.currentResolution.width, (float)Screen.currentResolution.height);
                    this.tweeting = true;
                    Screen.SetResolution(640, 480, false);
                }
                else if (Application.platform != RuntimePlatform.OSXPlayer)
                {
                    int num = UnityEngine.Random.Range(0, this.winTexts.Length);
                    Application.OpenURL(this.URLtext + this.winTexts[num].Replace("%wintime", this.times[0]));
                }
                else if (Application.platform == RuntimePlatform.OSXPlayer && !Screen.fullScreen)
                {
                    int num2 = UnityEngine.Random.Range(0, this.winTexts.Length);
                    Application.OpenURL(this.URLtext + this.winTexts[num2].Replace("%wintime", this.times[0]));
                }
                this.counter = 0;
            }
        }
    }

    // Token: 0x040000ED RID: 237
    private float alphaFadeValue = 1f;

    // Token: 0x040000EE RID: 238
    public AsyncOperation loadOp;

    // Token: 0x040000EF RID: 239
    private bool endScreen = true;

    // Token: 0x040000F0 RID: 240
    private bool mouseDown;

    // Token: 0x040000F1 RID: 241
    private Texture2D whiteTexture;

    // Token: 0x040000F2 RID: 242
    public Texture2D endTexture;

    // Token: 0x040000F3 RID: 243
    public Texture2D timeTexture;

    // Token: 0x040000F4 RID: 244
    public Texture2D[] mainTextures;

    // Token: 0x040000F5 RID: 245
    public Texture2D[] whatTextures;

    // Token: 0x040000F6 RID: 246
    public Texture2D[] twitterTextures;

    // Token: 0x040000F7 RID: 247
    private Vector2 fontSizes = Vector2.zero;

    // Token: 0x040000F8 RID: 248
    private Vector2 seconds = Vector2.zero;

    // Token: 0x040000F9 RID: 249
    private Vector2 minutes = Vector2.zero;

    // Token: 0x040000FA RID: 250
    private Vector2 hours = Vector2.zero;

    // Token: 0x040000FB RID: 251
    private string[] times = new string[2];

    // Token: 0x040000FC RID: 252
    public GUIStyle myStyle;

    // Token: 0x040000FD RID: 253
    private Vector2 mainTextureSize = new Vector2(290f, 42f);

    // Token: 0x040000FE RID: 254
    private Vector2 whatTextureSize = new Vector2(437f, 42f);

    // Token: 0x040000FF RID: 255
    private Vector2 twitterTextureSize = new Vector2(182f, 83f);

    // Token: 0x04000100 RID: 256
    private Vector2 scale = Vector2.zero;

    // Token: 0x04000101 RID: 257
    private Vector2 size = Vector2.zero;

    // Token: 0x04000102 RID: 258
    private Vector2 resolution = Vector2.zero;

    // Token: 0x04000103 RID: 259
    private string[] winTexts = new string[]
    {
        "It+took+me+%wintime+to+slow+light+down+to+walking+speed.+Then+things+got+really+weird.+gamelab.mit.edu/slower+%23slowlight",
        "It+took+me+%wintime+to+get+the+speed+of+light+down+to+walking+speed.+The+universe+was+not+amused.+gamelab.mit.edu/slower+%23slowlight",
        "It+took+me+%wintime+to+be+able+to+walk+near+the+speed+of+light.+How%27s+that+for+powerwalking?+gamelab.mit.edu/slower+%23slowlight",
        "I+got+close+to+the+speed+of+light+in+%wintime,+and+I+never+even+broke+into+a+jog.+gamelab.mit.edu/slower+%23slowlight",
        "It+took+%wintime+for+me+to+get+the+speed+of+light+down+to+my+walking+speed.+Holy+colors,+Einstein!+gamelab.mit.edu/slower+%23slowlight",
        "I+slowed+light+to+a+brisk+walk+in+%wintime.+I%27m+getting+a+Ph.D.+in+theoretical+physics.+gamelab.mit.edu/slower+%23slowlight",
        "I+slowed+the+speed+of+light+down+to+my+speed+in+%wintime.+My+eyes+are+coated+in+rainbowsauce.+gamelab.mit.edu/slower+%23slowlight"
    };

    // Token: 0x04000104 RID: 260
    private string URLtext = "http://twitter.com/share?text=";

    // Token: 0x04000105 RID: 261
    private bool tweeting;

    // Token: 0x04000106 RID: 262
    private bool fadeOut;

    // Token: 0x04000107 RID: 263
    private int counter;
}
