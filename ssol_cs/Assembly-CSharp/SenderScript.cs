using System;
using UnityEngine;

// Token: 0x02000010 RID: 16
public class SenderScript : MonoBehaviour
{
	// Token: 0x06000070 RID: 112 RVA: 0x00002523 File Offset: 0x00000723
	private void Start()
	{
		if (this.launchTimer == 0)
		{
			this.launchTimer = 3;
		}
		base.transform.LookAt(this.receiverTransform);
		this.viwMax = Mathf.Min(this.viwMax, 7.99f);
	}

	// Token: 0x06000071 RID: 113 RVA: 0x0000A200 File Offset: 0x00008400
	private void Update()
	{
		if (!GameObject.FindGameObjectWithTag("Player").GetComponent<GameState>().MovementFrozen)
		{
			this.launchCounter += Time.deltaTime;
		}
		if (this.launchCounter >= (float)this.launchTimer)
		{
			this.launchCounter = 0f;
			this.LaunchObject();
		}
	}

	// Token: 0x06000072 RID: 114 RVA: 0x0000A25C File Offset: 0x0000845C
	private void LaunchObject()
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("GameObjects/Moving Person", typeof(GameObject)), base.transform.position, base.transform.rotation);
		gameObject.transform.Translate(new Vector3(0f, gameObject.GetComponent<MeshFilter>().mesh.bounds.extents.y, 0f));
		gameObject.transform.parent = base.transform;
		gameObject.GetComponent<RelativisticObject>().viw = this.viwMax * base.transform.forward;
		gameObject.GetComponent<RelativisticObject>().SetStartTime();
	}

	// Token: 0x0400011E RID: 286
	public Transform receiverTransform;

	// Token: 0x0400011F RID: 287
	public int launchTimer;

	// Token: 0x04000120 RID: 288
	private float launchCounter;

	// Token: 0x04000121 RID: 289
	public float viwMax = 3f;
}
