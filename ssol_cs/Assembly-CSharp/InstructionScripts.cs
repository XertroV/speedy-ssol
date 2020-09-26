using System;
using UnityEngine;

// Token: 0x02000005 RID: 5
public class InstructionScripts : MonoBehaviour
{
	// Token: 0x06000037 RID: 55 RVA: 0x00005A64 File Offset: 0x00003C64
	private void Start()
	{
		if (this.endImages == null)
		{
			UnityEngine.Object[] array = Resources.LoadAll("Images/Instructions", typeof(Texture2D));
			this.endImages = new Texture2D[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				this.endImages[i] = (Texture2D)array[i];
			}
		}
		this.assetNumber = 0;
		this.movie = true;
		this.screens[this.assetNumber].Play();
		Screen.lockCursor = false;
		Screen.showCursor = true;
		if (Screen.width > this.endImages[0].width)
		{
			this.size = new Vector2((float)this.endImages[0].width, (float)this.endImages[0].height);
			this.difference = new Vector2((float)Screen.width - this.size.x, (float)Screen.height - this.size.y) / 2f;
			return;
		}
		this.difference = new Vector2((float)(Screen.width - this.endImages[0].width), (float)(Screen.height - this.endImages[0].height)) / 4f;
		this.size = new Vector2((float)this.endImages[0].width, (float)this.endImages[0].height);
	}

	// Token: 0x06000038 RID: 56 RVA: 0x000023EC File Offset: 0x000005EC
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			this.mouseDown = true;
		}
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00005BC0 File Offset: 0x00003DC0
	private void OnGUI()
	{
		if (this.alphaFadeValue > 0f)
		{
			GUI.depth = 2;
			this.alphaFadeValue -= Mathf.Clamp01(Time.deltaTime / 5f);
			if (this.mouseDown)
			{
				this.movie = false;
				this.alphaFadeValue = 0f;
				this.mouseDown = false;
			}
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, this.alphaFadeValue);
			GUI.DrawTexture(new Rect(this.difference.x, this.difference.y, this.size.x, this.size.y), this.endImages[this.assetNumber - 1]);
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1f - this.alphaFadeValue);
			GUI.DrawTexture(new Rect(this.difference.x, this.difference.y, this.size.x, this.size.y), this.screens[this.assetNumber]);
		}
		else if (this.mouseDown)
		{
			if (this.assetNumber < this.screens.Length - 1)
			{
				if (this.screens[this.assetNumber].isPlaying && this.movie)
				{
					this.screens[this.assetNumber].Stop();
					this.movie = false;
				}
				else
				{
					MonoBehaviour.print("Should move to that stupid next movie");
					this.assetNumber++;
					this.movie = true;
					this.alphaFadeValue = 1f;
				}
			}
			else if (this.assetNumber < this.endImages.Length - 1)
			{
				this.assetNumber++;
				this.movie = false;
			}
			else
			{
				Debug.Log("loading level 3, async -- instructionScripts");
				this.loadOp = Application.LoadLevelAsync(3);
				this.loader.Play();
			}
			this.mouseDown = false;
		}
		if (this.loadOp != null)
		{
			GUI.DrawTexture(new Rect(this.difference.x, this.difference.y, this.size.x, this.size.y), this.loader);
		}
		else if (this.movie)
		{
			GUI.DrawTexture(new Rect(this.difference.x, this.difference.y, this.size.x, this.size.y), this.screens[this.assetNumber]);
		}
		else
		{
			GUI.DrawTexture(new Rect(this.difference.x, this.difference.y, this.size.x, this.size.y), this.endImages[this.assetNumber]);
		}
		if (!this.screens[this.assetNumber].isPlaying)
		{
			this.screens[this.assetNumber].Play();
		}
	}

	// Token: 0x04000086 RID: 134
	private AsyncOperation loadOp;

	// Token: 0x04000087 RID: 135
	public MovieTexture[] screens;

	// Token: 0x04000088 RID: 136
	public MovieTexture loader;

	// Token: 0x04000089 RID: 137
	public Texture2D[] endImages;

	// Token: 0x0400008A RID: 138
	private int assetNumber;

	// Token: 0x0400008B RID: 139
	private bool movie;

	// Token: 0x0400008C RID: 140
	private bool mouseDown;

	// Token: 0x0400008D RID: 141
	private Vector2 difference = Vector2.zero;

	// Token: 0x0400008E RID: 142
	private Vector2 size;

	// Token: 0x0400008F RID: 143
	private float alphaFadeValue;
}
