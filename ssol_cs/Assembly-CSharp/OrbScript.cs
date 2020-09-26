using System;
using UnityEngine;

// Token: 0x0200000C RID: 12
public class OrbScript : MonoBehaviour
{
	// Token: 0x06000063 RID: 99 RVA: 0x000024CD File Offset: 0x000006CD
	private void Start()
	{
		GameObject.FindGameObjectWithTag("Player").GetComponent<GUIScripts>().orbCount++;
	}

	// Token: 0x06000064 RID: 100 RVA: 0x00009CA4 File Offset: 0x00007EA4
	private void OnTriggerEnter(Collider collision)
	{
		GameObject.FindGameObjectWithTag("Player").GetComponent<MovementScripts>().returnGrowth();
		GameObject.FindGameObjectWithTag("Player").GetComponent<GUIScripts>().OrbCollision();
		GameObject.FindGameObjectWithTag("Player").GetComponent<GameState>().OrbPicked();
		UnityEngine.Object.Destroy(base.gameObject);
		base.gameObject.SetActive(false);
	}

	// Token: 0x04000112 RID: 274
	public int type;
}
