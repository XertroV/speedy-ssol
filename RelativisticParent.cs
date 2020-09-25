using System;
using UnityEngine;

// Token: 0x02000014 RID: 20
public class RelativisticParent : MonoBehaviour
{
	// Token: 0x0600007E RID: 126
	private void Start()
	{
		if (base.name == "maptile")
		{
			return;
		}
		DateTime startT = DateTime.Now;
		if (base.GetComponent<ObjectMeshDensity>())
		{
			base.GetComponent<ObjectMeshDensity>().enabled = false;
		}
		int num = 0;
		int num2 = 0;
		this.checkSpeed();
		Matrix4x4 worldToLocalMatrix = base.transform.worldToLocalMatrix;
		MeshFilter[] componentsInChildren = base.GetComponentsInChildren<MeshFilter>();
		int[] array = new int[componentsInChildren.Length];
		MeshRenderer[] componentsInChildren2 = base.GetComponentsInChildren<MeshRenderer>();
		int num3 = componentsInChildren.Length;
		int num4 = 0;
		for (int i = 0; i < num3; i++)
		{
			if (!(componentsInChildren[i] == null) && !(componentsInChildren[i].sharedMesh == null))
			{
				num += componentsInChildren[i].sharedMesh.vertices.Length;
				num2 += componentsInChildren[i].sharedMesh.triangles.Length;
				array[i] = componentsInChildren[i].mesh.subMeshCount;
				num4 += componentsInChildren[i].mesh.subMeshCount;
			}
		}
		Vector3[] array2 = new Vector3[num];
		int[][] array3 = new int[num4][];
		for (int j = 0; j < num4; j++)
		{
			array3[j] = new int[num2];
		}
		Vector2[] array4 = new Vector2[num];
		Material[] array5 = new Material[num4];
		int num5 = 0;
		int num6 = 0;
		for (int k = 0; k < num3; k++)
		{
			Mesh sharedMesh = componentsInChildren[k].sharedMesh;
			if (!(sharedMesh == null))
			{
				for (int l = 0; l < array[k]; l++)
				{
					array5[num6] = componentsInChildren2[k].materials[l];
					int[] triangles = sharedMesh.GetTriangles(l);
					for (int m = 0; m < triangles.Length; m++)
					{
						array3[num6][m] = triangles[m] + num5;
					}
					num6++;
				}
				Matrix4x4 matrix4x = worldToLocalMatrix * componentsInChildren[k].transform.localToWorldMatrix;
				for (int n = 0; n < sharedMesh.vertices.Length; n++)
				{
					array2[num5] = matrix4x.MultiplyPoint3x4(sharedMesh.vertices[n]);
					array4[num5] = sharedMesh.uv[n];
					num5++;
				}
				componentsInChildren[k].gameObject.active = false;
			}
		}
		Mesh mesh = new Mesh();
		mesh.subMeshCount = num4;
		mesh.vertices = array2;
		num6 = 0;
		for (int num7 = 0; num7 < num3; num7++)
		{
			for (int num8 = 0; num8 < array[num7]; num8++)
			{
				mesh.SetTriangles(array3[num6], num6);
				num6++;
			}
		}
		mesh.uv = array4;
		base.GetComponent<MeshFilter>().mesh = mesh;
		base.GetComponent<MeshRenderer>().enabled = true;
		base.GetComponent<MeshFilter>().mesh.RecalculateNormals();
		base.GetComponent<MeshFilter>().renderer.materials = array5;
		base.transform.gameObject.active = true;
		this.meshFilter = base.GetComponent<MeshFilter>();
		this.state = GameObject.FindGameObjectWithTag("Player").GetComponent<GameState>();
		this.meshFilter = base.GetComponent<MeshFilter>();
		MeshRenderer component = base.GetComponent<MeshRenderer>();
		if (component.materials[0].mainTexture != null)
		{
			Material material = UnityEngine.Object.Instantiate(component.materials[0]) as Material;
			material.SetFloat("_viw", 0f);
			component.materials[0] = material;
		}
		Transform transform = Camera.main.transform;
		float d = (Camera.main.farClipPlane - Camera.main.nearClipPlane) / 2f;
		Vector3 center = transform.position + transform.forward * d;
		float d2 = 500000f;
		this.meshFilter.sharedMesh.bounds = new Bounds(center, Vector3.one * d2);
		if (base.GetComponent<ObjectMeshDensity>())
		{
			base.GetComponent<ObjectMeshDensity>().enabled = true;
		}
		this.TimeSince(startT, "END >");
	}

	// Token: 0x0600007F RID: 127
	private void Update()
	{
		if (this.meshFilter != null)
		{
			bool movementFrozen = this.state.MovementFrozen;
		}
	}

	// Token: 0x06000080 RID: 128
	public Vector3 RecursiveTransform(Vector3 pt, Transform trans)
	{
		Vector3 zero = Vector3.zero;
		if (trans.parent != null)
		{
			pt = this.RecursiveTransform(zero, trans.parent);
			return pt;
		}
		return trans.TransformPoint(pt);
	}

	// Token: 0x06000081 RID: 129
	public void PeriodicAddTime()
	{
		this.meshFilter.transform.Translate(new Vector3(0f, (float)this.amp * Mathf.Sin((float)((double)this.period * this.state.TotalTimeWorld)) - (float)this.amp * Mathf.Sin((float)((double)this.period * (this.state.TotalTimeWorld - this.state.DeltaTimeWorld))), 0f));
	}

	// Token: 0x06000082 RID: 130
	public Vector3 PeriodicSubtractTime(float tisw, Quaternion rotation)
	{
		return rotation * new Vector3(0f, (float)this.amp * Mathf.Sin((float)((double)this.period * (this.state.TotalTimeWorld + (double)tisw))) - (float)this.amp * Mathf.Sin((float)((double)this.period * this.state.TotalTimeWorld)), 0f);
	}

	// Token: 0x06000083 RID: 131
	public Vector3 CurrentVelocity()
	{
		Vector3 zero = Vector3.zero;
		zero.y = (float)this.amp * this.period * Mathf.Cos((float)((double)this.period * this.state.TotalTimeWorld));
		return zero;
	}

	// Token: 0x06000084 RID: 132
	public Vector4 CurrentVelocity4()
	{
		Vector4 zero = Vector4.zero;
		zero.y = (float)this.amp * this.period * Mathf.Cos((float)((double)this.period * this.state.TotalTimeWorld));
		return zero;
	}

	// Token: 0x06000085 RID: 133
	private void checkSpeed()
	{
		if (this.periodic && (float)this.amp * this.period > 4.95f)
		{
			this.period = 4.95f / (float)this.amp;
			return;
		}
		if (this.viw.magnitude > 4.95f)
		{
			this.viw = this.viw.normalized * 4.95f;
		}
	}

	// Token: 0x06000494 RID: 1172
	private void TimeSince(DateTime earlier)
	{
		this.TimeSince(earlier, string.Empty, string.Empty, false);
	}

	// Token: 0x060004A6 RID: 1190
	private void TimeSince(DateTime earlier, string msgPrefix)
	{
		this.TimeSince(earlier, msgPrefix, string.Empty, false);
	}

	// Token: 0x060004A7 RID: 1191
	private void TimeSince(DateTime earlier, string msgPrefix, string msgSuffix)
	{
		this.TimeSince(earlier, msgPrefix, msgSuffix, false);
	}

	// Token: 0x060004C0 RID: 1216
	private void TimeSince(DateTime earlier, string msgPrefix, string msgSuffix, bool alwasyPrint)
	{
		DateTime endT = DateTime.Now;
		double totalSecs = endT.Subtract(earlier).TotalSeconds;
		string msg = string.Empty;
		if (totalSecs > 0.4 || alwasyPrint)
		{
			msg = string.Concat(new string[]
			{
				msg,
				msg,
				"Time: ",
				endT.ToLongTimeString(),
				" | Start took: ",
				totalSecs.ToString("00.000")
			});
		}
		if (totalSecs > 0.4 || alwasyPrint)
		{
			msg = msg + "\n" + string.Join(" <|> ", new string[]
			{
				base.name,
				base.tag,
				this.ToString()
			});
		}
		msg = string.Join("\n", new string[]
		{
			msgPrefix,
			msg,
			msgSuffix
		});
		if (msg.Length > 2 || alwasyPrint)
		{
			Debug.Log(msg);
		}
	}

	// Token: 0x0400012D RID: 301
	private MeshFilter meshFilter;

	// Token: 0x0400012E RID: 302
	public Vector3 viw;

	// Token: 0x0400012F RID: 303
	private GameState state;

	// Token: 0x04000130 RID: 304
	public int amp;

	// Token: 0x04000131 RID: 305
	public float period;

	// Token: 0x04000132 RID: 306
	public bool periodic;
}
