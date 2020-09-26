using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000003 RID: 3
public class GUIScripts : MonoBehaviour
{
    // Token: 0x06000005 RID: 5 RVA: 0x000029E8 File Offset: 0x00000BE8
    private void Start()
    {
        this.state = base.GetComponent<GameState>();
        this.singleton = GameObject.FindGameObjectWithTag("Audio").GetComponent<MyUnitySingleton>();
        this.singleton.gameState = this.state;
        this.whiteTexture = (Texture2D)Resources.Load("Images/whiteTexture", typeof(Texture2D));
        this.blackTexture = (Texture2D)Resources.Load("Images/blackTexture", typeof(Texture2D));
        this.barSize = new Vector2((float)Screen.width, (float)(Screen.width * this.hudEmpty.height / this.hudEmpty.width)) * 1f;
        this.widthChange = 0.7f * (float)Screen.width / 1100f;
        this.lightArrowPos = new Vector2(0.958f * (float)Screen.width, this.barSize.y * 0.28f);
        this.lightArrowSize = new Vector2(this.barSize.y * 0.7f * (float)this.lightArrow.width / (float)this.lightArrow.height, this.barSize.y * 0.6f);
        this.playerArrowSize = new Vector2((float)Screen.width * 0.08f, (float)Screen.height * 0.01f);
        this.playerArrowPos = new Vector2(this.lightArrowPos.x - this.playerArrowSize.x * 0.98f, this.barSize.y * 0.85f);
        this.playerImageSize = new Vector2(this.barSize.x * 0.01f, (float)Screen.height * 0.04f);
        this.timeleft = this.updateInterval;
        this.numberPos = new Vector2(this.barSize.x * 0.042f, (float)Screen.height - this.barSize.y * 0.6f);
        this.numberSize = new Vector2((float)(150 * Screen.width) / this.originalMenuSize.x, (float)(150 * Screen.width) / this.originalMenuSize.x);
        this.totalNumberSize = new Vector2(10f * this.numberSize.x, 11f * this.numberSize.y);
        this.orbFadeValue = 0f;
        this.size = new Vector2((float)Screen.width, (float)Screen.height);
        if (Screen.width > this.optionsTexture.width)
        {
            this.size = new Vector2((float)this.optionsTexture.width, (float)this.optionsTexture.height);
        }
        this.difference = new Vector2(Mathf.Max((float)Screen.width - this.size.x, 0f), Mathf.Max((float)Screen.height - this.size.y, 0f)) / 2f;
        this.scale = new Vector2(this.size.x / this.originalMenuSize.x, this.size.y / this.originalMenuSize.y);
        this.resumeTextureSize = this.ScaleVector(this.resumeTextureSize);
        this.resumeTexturePos = this.ScaleVector(this.resumeTexturePos);
        this.menuTextureSize = this.ScaleVector(this.menuTextureSize);
        this.menuTexturePos = this.ScaleVector(this.menuTexturePos);
        this.checkBoxSize = this.ScaleVector(this.checkBoxSize);
        this.saturatedPos = this.ScaleVector(this.saturatedPos);
        this.deSaturatedPos = this.ScaleVector(this.deSaturatedPos);
        this.sliderBoxSize = this.ScaleVector(this.sliderBoxSize);
        this.volumeSliderPos = this.ScaleVector(this.volumeSliderPos);
        this.mouseSliderPos = this.ScaleVector(this.mouseSliderPos);
        this.sliderLength = 904f * this.scale.x - this.sliderBoxSize.x * 1.2f;
        this.fontSizes = new Vector2(100f * this.scale.x, 50f * this.scale.x);
        if (this.singleton)
        {
            this.mouseSensitivity = this.singleton.mouseSensitivity;
            this.volume = this.singleton.volume;
            this.saturated = this.singleton.saturated;
            this.unSaturated = !this.saturated;
        }
        this.volumeRect = new Rect(this.volume * this.sliderLength + this.volumeSliderPos.x, this.volumeSliderPos.y, this.sliderBoxSize.x, this.sliderBoxSize.y);
        this.mouseRect = new Rect(this.mouseSensitivity * this.sliderLength + this.mouseSliderPos.x, this.mouseSliderPos.y, this.sliderBoxSize.x, this.sliderBoxSize.y);
        this.times = new string[]
        {
            string.Empty,
            string.Empty
        };
        this.bgTex = new Texture2D[]
        {
            new Texture2D(1, 1),
            new Texture2D(1, 1)
        };
        this.bgTex[0].SetPixel(0, 0, new Color(0f, 0f, 0f, 0.4f));
        this.bgTex[0].Apply();
        this.bgTex[1].SetPixel(0, 0, new Color(0f, 0f, 0f, 0.7f));
        this.bgTex[1].Apply();
        this.keyStyle = new GUIStyle(this.myStyle);
        this.keyStyle.alignment = TextAnchor.MiddleCenter;
        this.keyStyle.fontSize = (int)((double)this.fontSizes.y / 1.5);
        this.keyStyle.normal.background = this.blackTexture;
        this.pressedKeyStyle = new GUIStyle(this.keyStyle);
        this.pressedKeyStyle.normal.background = this.whiteTexture;
        this.pressedKeyStyle.normal.textColor = Color.black;
        this.splitGreen = new Color(0f, 0.8f, 0.21176471f, 1f);
        this.splitRed = new Color(0.8f, 0.07058824f, 0f, 1f);

        this.route = new RouteWr20200921Short();
        this.SetRouteTo(this.route);
    }

    // Token: 0x06000006 RID: 6 RVA: 0x00003088 File Offset: 0x00001288
    private void Update()
    {
        Vector2 vector = new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y);
        vector -= this.difference;
        if (Input.GetMouseButtonDown(0))
        {
            if (this.volumeRect.Contains(vector))
            {
                this.volumeMove = true;
            }
            else if (this.mouseRect.Contains(vector))
            {
                this.mouseMove = true;
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (this.volumeMove)
            {
                this.volumeRect.x = vector.x - this.volumeRect.width / 2f;
                this.volumeRect.x = Mathf.Clamp(this.volumeRect.x, this.volumeSliderPos.x, this.volumeSliderPos.x + this.sliderLength);
                this.volume = Mathf.Max((this.volumeRect.x - this.volumeSliderPos.x) / this.sliderLength, 0.01f);
            }
            else if (this.mouseMove)
            {
                this.mouseRect.x = vector.x - this.mouseRect.width / 2f;
                this.mouseRect.x = Mathf.Clamp(this.mouseRect.x, this.mouseSliderPos.x, this.mouseSliderPos.x + this.sliderLength);
                this.mouseSensitivity = Mathf.Max((this.mouseRect.x - this.mouseSliderPos.x) / this.sliderLength, 0.01f);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            this.volumeMove = false;
            this.mouseMove = false;
        }
        if (this.loading)
        {
            this.loadFadeValue -= Time.deltaTime;
            if (this.loadFadeValue < 0f)
            {
                base.GetComponent<MovementScripts>().allowedToMove = true;
                this.loading = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            this.showTimer = !this.showTimer;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            this.showOrbs = !this.showOrbs;
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            this.loadFadeValue = 1.2f;
            this.loading = true;
            this.resetAfterTimeIs = this.state.TotalTimePlayer + 0.01;
        }
        if (this.resetAfterTimeIs > this.state.TotalTimePlayer && this.resetAfterTimeIs != 0.0)
        {
            Debug.Log("load level 3 -- GUIScripts.Update-Reset/1");
            Application.LoadLevelAsync(3);
            GameObject.FindGameObjectWithTag("Audio").GetComponent<MyUnitySingleton>().fadeOut = true;
        }
    }

    // Token: 0x06000007 RID: 7 RVA: 0x00003330 File Offset: 0x00001530
    private void OnGUI()
    {
        if (!this.state.MovementFrozen)
        {
            if (this.state.GameWin)
            {
                this.alphaFadeValue += Mathf.Clamp01(Time.deltaTime / 10f);
                this.orbFadeValue -= Mathf.Clamp01(Time.deltaTime / 5f);
                GUI.depth = 0;
                GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1f - this.alphaFadeValue);
                GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.whiteTexture);
                GameObject.FindGameObjectWithTag("Audio").GetComponent<MyUnitySingleton>().volume = this.volume;
                GameObject.FindGameObjectWithTag("Audio").GetComponent<MyUnitySingleton>().mouseSensitivity = this.mouseSensitivity;
                GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 0.5f);
                GUI.skin.label = this.myStyle;
                if (this.times[0] != string.Empty)
                {
                    GUI.skin.label.fontSize = (int)this.fontSizes.x;
                    Vector2 vector = GUI.skin.label.CalcSize(new GUIContent(this.times[0]));
                    GUI.Label(new Rect((float)Screen.width * 0.5f - vector.x / 2f, 100f * this.scale.y, vector.x, 100f), this.times[0]);
                    GUI.skin.label.fontSize = (int)this.fontSizes.y;
                    vector = GUI.skin.label.CalcSize(new GUIContent("All orbs collected!"));
                    GUI.Label(new Rect((float)Screen.width * 0.5f - vector.x / 2f, 50f * this.scale.y, vector.x, 50f), "All orbs collected!");
                }
                GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, this.alphaFadeValue);
            }
            else
            {
                if (this.loading)
                {
                    GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, this.loadFadeValue);
                    GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.loadingScreen);
                }
                GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, this.alphaFadeValue);
                GUI.skin.button.wordWrap = true;
                this.gamma = (float)this.state.SqrtOneMinusVSquaredCWDividedByCSquared;
                this.C = (float)this.state.SpeedOfLight;
                this.playerVel = (float)this.state.PlayerVelocity;
                float num = Mathf.Abs((float)this.state.MaxSpeed / this.C) * 75f;
                if (double.IsNaN((double)num) || num == 0f || num > 75f)
                {
                    num = 0.001f;
                }
                float num2 = Mathf.Abs(this.playerVel / (float)this.state.MaxSpeed) * 15f;
                if (double.IsNaN((double)num2) || num2 == 0f)
                {
                    num2 = 0.001f;
                }
                int num3 = (int)(this.gamma / (1f + this.playerVel / this.C) * 372f);
                int num4 = (int)(this.gamma / (1f - this.playerVel / this.C) * 767f);
                float width = (float)Mathf.Min(num4 - num3, this.hudEmpty.width) * this.widthChange;
                GUI.depth = 2;
                GUI.BeginGroup(new Rect(0f, (float)Screen.height - this.barSize.y, (float)Screen.width, this.barSize.y));
                GUI.DrawTexture(new Rect(0f, 0f, this.barSize.x, this.barSize.y), this.hudEmpty);
                GUI.EndGroup();
                GUI.depth = 2;
                GUI.BeginGroup(new Rect((float)(Screen.width / 2) - Mathf.Min((float)(num4 - 550) * this.widthChange, (float)(Screen.width / 2)), (float)Screen.height - this.barSize.y, width, this.barSize.y));
                GUI.DrawTexture(new Rect(-Mathf.Max((float)(Screen.width / 2) - (float)(num4 - 550) * this.widthChange, 0f), 0f, this.barSize.x, this.barSize.y), this.hud);
                GUI.EndGroup();
                GUI.depth = 1;
                GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, this.orbFadeValue / 3f);
                GUI.DrawTexture(new Rect(0f, (float)Screen.height - this.barSize.y, this.barSize.x, this.barSize.y), this.hudFlash);
                GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, this.alphaFadeValue);
                GUI.BeginGroup(new Rect(this.numberPos.x, this.numberPos.y, this.numberSize.x, this.numberSize.y));
                GUI.DrawTexture(new Rect(this.numberOffset.x, this.numberOffset.y, this.totalNumberSize.x, this.totalNumberSize.y), this.numbers);
                GUI.EndGroup();
                GUI.BeginGroup(new Rect(0f, (float)Screen.height - this.barSize.y, (float)Screen.width, this.barSize.y));
                GUI.depth = 1;
                GUIUtility.RotateAroundPivot(num2, new Vector2(this.playerArrowPos.x + this.playerArrowSize.x, this.playerArrowPos.y + this.playerArrowSize.y));
                GUI.DrawTexture(new Rect(this.playerArrowPos.x, this.playerArrowPos.y, this.playerArrowSize.x, this.playerArrowSize.y), this.playerArrow);
                GUIUtility.RotateAroundPivot(-num2, new Vector2(this.playerArrowPos.x + this.playerImageSize.x / 2f, this.playerArrowPos.y + this.playerImageSize.y / 2f));
                GUI.DrawTexture(new Rect(this.playerArrowPos.x, this.playerArrowPos.y - this.playerImageSize.y * 0.8f, this.playerImageSize.x, this.playerImageSize.y), this.playerImage);
                GUIUtility.RotateAroundPivot(num2, new Vector2(this.playerArrowPos.x + this.playerImageSize.x / 2f, this.playerArrowPos.y + this.playerImageSize.y / 2f));
                GUIUtility.RotateAroundPivot(-num2, new Vector2(this.playerArrowPos.x + this.playerArrowSize.x, this.playerArrowPos.y + this.playerArrowSize.y));
                GUIUtility.RotateAroundPivot(-num, new Vector2(this.playerArrowPos.x + this.playerArrowSize.x + this.lightArrowSize.x / 10f, this.playerArrowPos.y));
                GUI.DrawTexture(new Rect(this.lightArrowPos.x, this.lightArrowPos.y, this.lightArrowSize.x, this.lightArrowSize.y), this.lightArrow);
                GUIUtility.RotateAroundPivot(num, new Vector2(this.playerArrowPos.x + this.playerArrowSize.x + this.lightArrowSize.x / 10f, this.playerArrowPos.y));
                GUI.EndGroup();
                this.UpdateTmpTimes();
                this.timeleft -= Time.deltaTime;
                this.accum += Time.timeScale / Time.deltaTime;
                this.frames++;
                if ((double)this.timeleft <= 0.0)
                {
                    this.curFPS = this.accum / (float)this.frames;
                    this.timeleft = this.updateInterval;
                    this.accum = 0f;
                    this.frames = 0;
                }
                float num5 = this.alphaFadeValue;
                if (this.orbFadeValue >= 0f)
                {
                    this.orbFadeValue -= Mathf.Clamp01(Time.deltaTime);
                }
            }
            GUI.BeginGroup(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height));
            int p = 10;
            RectOffset padding = new RectOffset(p, p, p, 0);
            float fontSize = this.fontSizes.y / 1.75f;
            GUI.skin.box.padding = padding;
            Vector2 testSize = GUI.skin.box.CalcSize(new GUIContent("test"));
            float maxY = (float)Screen.height - this.barSize.y;
            float currY = (float)this.wrSplits.Length * testSize.y;
            float scale;
            if (currY > maxY)
            {
                scale = maxY / currY;
                fontSize *= scale;
                p = (int)((float)p * scale);
                padding = new RectOffset(p, p, p, 0);
            }
            else
            {
                scale = 1f;
            }
            GUI.skin.box.normal.background = this.bgTex[1];
            GUI.skin.box.fontSize = (int)fontSize;
            GUI.skin.box.font = this.myStyle.font;
            GUI.skin.label = new GUIStyle(this.myStyle);
            GUI.skin.label.fontSize = (int)fontSize;
            GUI.skin.label.padding = padding;
            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1f);
            Vector2 vecSplits = GUI.skin.box.CalcSize(new GUIContent("test"));
            Vector2 vecTimerDs = GUI.skin.label.CalcSize(new GUIContent("Time: 000.000"));
            float col2Offset = GUI.skin.label.CalcSize(new GUIContent("-000.000")).x;
            if (this.showTimer)
            {
                float timerBoxWidth = 400f * scale;
                string timeStr = this.state.TotalTimePlayer.ToString("Time: 000.000");
                string speedStr = this.state.playerVelocity.ToString("Speed: 00.000");
                GUI.Box(new Rect(0f, 0f, vecTimerDs.x + 10f * scale, vecTimerDs.y), new GUIContent(timeStr));
                GUI.Box(new Rect(0f, 0f + vecTimerDs.y, vecTimerDs.x + 10f * scale, vecTimerDs.y), new GUIContent(speedStr));
                GUI.skin.box.alignment = TextAnchor.MiddleRight;
                GUI.Box(new Rect((float)Screen.width - timerBoxWidth, 0f, timerBoxWidth, vecSplits.y), new GUIContent("Time"));
                GUI.skin.box.normal.background = null;
                GUI.Box(new Rect((float)Screen.width - col2Offset, 0f, vecSplits.x, vecSplits.y), new GUIContent("+/-"));
                GUI.skin.box.alignment = TextAnchor.MiddleLeft;
                bool doneOnePreview = false;
                for (int s = 0; s < this.splitOn.Length; s++)
                {
                    GUI.skin.box.normal.background = this.bgTex[s % 2];

                    double tempTime = this.state.orbToSplit.ContainsKey(this.splitOn[s]) ? this.state.orbToSplit[this.splitOn[s]] : 0.0;
                    GUI.Box(new Rect((float)Screen.width - timerBoxWidth, vecSplits.y * (float)(s + 1), timerBoxWidth, vecSplits.y), new GUIContent(this.splitNames[s]));
                    if (tempTime > 0.0 || !doneOnePreview)
                    {
                        if (tempTime == 0.0 && !doneOnePreview)
                        {
                            doneOnePreview = true;
                            tempTime = this.state.TotalTimePlayer;
                        }
                        GUI.skin.box.normal.background = null;
                        string splitTime = tempTime.ToString("F03");
                        vecSplits = GUI.skin.box.CalcSize(new GUIContent(splitTime));
                        float plusMinusSplitTime = (float)tempTime - this.wrSplits[s];
                        string pmSplit = ((plusMinusSplitTime < 0f) ? "" : "+") + plusMinusSplitTime.ToString("F02");
                        Vector2 vecPMSplit = GUI.skin.box.CalcSize(new GUIContent(pmSplit));
                        GUI.Label(new Rect((float)Screen.width - vecSplits.x, vecSplits.y * (float)(s + 1), vecSplits.x, vecSplits.y), splitTime);
                        Color color = GUI.color;
                        GUI.color = ((plusMinusSplitTime < 0f) ? Color.green : Color.red);
                        GUI.Label(new Rect((float)Screen.width - vecSplits.x - col2Offset, vecSplits.y * (float)(s + 1), vecPMSplit.x, vecPMSplit.y), pmSplit);
                        GUI.color = color;
                    }
                }
            }
            GUI.color = Color.white;
            GUI.skin.box.fontSize = (int)(this.fontSizes.y * 0.5f);
            GUI.skin.box.normal.background = this.bgTex[1];
            float orbsDisplayWidth = 0f;
            if (this.showOrbs)
            {
                orbsDisplayWidth = 400f;
                GUI.skin.box.padding = new RectOffset(0, 0, 0, 0);
                Vector2 boxTxtSize = GUI.skin.box.CalcSize(new GUIContent("00"));
                GUI.skin.box.padding = new RectOffset((int)(Math.Max(0f, GUIScripts.defaultOrbBoxSize - boxTxtSize.x) / 2f), 0, (int)((GUIScripts.defaultOrbBoxSize - boxTxtSize.y) / 2f), 0);
                GUI.BeginGroup(new Rect(10f, 100f, orbsDisplayWidth, 400f));
                this.DrawRecentOrbs();
                GUI.EndGroup();
                GUI.skin.box.padding = new RectOffset(0, 0, 0, 0);
                boxTxtSize = GUI.skin.box.CalcSize(new GUIContent("@"));
                GUI.skin.box.padding = new RectOffset((int)((GUIScripts.defaultOrbBoxSize - boxTxtSize.x) / 2f), 0, (int)((GUIScripts.defaultOrbBoxSize - boxTxtSize.y) / 2f), 0);
                GUI.BeginGroup(new Rect(10f, 510f, orbsDisplayWidth, 400f));
                this.DrawOrbsTotal();
                GUI.EndGroup();
            }
            GUI.EndGroup();
            float keyOffset = this.keyDimensions + this.keyPadding;
            GUI.skin.label.fontSize = (int)(this.fontSizes.y * 1f);
            GUI.BeginGroup(new Rect((this.showOrbs ? orbsDisplayWidth : 0f) + 20f, (float)Screen.height - this.barSize.y - 2f * this.keyDimensions - 2.5f * this.keyPadding, 3f * keyOffset, 2f * keyOffset));
            float halfKeyPadding = this.keyPadding / 2f;
            GUI.Box(new Rect(0f, keyOffset, keyOffset, keyOffset), this.bgTex[0]);
            GUI.Box(new Rect(keyOffset, keyOffset, keyOffset, keyOffset), this.bgTex[0]);
            GUI.Box(new Rect(keyOffset * 2f, keyOffset, keyOffset, keyOffset), this.bgTex[0]);
            GUI.Box(new Rect(keyOffset, 0f, keyOffset, keyOffset), this.bgTex[0]);
            GUI.BeginGroup(new Rect(halfKeyPadding, halfKeyPadding, 3f * keyOffset - this.keyPadding, 2f * keyOffset - this.keyPadding));
            GUI.Button(new Rect(0f, keyOffset, this.keyDimensions, this.keyDimensions), "A", Input.GetKey(KeyCode.A) ? this.pressedKeyStyle : this.keyStyle);
            GUI.Button(new Rect(keyOffset * 1f, keyOffset, this.keyDimensions, this.keyDimensions), "S", Input.GetKey(KeyCode.S) ? this.pressedKeyStyle : this.keyStyle);
            GUI.Button(new Rect(keyOffset * 2f, keyOffset, this.keyDimensions, this.keyDimensions), "D", Input.GetKey(KeyCode.D) ? this.pressedKeyStyle : this.keyStyle);
            GUI.Button(new Rect(keyOffset, 0f, this.keyDimensions, this.keyDimensions), "W", Input.GetKey(KeyCode.W) ? this.pressedKeyStyle : this.keyStyle);
            GUI.EndGroup();
            GUI.EndGroup();
        }
        else
        {
            GUI.depth = 2;
            GUI.BeginGroup(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height));
            GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.blackTexture);
            GUI.DrawTexture(new Rect(this.difference.x, this.difference.y, this.size.x, this.size.y), this.optionsTexture);
            GUI.EndGroup();
            GUI.depth = 1;
            GUI.BeginGroup(new Rect(this.difference.x, this.difference.y, this.size.x, this.size.y));
            GUI.skin.button.normal.background = this.resumeTextures[0];
            GUI.skin.button.hover.background = this.resumeTextures[1];
            GUI.skin.button.active.background = this.resumeTextures[0];
            if (GUI.Button(new Rect(this.resumeTexturePos.x, this.resumeTexturePos.y, this.resumeTextureSize.x, this.resumeTextureSize.y), string.Empty))
            {
                this.state.ChangeState();
            }
            GUI.skin.button.normal.background = this.menuTextures[0];
            GUI.skin.button.hover.background = this.menuTextures[1];
            GUI.skin.button.active.background = this.menuTextures[0];
            if (GUI.Button(new Rect(this.menuTexturePos.x, this.menuTexturePos.y, this.menuTextureSize.x, this.menuTextureSize.y), string.Empty))
            {
                Debug.Log("load level 1 -- GUIScripts OnGUI");
                Application.LoadLevel(1);
            }
            GUI.DrawTexture(this.volumeRect, this.sliderBox);
            GUI.DrawTexture(this.mouseRect, this.sliderBox);
            this.saturated = GUI.Toggle(new Rect(this.saturatedPos.x, this.saturatedPos.y, this.checkBoxSize.x, this.checkBoxSize.y), !this.unSaturated, string.Empty);
            this.unSaturated = GUI.Toggle(new Rect(this.deSaturatedPos.x, this.deSaturatedPos.y, this.checkBoxSize.x, this.checkBoxSize.y), !this.saturated, string.Empty);
            GUI.EndGroup();
            this.saturated = !this.unSaturated;
            if (base.GetComponentInChildren<ColorCorrectionEffect>())
            {
                base.GetComponentInChildren<ColorCorrectionEffect>().enabled = this.unSaturated;
            }
            if (base.GetComponent<MovementScripts>())
            {
                base.GetComponent<MovementScripts>().mouseSensitivity = this.mouseSensitivity;
            }
            if (GameObject.FindGameObjectWithTag("Playermesh"))
            {
                GameObject.FindGameObjectWithTag("Playermesh").GetComponent<AudioScripts>().volume = this.volume;
            }
            if (GameObject.FindGameObjectWithTag("Audio"))
            {
                GameObject.FindGameObjectWithTag("Audio").GetComponent<MyUnitySingleton>().mouseSensitivity = this.mouseSensitivity;
                GameObject.FindGameObjectWithTag("Audio").GetComponent<MyUnitySingleton>().volume = this.volume;
                GameObject.FindGameObjectWithTag("Audio").GetComponent<MyUnitySingleton>().saturated = this.saturated;
            }
        }
        if (this.unset && this.frames > 5)
        {
            GUI.skin.toggle.normal.background = this.checkBoxTextures[0];
            GUI.skin.toggle.hover.background = this.checkBoxTextures[0];
            GUI.skin.toggle.active.background = this.checkBoxTextures[1];
            GUI.skin.toggle.onNormal.background = this.checkBoxTextures[1];
            GUI.skin.toggle.onHover.background = this.checkBoxTextures[1];
            GUI.skin.toggle.onActive.background = this.checkBoxTextures[0];
            this.frames = 0;
            this.unset = false;
        }
        if (this.unset)
        {
            this.frames++;
        }
    }

    // Token: 0x06000008 RID: 8 RVA: 0x000049B8 File Offset: 0x00002BB8
    public void OrbCollision()
    {
        this.orbFadeValue = 3f;
        this.numberOffset.x = this.numberOffset.x - this.numberSize.x;
        if (-this.numberOffset.x >= this.totalNumberSize.x)
        {
            if (this.numberOffset.y == 0f)
            {
                this.numberPos = new Vector2(this.barSize.x * 0.035f, (float)Screen.height - this.barSize.y * 0.65f);
            }
            this.numberOffset = new Vector2(0f, this.numberOffset.y - this.numberSize.y);
        }
        this.regularOrbs++;
        if (this.regularOrbs == 100)
        {
            this.alphaFadeValue = 0f;
        }
    }

    // Token: 0x06000009 RID: 9 RVA: 0x0000223C File Offset: 0x0000043C
    private Vector2 ScaleVector(Vector2 input)
    {
        return new Vector2(input.x * this.scale.x, input.y * this.scale.y);
    }

    // Token: 0x0600000A RID: 10 RVA: 0x00004A9C File Offset: 0x00002C9C
    public void GetTimes()
    {
        if (GameObject.FindGameObjectWithTag("Audio") != null)
        {
            this.seconds = new Vector2((float)this.state.TotalTimePlayer, (float)this.state.TotalTimeWorld);
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
            this.seconds.x.ToString("00")
        });
        this.times[1] = string.Concat(new string[]
        {
            this.hours.y.ToString("00"),
            ":",
            this.minutes.y.ToString("00"),
            ":",
            this.seconds.y.ToString("00")
        });
    }

    // Token: 0x0600000B RID: 11 RVA: 0x00004CF0 File Offset: 0x00002EF0
    public void UpdateTmpTimes()
    {
        this.tmpTime = ((float)this.state.TotalTimePlayer).ToString("000.000");
        string[] array = new string[100];
        for (int i = 0; i < this.state.orbState.Length; i++)
        {
            array[i] = (this.state.orbState[i] ? "_@_" : "___");
            if (i % 10 == 0)
            {
                string[] array2 = array;
                int num = i;
                array2[num] = "\n_" + (i / 10).ToString() + "_:" + array2[num];
            }
        }
        this.orbDisplay = "____0_,_1_,_2_,_3_,_4_,_5_,_6_,_7_,_8_,_9_" + string.Join(",", array);
    }

    // Token: 0x0600000C RID: 12 RVA: 0x00002267 File Offset: 0x00000467
    public void DrawRecentOrbs()
    {
        this.DrawRecentOrbs(35f, 5f);
    }

    // Token: 0x0600000D RID: 13 RVA: 0x00002279 File Offset: 0x00000479
    public void DrawOrbsTotal()
    {
        this.DrawOrbsTotal(35f, 5f);
    }

    // Token: 0x0600000E RID: 14 RVA: 0x00004DA4 File Offset: 0x00002FA4
    public void DrawRecentOrbs(float boxSize, float boxOffset)
    {
        new List<int>(this.state.orbCollectionList).Reverse();
        float num = boxSize + boxOffset;
        for (int i = 0; i < this.state.orbCollectionList.Count; i++)
        {
            Vector2 vector = new Vector2((float)(i % 10), (float)((i - i % 10) / 10));
            GUI.Box(new Rect(vector.x * num, vector.y * num, boxSize, boxSize), new GUIContent(this.state.orbCollectionList[i].ToString("D02")));
        }
    }

    // Token: 0x0600000F RID: 15 RVA: 0x00004E3C File Offset: 0x0000303C
    public void DrawOrbsTotal(float boxSize, float boxOffset)
    {
        float num = boxSize + boxOffset;
        for (int i = 0; i < this.state.orbState.Length; i++)
        {
            Vector2 vector = new Vector2((float)(i % 10), (float)((i - i % 10) / 10));
            GUIContent content = new GUIContent(this.state.orbState[i] ? "@" : " ");
            GUI.Box(new Rect(vector.x * num, vector.y * num, boxSize, boxSize), content);
        }
    }

    public void SetRouteTo(Route route)
    {
        this.SetRouteTo(route, true);
    }

    public void SetRouteTo(Route route, bool reset)
    {
        this.route = route;
        if (reset)
        {
            this.ResetSplits();
        }
    }

    public void ResetSplits()
    {
        this.splitOn = this.route.SplitOn();
        this.splitNames = this.route.SplitNames();
        this.wrSplits = this.route.BenchmarkSplits();
    }


    // Token: 0x04000001 RID: 1
    private AsyncOperation loadOp;

    // Token: 0x04000002 RID: 2
    private Vector2 barSize;

    // Token: 0x04000003 RID: 3
    public Texture2D loadingScreen;

    // Token: 0x04000004 RID: 4
    public Texture2D hud;

    // Token: 0x04000005 RID: 5
    public Texture2D hudEmpty;

    // Token: 0x04000006 RID: 6
    public Texture2D lightArrow;

    // Token: 0x04000007 RID: 7
    public Texture2D playerArrow;

    // Token: 0x04000008 RID: 8
    public Texture2D playerImage;

    // Token: 0x04000009 RID: 9
    public Texture2D hudFlash;

    // Token: 0x0400000A RID: 10
    public Texture2D numbers;

    // Token: 0x0400000B RID: 11
    private Texture2D whiteTexture;

    // Token: 0x0400000C RID: 12
    private Texture2D blackTexture;

    // Token: 0x0400000D RID: 13
    public int orbCount;

    // Token: 0x0400000E RID: 14
    public int infraredCount;

    // Token: 0x0400000F RID: 15
    public int infraredOrbs;

    // Token: 0x04000010 RID: 16
    public int regularOrbs;

    // Token: 0x04000011 RID: 17
    public float updateInterval = 0.5f;

    // Token: 0x04000012 RID: 18
    private float accum;

    // Token: 0x04000013 RID: 19
    private int frames;

    // Token: 0x04000014 RID: 20
    private float timeleft;

    // Token: 0x04000015 RID: 21
    public float curFPS;

    // Token: 0x04000016 RID: 22
    private float gamma;

    // Token: 0x04000017 RID: 23
    private float C;

    // Token: 0x04000018 RID: 24
    private float playerVel;

    // Token: 0x04000019 RID: 25
    private float widthChange;

    // Token: 0x0400001A RID: 26
    private Vector2 lightArrowSize = Vector2.zero;

    // Token: 0x0400001B RID: 27
    private Vector2 lightArrowPos = Vector2.zero;

    // Token: 0x0400001C RID: 28
    private Vector2 playerArrowSize = Vector2.zero;

    // Token: 0x0400001D RID: 29
    private Vector2 playerArrowPos = Vector2.zero;

    // Token: 0x0400001E RID: 30
    private Vector2 playerImageSize = Vector2.zero;

    // Token: 0x0400001F RID: 31
    private Vector2 numberSize = Vector2.zero;

    // Token: 0x04000020 RID: 32
    private Vector2 numberOffset = Vector2.zero;

    // Token: 0x04000021 RID: 33
    private Vector2 totalNumberSize = Vector2.zero;

    // Token: 0x04000022 RID: 34
    private Vector2 numberPos = Vector2.zero;

    // Token: 0x04000023 RID: 35
    private float alphaFadeValue = 1f;

    // Token: 0x04000024 RID: 36
    public float orbFadeValue;

    // Token: 0x04000025 RID: 37
    private Vector2 orbPos;

    // Token: 0x04000026 RID: 38
    private Vector2 orbSize;

    // Token: 0x04000027 RID: 39
    public Texture2D optionsTexture;

    // Token: 0x04000028 RID: 40
    private Vector2 originalMenuSize = new Vector2(2560f, 1440f);

    // Token: 0x04000029 RID: 41
    private Vector2 difference = Vector2.zero;

    // Token: 0x0400002A RID: 42
    private Vector2 size = Vector2.zero;

    // Token: 0x0400002B RID: 43
    private Vector2 scale = Vector2.zero;

    // Token: 0x0400002C RID: 44
    public Texture2D[] resumeTextures;

    // Token: 0x0400002D RID: 45
    public Texture2D[] menuTextures;

    // Token: 0x0400002E RID: 46
    private Vector2 resumeTextureSize = new Vector2(212f, 83f);

    // Token: 0x0400002F RID: 47
    private Vector2 menuTextureSize = new Vector2(303f, 85f);

    // Token: 0x04000030 RID: 48
    private Vector2 resumeTexturePos = new Vector2(1541f, 980f);

    // Token: 0x04000031 RID: 49
    private Vector2 menuTexturePos = new Vector2(1493f, 1084f);

    // Token: 0x04000032 RID: 50
    public Texture2D[] checkBoxTextures;

    // Token: 0x04000033 RID: 51
    private Vector2 checkBoxSize = new Vector2(110f, 110f);

    // Token: 0x04000034 RID: 52
    private Vector2 saturatedPos = new Vector2(912f, 891f);

    // Token: 0x04000035 RID: 53
    private Vector2 deSaturatedPos = new Vector2(1183f, 891f);

    // Token: 0x04000036 RID: 54
    public Texture2D sliderBox;

    // Token: 0x04000037 RID: 55
    private Vector2 sliderBoxSize = new Vector2(65f, 65f);

    // Token: 0x04000038 RID: 56
    private Vector2 volumeSliderPos = new Vector2(970f, 570.5f);

    // Token: 0x04000039 RID: 57
    private Vector2 mouseSliderPos = new Vector2(970f, 715f);

    // Token: 0x0400003A RID: 58
    private float sliderLength;

    // Token: 0x0400003B RID: 59
    private Rect volumeRect;

    // Token: 0x0400003C RID: 60
    private Rect mouseRect;

    // Token: 0x0400003D RID: 61
    public float volume = 1f;

    // Token: 0x0400003E RID: 62
    public float mouseSensitivity = 0.5f;

    // Token: 0x0400003F RID: 63
    private bool volumeMove;

    // Token: 0x04000040 RID: 64
    private bool mouseMove;

    // Token: 0x04000041 RID: 65
    private bool saturated = true;

    // Token: 0x04000042 RID: 66
    private bool unSaturated;

    // Token: 0x04000043 RID: 67
    private bool unset = true;

    // Token: 0x04000044 RID: 68
    private bool loading = true;

    // Token: 0x04000045 RID: 69
    private float loadFadeValue = 1f;

    // Token: 0x04000046 RID: 70
    private Vector2 fontSizes = Vector2.zero;

    // Token: 0x04000047 RID: 71
    private Vector2 seconds = Vector2.zero;

    // Token: 0x04000048 RID: 72
    private Vector2 minutes = Vector2.zero;

    // Token: 0x04000049 RID: 73
    private Vector2 hours = Vector2.zero;

    // Token: 0x0400004A RID: 74
    private string[] times;

    // Token: 0x0400004B RID: 75
    public GUIStyle myStyle;

    // Token: 0x0400004C RID: 76
    private GameState state;

    // Token: 0x0400004D RID: 77
    private string tmpTime;

    // Token: 0x0400004E RID: 78
    private bool showTimer = true;

    // Token: 0x0400004F RID: 79
    private string orbDisplay = string.Empty;

    // Token: 0x04000050 RID: 80
    private bool showOrbs;

    // Token: 0x04000051 RID: 81
    private Texture2D[] bgTex;

    // Token: 0x04000052 RID: 82
    private MyUnitySingleton singleton;

    // Token: 0x04000053 RID: 83
    private double resetAfterTimeIs;

    // Token: 0x04000054 RID: 84
    private float keyDimensions = 40f;

    // Token: 0x04000055 RID: 85
    private float keyPadding = 10f;

    // Token: 0x04000056 RID: 86
    private GUIStyle keyStyle;

    // Token: 0x04000057 RID: 87
    private GUIStyle pressedKeyStyle;

    // Token: 0x04000058 RID: 88
    private Color splitRed;

    // Token: 0x04000059 RID: 89
    private Color splitGreen;

    // Token: 0x0400005A RID: 90
    public static float defaultOrbBoxSize = 35f;

    // Token: 0x0400052E RID: 1326
    public int[] splitOn;

    // Token: 0x040008F4 RID: 2292
    public string[] splitNames;

    // Token: 0x04001B1F RID: 6943
    public float[] wrSplits;

    public Route route;


}
