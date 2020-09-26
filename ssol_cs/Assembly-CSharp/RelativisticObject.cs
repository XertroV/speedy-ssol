using System;
using UnityEngine;

// Token: 0x02000013 RID: 19
public class RelativisticObject : MonoBehaviour
{
	// Token: 0x0600007D RID: 125 RVA: 0x0000257F File Offset: 0x0000077F
	public void SetStartTime()
	{
		this.startTime = (float)GameObject.FindGameObjectWithTag("Player").GetComponent<GameState>().TotalTimeWorld;
	}

	// Token: 0x0600007E RID: 126 RVA: 0x0000259C File Offset: 0x0000079C
	public void SetDeathTime()
	{
		this.deathTime = (float)this.state.TotalTimeWorld;
	}

	// Token: 0x0600007F RID: 127 RVA: 0x0000ABE4 File Offset: 0x00008DE4
	private void Start()
	{
		this.checkSpeed();
		this.state = GameObject.FindGameObjectWithTag("Player").GetComponent<GameState>();
		this.meshFilter = base.GetComponent<MeshFilter>();
		MeshRenderer component = base.GetComponent<MeshRenderer>();
		if (component != null && component.materials[0].mainTexture != null)
		{
			Material material = UnityEngine.Object.Instantiate(component.materials[0]) as Material;
			component.materials[0] = material;
			component.materials[0].SetFloat("_strtTime", this.startTime);
			component.materials[0].SetVector("_strtPos", new Vector4(base.transform.position.x, base.transform.position.y, base.transform.position.z, 0f));
		}
		if (this.meshFilter != null)
		{
			this.rawVerts = this.meshFilter.mesh.vertices;
		}
		else
		{
			this.rawVerts = null;
		}
		Transform transform = Camera.main.transform;
		float d = (Camera.main.farClipPlane - Camera.main.nearClipPlane) / 2f;
		Vector3 center = transform.position + transform.forward * d;
		float d2 = 500000f;
		this.meshFilter.sharedMesh.bounds = new Bounds(center, Vector3.one * d2);
	}

	// Token: 0x06000080 RID: 128 RVA: 0x0000AD70 File Offset: 0x00008F70
	private void Update()
	{
		MeshRenderer component = base.GetComponent<MeshRenderer>();
		if (this.meshFilter != null && !this.state.MovementFrozen)
		{
			ObjectMeshDensity component2 = base.GetComponent<ObjectMeshDensity>();
			if (component2 != null && this.rawVerts != null && component2.change != null)
			{
				if (!component2.state && this.RecursiveTransform(this.rawVerts[0], this.meshFilter.transform).magnitude < 21000f)
				{
					if (component2.ReturnVerts(this.meshFilter.mesh, true))
					{
						this.rawVerts = new Vector3[this.meshFilter.mesh.vertices.Length];
						Array.Copy(this.meshFilter.mesh.vertices, this.rawVerts, this.meshFilter.mesh.vertices.Length);
					}
				}
				else if (component2.state && this.RecursiveTransform(this.rawVerts[0], this.meshFilter.transform).magnitude > 21000f && component2.ReturnVerts(this.meshFilter.mesh, false))
				{
					this.rawVerts = new Vector3[this.meshFilter.mesh.vertices.Length];
					Array.Copy(this.meshFilter.mesh.vertices, this.rawVerts, this.meshFilter.mesh.vertices.Length);
				}
			}
			if (component != null)
			{
				Vector3 vector = this.viw / (float)this.state.SpeedOfLight;
				component.materials[0].SetVector("_viw", new Vector4(vector.x, vector.y, vector.z, 0f));
			}
			if (base.transform != null && this.deathTime != 0f)
			{
				float num = 57.29578f * Mathf.Acos(Vector3.Dot(this.state.PlayerVelocityVector, Vector3.forward) / this.state.PlayerVelocityVector.magnitude);
				if (this.state.PlayerVelocityVector.sqrMagnitude == 0f)
				{
					num = 0f;
				}
				Quaternion rotation = Quaternion.AngleAxis(-num, Vector3.Cross(this.state.PlayerVelocityVector, Vector3.forward));
				Vector3 vector2 = new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z);
				vector2 -= this.state.playerTransform.position;
				vector2 = rotation * vector2;
				Vector3 vector3 = rotation * this.viw;
				float num2 = -Vector3.Dot(vector2, vector2);
				float num3 = -(2f * Vector3.Dot(vector2, vector3));
				float num4 = (float)this.state.SpeedOfLightSqrd - Vector3.Dot(vector3, vector3);
				float num5 = (float)(((double)(-(double)num3) - Math.Sqrt((double)(num3 * num3 - 4f * num4 * num2))) / (double)(2f * num4));
				if (this.state.TotalTimeWorld + (double)num5 > (double)this.deathTime)
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
			}
			if (base.GetComponent<Rigidbody>() != null && !double.IsNaN(this.state.SqrtOneMinusVSquaredCWDividedByCSquared) && (float)this.state.SqrtOneMinusVSquaredCWDividedByCSquared != 0f)
			{
				Vector3 velocity = this.viw;
				velocity.x /= (float)this.state.SqrtOneMinusVSquaredCWDividedByCSquared;
				velocity.y /= (float)this.state.SqrtOneMinusVSquaredCWDividedByCSquared;
				velocity.z /= (float)this.state.SqrtOneMinusVSquaredCWDividedByCSquared;
				base.GetComponent<Rigidbody>().velocity = velocity;
			}
		}
		else if (this.meshFilter != null && component != null && base.GetComponent<Rigidbody>() != null)
		{
			base.GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
	}

	// Token: 0x06000081 RID: 129 RVA: 0x0000B1E8 File Offset: 0x000093E8
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

	// Token: 0x06000082 RID: 130 RVA: 0x000025B0 File Offset: 0x000007B0
	private void checkSpeed()
	{
		if (this.viw.magnitude > 4.95f)
		{
			this.viw = this.viw.normalized * 4.95f;
		}
	}

	// Token: 0x04000135 RID: 309
	private MeshFilter meshFilter;

	// Token: 0x04000136 RID: 310
	private Vector3[] rawVerts;

	// Token: 0x04000137 RID: 311
	public Vector3 viw;

	// Token: 0x04000138 RID: 312
	private GameState state;

	// Token: 0x04000139 RID: 313
	private float startTime;

	// Token: 0x0400013A RID: 314
	private float deathTime;

	// Token: 0x0400013B RID: 315
	private Vector3 oneVert;
}
