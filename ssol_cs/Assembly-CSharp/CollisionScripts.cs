using System;
using UnityEngine;

// Token: 0x02000002 RID: 2
public class CollisionScripts : MonoBehaviour
{
	// Token: 0x06000002 RID: 2 RVA: 0x00002768 File Offset: 0x00000968
	private void OnCollisionEnter(Collision collision)
	{
		if (base.gameObject.tag == "Playermesh")
		{
			var state = GameObject.FindGameObjectWithTag("Player").GetComponent<GameState>();
			state.PlayerVelocityVector *= 1f - (float)(0.98 * (double)Time.deltaTime);
			//Debug.Log($"Collision at {state.TotalTimePlayer}");
		}
	}

	// Token: 0x06000003 RID: 3 RVA: 0x00002768 File Offset: 0x00000968
	private void OnCollisionStay(Collision collision)
	{
		if (base.gameObject.tag == "Playermesh")
		{
			GameObject.FindGameObjectWithTag("Player").GetComponent<GameState>().PlayerVelocityVector *= 1f - (float)(0.98 * (double)Time.deltaTime);
		}
	}
}
