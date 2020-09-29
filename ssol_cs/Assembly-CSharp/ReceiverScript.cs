using System;
using UnityEngine;

// Token: 0x0200000F RID: 15
public class ReceiverScript : MonoBehaviour
{
	// Token: 0x0600006D RID: 109 RVA: 0x000024FD File Offset: 0x000006FD
	private void Start()
	{
		base.transform.LookAt(this.senderTransform);
	}

	// Token: 0x0600006E RID: 110 RVA: 0x0000247A File Offset: 0x0000067A
	private void LateUpdate()
	{
	}

	// Token: 0x0400011D RID: 285
	public Transform senderTransform;
}
