using System;
using UnityEngine;

// Token: 0x0200000D RID: 13
public class PreLoaderScript : MonoBehaviour
{
	// Token: 0x06000066 RID: 102 RVA: 0x00009D7C File Offset: 0x00007F7C
	private void Start()
	{
		this.blackTexture = (Texture2D)Resources.Load("Images/blackTexture", typeof(Texture2D));
		if (this.splashes.Length == 0)
		{
			this.splashes = new Texture2D[]
			{
				(Texture2D)Resources.Load("Images/blackTexture", typeof(Texture2D))
			};
		}
		this.difference = new Vector2[this.splashes.Length];
		this.size = new Vector2[this.splashes.Length];
		for (int i = 0; i < this.splashes.Length; i++)
		{
			this.size[i] = new Vector2((float)Screen.width, (float)Screen.height);
			if ((float)Screen.width > this.sizes[i].x)
			{
				this.size[i].x = this.sizes[i].x;
				if ((float)Screen.height > this.sizes[i].y)
				{
					this.size[i].y = this.sizes[i].y;
				}
			}
			this.difference[i] = new Vector2(Mathf.Max((float)Screen.width - this.sizes[i].x, 0f), Mathf.Max((float)Screen.height - this.sizes[i].y, 0f)) / 2f;
		}
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00009F0C File Offset: 0x0000810C
	private void Update()
	{
		this.counter -= Time.deltaTime;
		this.stillCounter -= Time.deltaTime;
		if (this.current <= this.splashes.Length)
		{
			Debug.Log("load level 1 -- PreLoaderScript.Update/1");
			Application.LoadLevel(1);
		}
		if (this.counter <= 0f)
		{
			if (this.fadeIn == 0)
			{
				this.stillCounter = 1f;
				this.fadeIn = 1;
			}
			if (this.fadeIn == 1 && this.stillCounter <= 0f && this.current > 0)
			{
				this.fadeIn = 2;
				this.counter = 1f;
			}
			else if (this.fadeIn == 2)
			{
				this.fadeIn = 0;
				this.current++;
				if (this.current == this.splashes.Length)
				{
					Debug.Log("load level 1 -- PreLoaderScript.Update/2");
					Application.LoadLevel(1);
				}
				this.counter = 1f;
			}
		}
		if (Input.GetMouseButtonDown(0) && this.current == 0 && this.fadeIn < 2)
		{
			this.fadeIn = 2;
			this.counter = 1f;
		}
	}

	// Token: 0x06000068 RID: 104 RVA: 0x0000A030 File Offset: 0x00008230
	private void OnGUI()
	{
		GUI.depth = 2;
		GUI.BeginGroup(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height));
		GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.blackTexture);
		GUI.EndGroup();
		GUI.depth = 1;
		if (this.fadeIn == 0)
		{
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1f - this.counter);
		}
		else if (this.fadeIn == 1)
		{
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1f);
		}
		else
		{
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, this.counter);
		}
		if (this.current < this.splashes.Length)
		{
			GUI.BeginGroup(new Rect(this.difference[this.current].x, this.difference[this.current].y, this.size[this.current].x, this.size[this.current].y));
			GUI.DrawTexture(new Rect(0f, 0f, this.size[this.current].x, this.size[this.current].y), this.splashes[this.current]);
			GUI.EndGroup();
		}
	}

	// Token: 0x04000113 RID: 275
	private float counter = 1f;

	// Token: 0x04000114 RID: 276
	private float stillCounter = 1f;

	// Token: 0x04000115 RID: 277
	private int fadeIn;

	// Token: 0x04000116 RID: 278
	private AsyncOperation loadOp;

	// Token: 0x04000117 RID: 279
	public Texture2D[] splashes;

	// Token: 0x04000118 RID: 280
	private Texture2D blackTexture;

	// Token: 0x04000119 RID: 281
	private int current;

	// Token: 0x0400011A RID: 282
	private Vector2[] difference;

	// Token: 0x0400011B RID: 283
	private Vector2[] size;

	// Token: 0x0400011C RID: 284
	private Vector2[] sizes = new Vector2[]
	{
		new Vector2(2560f, 1440f),
		new Vector2(1024f, 768f),
		new Vector2(1239f, 822f)
	};
}
