using System;
using UnityEngine;

// Token: 0x02000012 RID: 18
public class WinLevelScript : MonoBehaviour
{
	// Token: 0x06000079 RID: 121 RVA: 0x0000255E File Offset: 0x0000075E
	private void Start()
	{
		this.whiteTexture = (Texture2D)Resources.Load("Images/whiteTexture", typeof(Texture2D));
	}

	// Token: 0x0600007A RID: 122 RVA: 0x0000AA44 File Offset: 0x00008C44
	private void OnGUI()
	{
		if (this.alphaFadeValue > 0f && !this.ending)
		{
			this.alphaFadeValue -= Mathf.Clamp01(Time.deltaTime / 5f);
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, this.alphaFadeValue);
			GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.whiteTexture);
			return;
		}
		if (this.ending)
		{
			this.alphaFadeValue += Mathf.Clamp01(Time.deltaTime / 15f);
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, this.alphaFadeValue);
			GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.whiteTexture);
			if (this.alphaFadeValue > 1f)
			{
				Screen.showCursor = true;
				Screen.lockCursor = false;
				GameObject.FindGameObjectWithTag("Audio").GetComponent<MyUnitySingleton>().gameState = null;
				Debug.Log("load level 4 -- WinLevelScript.OnGUI/1");
				Application.LoadLevel(4);
			}
		}
	}

	// Token: 0x0600007B RID: 123 RVA: 0x0000AB94 File Offset: 0x00008D94
	private void OnTriggerEnter()
	{
		if (!this.ending)
		{
			this.ending = true;
			GameObject.FindGameObjectWithTag("Audio").GetComponent<MyUnitySingleton>().audio.PlayOneShot((AudioClip)Resources.Load("Audio/orb11", typeof(AudioClip)));
		}
	}

	// Token: 0x04000131 RID: 305
	private float alphaFadeValue;

	// Token: 0x04000132 RID: 306
	private AsyncOperation loadOp;

	// Token: 0x04000133 RID: 307
	public bool ending;

	// Token: 0x04000134 RID: 308
	private Texture2D whiteTexture;
}
