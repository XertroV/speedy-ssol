using System;
using UnityEngine;

// Token: 0x02000011 RID: 17
public class WhatHappenedScript : MonoBehaviour
{
	// Token: 0x06000074 RID: 116 RVA: 0x0000A394 File Offset: 0x00008594
	private void Start()
	{
		if (this.endImages == null)
		{
			UnityEngine.Object[] array = Resources.LoadAll("Images/Instructions", typeof(Texture2D));
			this.endImages = new Texture2D[array.Length];
			for (int i = 1; i <= array.Length; i++)
			{
				this.endImages[i - 1] = (Texture2D)Resources.Load("Images/Instructions/0" + i, typeof(Texture2D));
			}
		}
		this.blackTexture = (Texture2D)Resources.Load("Images/blackTexture", typeof(Texture2D));
		this.whiteTexture = (Texture2D)Resources.Load("Images/whiteTexture", typeof(Texture2D));
		this.assetNumber = 0;
		Screen.lockCursor = false;
		Screen.showCursor = true;
		if (this.endImages[0].width < Screen.width && this.endImages[0].height < Screen.height)
		{
			this.offset = new Vector2((float)((Screen.width - this.endImages[0].width) / 2), (float)((Screen.height - this.endImages[0].height) / 2));
			this.scale = new Vector2((float)this.endImages[0].width, (float)this.endImages[0].height);
		}
		else
		{
			this.offset = new Vector2(0f, 0f);
			this.scale = new Vector2((float)Screen.width, (float)Screen.height);
		}
		this.buttonScale = new Vector2(this.scale.x / 2560f, this.scale.y / 1440f);
		this.menuTextureRect = this.ScaleRect(this.menuTextureRect);
		this.backTextureRect = this.ScaleRect(this.backTextureRect);
		this.nextTextureRect = this.ScaleRect(this.nextTextureRect);
	}

	// Token: 0x06000075 RID: 117 RVA: 0x0000A584 File Offset: 0x00008784
	private void Update()
	{
		if (!this.fade && this.alphaFadeValue < 1f)
		{
			this.alphaFadeValue += Mathf.Clamp01(Time.deltaTime / 2f);
		}
		if (this.fade)
		{
			this.alphaFadeValue -= Mathf.Clamp01(Time.deltaTime / 2f);
			if (this.alphaFadeValue <= 0f)
			{
				Debug.Log("load level 1 -- WhatHappened.Update/1");
				Application.LoadLevel(1);
			}
		}
	}

	// Token: 0x06000076 RID: 118 RVA: 0x0000A608 File Offset: 0x00008808
	private void OnGUI()
	{
		if (this.fade)
		{
			GUI.depth = 0;
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1f - this.alphaFadeValue);
			GUI.BeginGroup(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height));
			GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.blackTexture);
			GUI.EndGroup();
		}
		if (this.fade)
		{
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, this.alphaFadeValue);
		}
		else
		{
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1f);
		}
		GUI.depth = 3;
		GUI.BeginGroup(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height));
		GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.whiteTexture);
		GUI.EndGroup();
		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, this.alphaFadeValue);
		GUI.depth = 1;
		GUI.BeginGroup(new Rect(this.offset.x, this.offset.y, this.scale.x, this.scale.y));
		GUI.DrawTexture(new Rect(0f, 0f, this.scale.x, this.scale.y), this.endImages[this.assetNumber]);
		GUI.EndGroup();
		if (this.assetNumber < this.endImages.Length - 1)
		{
			GUI.skin.button.normal.background = this.nextTextures[0];
			GUI.skin.button.hover.background = this.nextTextures[1];
			GUI.skin.button.active.background = this.nextTextures[0];
			if (GUI.Button(this.nextTextureRect, string.Empty))
			{
				this.assetNumber++;
			}
		}
		else
		{
			GUI.skin.button.normal.background = this.menuTextures[0];
			GUI.skin.button.hover.background = this.menuTextures[1];
			GUI.skin.button.active.background = this.menuTextures[0];
			if (GUI.Button(this.menuTextureRect, string.Empty))
			{
				this.fade = true;
				if (GameObject.FindGameObjectWithTag("Audio") != null)
				{
					GameObject.FindGameObjectWithTag("Audio").GetComponent<MyUnitySingleton>().fadeOut = true;
				}
			}
		}
		if (this.assetNumber > 0)
		{
			GUI.skin.button.normal.background = this.backTextures[0];
			GUI.skin.button.hover.background = this.backTextures[1];
			GUI.skin.button.active.background = this.backTextures[0];
			if (GUI.Button(this.backTextureRect, string.Empty))
			{
				this.assetNumber--;
			}
		}
	}

	// Token: 0x06000077 RID: 119 RVA: 0x0000A9E4 File Offset: 0x00008BE4
	private Rect ScaleRect(Rect input)
	{
		return new Rect(input.x * this.buttonScale.x, input.y * this.buttonScale.y, input.width * this.buttonScale.x, input.height * this.buttonScale.y);
	}

	// Token: 0x04000122 RID: 290
	public Texture2D[] endImages;

	// Token: 0x04000123 RID: 291
	private bool fade;

	// Token: 0x04000124 RID: 292
	private float alphaFadeValue;

	// Token: 0x04000125 RID: 293
	private int assetNumber;

	// Token: 0x04000126 RID: 294
	private Vector2 offset;

	// Token: 0x04000127 RID: 295
	private Vector2 scale;

	// Token: 0x04000128 RID: 296
	private Texture2D blackTexture;

	// Token: 0x04000129 RID: 297
	private Texture2D whiteTexture;

	// Token: 0x0400012A RID: 298
	public Texture2D[] menuTextures;

	// Token: 0x0400012B RID: 299
	public Texture2D[] backTextures;

	// Token: 0x0400012C RID: 300
	public Texture2D[] nextTextures;

	// Token: 0x0400012D RID: 301
	private Vector2 buttonScale = Vector2.zero;

	// Token: 0x0400012E RID: 302
	private Rect menuTextureRect = new Rect(2206f, 574f, 285f, 242f);

	// Token: 0x0400012F RID: 303
	private Rect backTextureRect = new Rect(62f, 574f, 265f, 242f);

	// Token: 0x04000130 RID: 304
	private Rect nextTextureRect = new Rect(2227f, 574f, 265f, 242f);
}
