using System;
using UnityEngine;

// Token: 0x0200000E RID: 14
public class Receiver2Script : MonoBehaviour
{
	// Token: 0x0600006A RID: 106 RVA: 0x0000247A File Offset: 0x0000067A
	private void Start()
	{
	}

	// Token: 0x0600006B RID: 107 RVA: 0x000024EB File Offset: 0x000006EB
	private void OnTriggerEnter(Collider collider)
	{
		collider.gameObject.GetComponent<RelativisticObject>().SetDeathTime();
	}
}
