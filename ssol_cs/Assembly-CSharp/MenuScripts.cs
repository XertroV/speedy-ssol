using System;
using UnityEngine;

// Token: 0x02000006 RID: 6
public class MenuScripts : MonoBehaviour
{
	// Token: 0x0600003B RID: 59 RVA: 0x00006090 File Offset: 0x00004290
	private void Start()
	{
		Screen.showCursor = true;
		Screen.lockCursor = false;
		Texture2D texture2D = (Texture2D)Resources.Load("Images/blackTexture", typeof(Texture2D));
		if (!this.menuTexture)
		{
			this.menuTexture = texture2D;
		}
		this.blackTexture = texture2D;
		this.size = new Vector2((float)Screen.width, Mathf.Min((float)Screen.width * 2560f / 1440f, (float)Screen.height));
		if (Screen.width > this.menuTexture.width)
		{
			this.size = new Vector2((float)this.menuTexture.width, (float)this.menuTexture.height);
			this.difference = new Vector2((float)Screen.width - this.size.x, (float)Screen.height - this.size.y) / 2f;
		}
		this.difference = new Vector2(Mathf.Max((float)Screen.width - this.size.x, 0f), Mathf.Max((float)Screen.height - this.size.y, 0f)) / 2f;
		this.scale = new Vector2(this.size.x / this.originalMenuSize.x, this.size.y / this.originalMenuSize.y);
		this.menuTextureSize = this.ScaleVector(this.menuTextureSize);
		this.menuTexturePos = this.ScaleVector(this.menuTexturePos);
		this.checkBoxSize = this.ScaleVector(this.checkBoxSize);
		this.saturatedPos = this.ScaleVector(this.saturatedPos);
		this.deSaturatedPos = this.ScaleVector(this.deSaturatedPos);
		this.sliderBoxSize = this.ScaleVector(this.sliderBoxSize);
		this.volumeSliderPos = this.ScaleVector(this.volumeSliderPos);
		this.mouseSliderPos = this.ScaleVector(this.mouseSliderPos);
		this.playTexturePos = this.ScaleVector(this.playTexturePos);
		this.creditTexturePos = this.ScaleVector(this.creditTexturePos);
		this.quitTexturePos = this.ScaleVector(this.quitTexturePos);
		this.optionTexturePos = this.ScaleVector(this.optionTexturePos);
		this.playTextureSize = this.ScaleVector(this.playTextureSize);
		this.creditTextureSize = this.ScaleVector(this.creditTextureSize);
		this.quitTextureSize = this.ScaleVector(this.quitTextureSize);
		this.optionTextureSize = this.ScaleVector(this.optionTextureSize);
		this.sliderLength = 904f * this.scale.x - this.sliderBoxSize.x * 1.2f;
		GameObject gameObject = GameObject.FindGameObjectWithTag("Audio");
		if (gameObject)
		{
			MyUnitySingleton component = gameObject.GetComponent<MyUnitySingleton>();
			this.mouseSensitivity = component.mouseSensitivity;
			this.volume = component.volume;
		}
		this.volumeRect = new Rect(this.volume * this.sliderLength + this.volumeSliderPos.x, this.volumeSliderPos.y, this.sliderBoxSize.x, this.sliderBoxSize.y);
		this.mouseRect = new Rect(this.mouseSensitivity * this.sliderLength + this.mouseSliderPos.x, this.mouseSliderPos.y, this.sliderBoxSize.x, this.sliderBoxSize.y);
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00006408 File Offset: 0x00004608
	private void Update()
	{
		Vector2 vector = new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y);
		vector -= this.difference;
		if (Input.GetMouseButtonDown(0))
		{
			this.mouseDown = true;
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
		if (this.fadeOut && this.alphaFadeValue <= 0f)
		{
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				Debug.Log("loading level 3 b/c shift key being pressed - MenuScripts.Update");
				GameObject.FindGameObjectWithTag("Audio").GetComponent<MyUnitySingleton>().fadeOut = true;
				Application.LoadLevel(3);
			}
			else
			{
				Debug.Log("loading level 2 - MenuScripts.Update");
				Application.LoadLevel(2);
			}
		}
		if (this.fadeIn)
		{
			this.startFadeValue += Time.deltaTime / 2f;
			if (this.startFadeValue >= 1f)
			{
				this.fadeIn = false;
			}
		}
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00006668 File Offset: 0x00004868
	private void OnGUI()
	{
		if (!this.options)
		{
			if (this.alphaFadeValue > 0f)
			{
				this.alphaFadeValue -= Mathf.Clamp01(Time.deltaTime / 2f);
			}
			GUI.skin.button.wordWrap = true;
			GUI.depth = 2;
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1f);
			if (this.fadeOut)
			{
				GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1f - this.alphaFadeValue);
				GUI.depth = 0;
				GUI.BeginGroup(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height));
				GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.loadTexture);
				GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, this.alphaFadeValue);
				GUI.EndGroup();
			}
			GUI.BeginGroup(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height));
			GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.blackTexture);
			if (this.fadeIn)
			{
				GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, this.startFadeValue);
			}
			GUI.DrawTexture(new Rect(this.difference.x, this.difference.y, this.size.x, this.size.y), this.menuTexture);
			GUI.EndGroup();
			GUI.depth = 1;
			GUI.BeginGroup(new Rect(this.difference.x, this.difference.y, this.size.x, this.size.y));
			GUI.skin.button.normal.background = this.playTextures[0];
			GUI.skin.button.hover.background = this.playTextures[1];
			GUI.skin.button.active.background = this.playTextures[0];
			if (GUI.Button(new Rect(this.playTexturePos.x, this.playTexturePos.y, this.playTextureSize.x, this.playTextureSize.y), string.Empty) && !this.credits && this.alphaFadeValue <= 0f && this.mouseDown)
			{
				this.alphaFadeValue = 1f;
				this.mouseDown = false;
				this.fadeOut = true;
			}
			GUI.skin.button.normal.background = this.quitTextures[0];
			GUI.skin.button.hover.background = this.quitTextures[1];
			GUI.skin.button.active.background = this.quitTextures[0];
			if (GUI.Button(new Rect(this.quitTexturePos.x, this.quitTexturePos.y, this.quitTextureSize.x, this.quitTextureSize.y), string.Empty) && !this.credits && this.alphaFadeValue <= 0f && this.mouseDown)
			{
				Application.Quit();
				this.mouseDown = false;
			}
			GUI.skin.button.normal.background = this.creditTextures[0];
			GUI.skin.button.hover.background = this.creditTextures[1];
			GUI.skin.button.active.background = this.creditTextures[0];
			if (GUI.Button(new Rect(this.creditTexturePos.x, this.creditTexturePos.y, this.creditTextureSize.x, this.creditTextureSize.y), string.Empty) && !this.credits && this.alphaFadeValue <= 0f && this.mouseDown)
			{
				this.credits = true;
				this.mouseDown = false;
			}
			GUI.skin.button.normal.background = this.optionTextures[0];
			GUI.skin.button.hover.background = this.optionTextures[1];
			GUI.skin.button.active.background = this.optionTextures[0];
			if (GUI.Button(new Rect(this.optionTexturePos.x, this.optionTexturePos.y, this.optionTextureSize.x, this.optionTextureSize.y), string.Empty) && !this.credits && this.alphaFadeValue <= 0f && this.mouseDown)
			{
				this.options = true;
				this.mouseDown = false;
			}
			GUI.EndGroup();
			if (this.credits)
			{
				GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1f - this.alphaFadeValue);
				GUI.BeginGroup(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height));
				GUI.DrawTexture(new Rect(this.difference.x, this.difference.y, this.size.x, this.size.y), this.creditTexture);
				GUI.EndGroup();
			}
			else if (this.fadeOut)
			{
				GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, this.alphaFadeValue);
			}
		}
		else
		{
			GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.blackTexture);
			GUI.depth = 2;
			GUI.BeginGroup(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height));
			GUI.DrawTexture(new Rect(this.difference.x, this.difference.y, this.size.x, this.size.y), this.optionsTexture);
			GUI.EndGroup();
			GUI.depth = 1;
			GUI.BeginGroup(new Rect(this.difference.x, this.difference.y, this.size.x, this.size.y));
			GUI.skin.button.normal.background = this.menuTextures[0];
			GUI.skin.button.hover.background = this.menuTextures[1];
			GUI.skin.button.active.background = this.menuTextures[0];
			if (GUI.Button(new Rect(this.menuTexturePos.x, this.menuTexturePos.y, this.menuTextureSize.x, this.menuTextureSize.y), string.Empty))
			{
				this.options = false;
				this.mouseDown = false;
			}
			if (this.unset)
			{
				this.mouseSensitivity = GameObject.FindGameObjectWithTag("Audio").GetComponent<MyUnitySingleton>().mouseSensitivity;
				this.volume = GameObject.FindGameObjectWithTag("Audio").GetComponent<MyUnitySingleton>().volume;
				GUI.skin.toggle.normal.background = this.checkBoxTextures[0];
				GUI.skin.toggle.hover.background = this.checkBoxTextures[0];
				GUI.skin.toggle.active.background = this.checkBoxTextures[1];
				GUI.skin.toggle.onNormal.background = this.checkBoxTextures[1];
				GUI.skin.toggle.onHover.background = this.checkBoxTextures[1];
				GUI.skin.toggle.onActive.background = this.checkBoxTextures[0];
				this.unset = false;
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
			if (GameObject.FindGameObjectWithTag("Playermesh"))
			{
				GameObject.FindGameObjectWithTag("Playermesh").GetComponent<AudioScripts>().volume = this.volume;
				base.GetComponent<MovementScripts>().mouseSensitivity = this.mouseSensitivity;
			}
			if (GameObject.FindGameObjectWithTag("Audio"))
			{
				GameObject.FindGameObjectWithTag("Audio").GetComponent<MyUnitySingleton>().volume = this.volume;
				GameObject.FindGameObjectWithTag("Audio").GetComponent<MyUnitySingleton>().mouseSensitivity = this.mouseSensitivity;
				GameObject.FindGameObjectWithTag("Audio").GetComponent<MyUnitySingleton>().saturated = this.saturated;
			}
		}
		if (this.credits && this.mouseDown)
		{
			this.credits = false;
			this.mouseDown = false;
		}
	}

	// Token: 0x0600003E RID: 62 RVA: 0x000023FD File Offset: 0x000005FD
	private Vector2 ScaleVector(Vector2 input)
	{
		return new Vector2(input.x * this.scale.x, input.y * this.scale.y);
	}

	// Token: 0x04000090 RID: 144
	private const float SLIDER_X_RES = 904f;

	// Token: 0x04000091 RID: 145
	private const float FULL_SCREEN_X = 2560f;

	// Token: 0x04000092 RID: 146
	private const float FULL_SCREEN_Y = 1440f;

	// Token: 0x04000093 RID: 147
	public Texture2D menuTexture;

	// Token: 0x04000094 RID: 148
	public Texture2D creditTexture;

	// Token: 0x04000095 RID: 149
	private Texture2D blackTexture;

	// Token: 0x04000096 RID: 150
	public Texture2D[] playTextures;

	// Token: 0x04000097 RID: 151
	public Texture2D[] creditTextures;

	// Token: 0x04000098 RID: 152
	public Texture2D[] quitTextures;

	// Token: 0x04000099 RID: 153
	public Texture2D[] optionTextures;

	// Token: 0x0400009A RID: 154
	private Vector2 difference = Vector2.zero;

	// Token: 0x0400009B RID: 155
	private Vector2 size = Vector2.zero;

	// Token: 0x0400009C RID: 156
	private Vector2 scale = Vector2.zero;

	// Token: 0x0400009D RID: 157
	private Vector2 playTextureSize = new Vector2(158f, 80f);

	// Token: 0x0400009E RID: 158
	private Vector2 creditTextureSize = new Vector2(291f, 81f);

	// Token: 0x0400009F RID: 159
	private Vector2 quitTextureSize = new Vector2(185f, 80f);

	// Token: 0x040000A0 RID: 160
	private Vector2 optionTextureSize = new Vector2(274f, 80f);

	// Token: 0x040000A1 RID: 161
	private Vector2 playTexturePos = new Vector2(1635f, 837f);

	// Token: 0x040000A2 RID: 162
	private Vector2 creditTexturePos = new Vector2(1574f, 938f);

	// Token: 0x040000A3 RID: 163
	private Vector2 quitTexturePos = new Vector2(1632f, 1134f);

	// Token: 0x040000A4 RID: 164
	private Vector2 optionTexturePos = new Vector2(1583f, 1037f);

	// Token: 0x040000A5 RID: 165
	private bool mouseDown;

	// Token: 0x040000A6 RID: 166
	private float alphaFadeValue;

	// Token: 0x040000A7 RID: 167
	private float startFadeValue;

	// Token: 0x040000A8 RID: 168
	private bool credits;

	// Token: 0x040000A9 RID: 169
	private bool options;

	// Token: 0x040000AA RID: 170
	public Texture2D loadTexture;

	// Token: 0x040000AB RID: 171
	public Texture2D optionsTexture;

	// Token: 0x040000AC RID: 172
	private Vector2 originalMenuSize = new Vector2(2560f, 1440f);

	// Token: 0x040000AD RID: 173
	public Texture2D[] menuTextures;

	// Token: 0x040000AE RID: 174
	private Vector2 menuTextureSize = new Vector2(303f, 85f);

	// Token: 0x040000AF RID: 175
	private Vector2 menuTexturePos = new Vector2(1493f, 1084f);

	// Token: 0x040000B0 RID: 176
	public Texture2D[] checkBoxTextures;

	// Token: 0x040000B1 RID: 177
	private Vector2 checkBoxSize = new Vector2(110f, 110f);

	// Token: 0x040000B2 RID: 178
	private Vector2 saturatedPos = new Vector2(912f, 891f);

	// Token: 0x040000B3 RID: 179
	private Vector2 deSaturatedPos = new Vector2(1183f, 891f);

	// Token: 0x040000B4 RID: 180
	public Texture2D sliderBox;

	// Token: 0x040000B5 RID: 181
	private Vector2 sliderBoxSize = new Vector2(65f, 65f);

	// Token: 0x040000B6 RID: 182
	private Vector2 volumeSliderPos = new Vector2(970f, 570.5f);

	// Token: 0x040000B7 RID: 183
	private Vector2 mouseSliderPos = new Vector2(970f, 715f);

	// Token: 0x040000B8 RID: 184
	private float sliderLength;

	// Token: 0x040000B9 RID: 185
	private Rect volumeRect;

	// Token: 0x040000BA RID: 186
	private Rect mouseRect;

	// Token: 0x040000BB RID: 187
	public float volume;

	// Token: 0x040000BC RID: 188
	public float mouseSensitivity;

	// Token: 0x040000BD RID: 189
	private bool volumeMove;

	// Token: 0x040000BE RID: 190
	private bool mouseMove;

	// Token: 0x040000BF RID: 191
	private bool saturated = true;

	// Token: 0x040000C0 RID: 192
	private bool unSaturated;

	// Token: 0x040000C1 RID: 193
	private bool unset = true;

	// Token: 0x040000C2 RID: 194
	private bool fadeOut;

	// Token: 0x040000C3 RID: 195
	private bool fadeIn = true;
}
