using System;
using UnityEngine;

// Token: 0x02000015 RID: 21
public class TemporaryInstructionScripts : MonoBehaviour
{
	// Token: 0x06000091 RID: 145 RVA: 0x0000B92C File Offset: 0x00009B2C
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
			this.offset = new Vector2(0f, (float)((Screen.height - Screen.width * this.endImages[0].height / this.endImages[0].width) / 2));
			this.scale = new Vector2((float)Screen.width, (float)(Screen.width * this.endImages[0].height / this.endImages[0].width));
		}
		if (Screen.width > this.loader.width)
		{
			this.size = new Vector2((float)this.loader.width, (float)this.loader.height);
			this.difference = new Vector2((float)Screen.width - this.size.x, (float)Screen.height - this.size.y) / 2f;
			return;
		}
		this.difference = new Vector2(0f, 0f) / 4f;
		this.size = new Vector2((float)Screen.width, (float)Screen.height);
	}

	// Token: 0x06000092 RID: 146 RVA: 0x0000BB50 File Offset: 0x00009D50
	private void Update()
	{
		if (this.loadOp == null)
		{
			if (Input.GetMouseButtonDown(0) && !this.fadeIn)
			{
				if (this.assetNumber < this.endImages.Length - 1)
				{
					this.assetNumber++;
				}
				else
				{
					Debug.Log("load level 3 -- TempInstScripts.Update/1");
					this.loadOp = Application.LoadLevelAsync(3);
					this.loader.Play();
					this.loader.loop = true;
					GameObject.FindGameObjectWithTag("Audio").GetComponent<MyUnitySingleton>().fadeOut = true;
				}
			}
			if (this.fadeIn)
			{
				this.alphaFadeValue += Time.deltaTime;
				if (this.alphaFadeValue >= 1f)
				{
					this.fadeIn = false;
				}
			}
		}
	}

	// Token: 0x06000093 RID: 147 RVA: 0x0000BC0C File Offset: 0x00009E0C
	private void OnGUI()
	{
		if (this.loadOp == null)
		{
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1f);
			GUI.depth = 3;
			GUI.BeginGroup(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height));
			GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.loadingTexture);
			GUI.EndGroup();
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, this.alphaFadeValue);
			GUI.depth = 1;
			GUI.BeginGroup(new Rect(this.offset.x, this.offset.y, this.scale.x, this.scale.y));
			GUI.DrawTexture(new Rect(0f, 0f, this.scale.x, this.scale.y), this.endImages[this.assetNumber]);
			GUI.EndGroup();
			return;
		}
		GUI.depth = 1;
		GUI.DrawTexture(new Rect(this.difference.x, this.difference.y, this.size.x, this.size.y), this.loader);
	}

	// Token: 0x04000142 RID: 322
	private AsyncOperation loadOp;

	// Token: 0x04000143 RID: 323
	public Texture2D[] endImages;

	// Token: 0x04000144 RID: 324
	public MovieTexture loader;

	// Token: 0x04000145 RID: 325
	private int assetNumber;

	// Token: 0x04000146 RID: 326
	private Vector2 offset;

	// Token: 0x04000147 RID: 327
	private Vector2 scale;

	// Token: 0x04000148 RID: 328
	public Texture2D loadingTexture;

	// Token: 0x04000149 RID: 329
	private Vector2 difference = Vector2.zero;

	// Token: 0x0400014A RID: 330
	private Vector2 size = Vector2.zero;

	// Token: 0x0400014B RID: 331
	private bool fadeIn = true;

	// Token: 0x0400014C RID: 332
	private float alphaFadeValue;
}
