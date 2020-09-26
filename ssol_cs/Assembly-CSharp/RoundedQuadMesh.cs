using System;
using UnityEngine;

// Token: 0x0200002D RID: 45
public class RoundedQuadMesh : MonoBehaviour
{
	// Token: 0x0600009D RID: 157 RVA: 0x0000C484 File Offset: 0x0000A684
	private void Start()
	{
		this.m_MeshFilter = base.GetComponent<MeshFilter>();
		if (this.m_MeshFilter == null)
		{
			this.m_MeshFilter = base.gameObject.AddComponent<MeshFilter>();
		}
		if (base.GetComponent<MeshRenderer>() == null)
		{
			base.gameObject.AddComponent<MeshRenderer>();
		}
		this.m_Mesh = new Mesh();
		this.m_MeshFilter.sharedMesh = this.m_Mesh;
		this.UpdateMesh();
	}

	// Token: 0x0600009E RID: 158 RVA: 0x0000C4FC File Offset: 0x0000A6FC
	public Mesh UpdateMesh()
	{
		if (this.CornerVertexCount < 2)
		{
			this.CornerVertexCount = 2;
		}
		int num = this.DoubleSided ? 2 : 1;
		int num2 = this.CornerVertexCount * 4 * num + num;
		int num3 = this.CornerVertexCount * 4 * num;
		if (this.m_Vertices == null || this.m_Vertices.Length != num2)
		{
			this.m_Vertices = new Vector3[num2];
			this.m_Normals = new Vector3[num2];
		}
		if (this.m_Triangles == null || this.m_Triangles.Length != num3 * 3)
		{
			this.m_Triangles = new int[num3 * 3];
		}
		if (this.CreateUV && (this.m_UV == null || this.m_UV.Length != num2))
		{
			this.m_UV = new Vector2[num2];
		}
		int num4 = this.CornerVertexCount * 4;
		if (this.CreateUV)
		{
			this.m_UV[0] = Vector2.one * 0.5f;
			if (this.DoubleSided)
			{
				this.m_UV[num4 + 1] = this.m_UV[0];
			}
		}
		float num5 = Mathf.Max(0f, this.RoundTopLeft + this.RoundEdges);
		float num6 = Mathf.Max(0f, this.RoundTopRight + this.RoundEdges);
		float num7 = Mathf.Max(0f, this.RoundBottomLeft + this.RoundEdges);
		float num8 = Mathf.Max(0f, this.RoundBottomRight + this.RoundEdges);
		float num9 = 1.5707964f / (float)(this.CornerVertexCount - 1);
		float num10 = 1f;
		float num11 = 1f;
		float num12 = 1f;
		float num13 = 1f;
		Vector2 b = Vector2.one;
		if (this.UsePercentage)
		{
			b = new Vector2(this.rect.width, this.rect.height) * 0.5f;
			if (this.rect.width > this.rect.height)
			{
				num10 = this.rect.height / this.rect.width;
			}
			else
			{
				num11 = this.rect.width / this.rect.height;
			}
			num5 = Mathf.Clamp01(num5);
			num6 = Mathf.Clamp01(num6);
			num7 = Mathf.Clamp01(num7);
			num8 = Mathf.Clamp01(num8);
		}
		else
		{
			num12 = this.rect.width * 0.5f;
			num13 = this.rect.height * 0.5f;
			if (num5 + num6 > this.rect.width)
			{
				float num14 = this.rect.width / (num5 + num6);
				num5 *= num14;
				num6 *= num14;
			}
			if (num7 + num8 > this.rect.width)
			{
				float num15 = this.rect.width / (num7 + num8);
				num7 *= num15;
				num8 *= num15;
			}
			if (num5 + num7 > this.rect.height)
			{
				float num16 = this.rect.height / (num5 + num7);
				num5 *= num16;
				num7 *= num16;
			}
			if (num6 + num8 > this.rect.height)
			{
				float num17 = this.rect.height / (num6 + num8);
				num6 *= num17;
				num8 *= num17;
			}
		}
		this.m_Vertices[0] = this.rect.center * this.Scale;
		if (this.DoubleSided)
		{
			this.m_Vertices[num4 + 1] = this.rect.center * this.Scale;
		}
		for (int i = 0; i < this.CornerVertexCount; i++)
		{
			float num18 = Mathf.Sin((float)i * num9);
			float num19 = Mathf.Cos((float)i * num9);
			Vector2 a = new Vector3(-num12 + (1f - num19) * num5 * num10, num13 - (1f - num18) * num5 * num11);
			Vector2 a2 = new Vector3(num12 - (1f - num18) * num6 * num10, num13 - (1f - num19) * num6 * num11);
			Vector2 a3 = new Vector3(num12 - (1f - num19) * num8 * num10, -num13 + (1f - num18) * num8 * num11);
			Vector2 a4 = new Vector3(-num12 + (1f - num18) * num7 * num10, -num13 + (1f - num19) * num7 * num11);
			this.m_Vertices[1 + i] = (Vector2.Scale(a, b) + this.rect.center) * this.Scale;
			this.m_Vertices[1 + this.CornerVertexCount + i] = (Vector2.Scale(a2, b) + this.rect.center) * this.Scale;
			this.m_Vertices[1 + this.CornerVertexCount * 2 + i] = (Vector2.Scale(a3, b) + this.rect.center) * this.Scale;
			this.m_Vertices[1 + this.CornerVertexCount * 3 + i] = (Vector2.Scale(a4, b) + this.rect.center) * this.Scale;
			if (this.CreateUV)
			{
				if (!this.UsePercentage)
				{
					Vector2 b2 = new Vector2(2f / this.rect.width, 2f / this.rect.height);
					a = Vector2.Scale(a, b2);
					a2 = Vector2.Scale(a2, b2);
					a3 = Vector2.Scale(a3, b2);
					a4 = Vector2.Scale(a4, b2);
				}
				this.m_UV[1 + i] = a * 0.5f + Vector2.one * 0.5f;
				this.m_UV[1 + this.CornerVertexCount + i] = a2 * 0.5f + Vector2.one * 0.5f;
				this.m_UV[1 + this.CornerVertexCount * 2 + i] = a3 * 0.5f + Vector2.one * 0.5f;
				this.m_UV[1 + this.CornerVertexCount * 3 + i] = a4 * 0.5f + Vector2.one * 0.5f;
			}
			if (this.DoubleSided)
			{
				this.m_Vertices[1 + this.CornerVertexCount * 8 - i] = this.m_Vertices[1 + i];
				this.m_Vertices[1 + this.CornerVertexCount * 7 - i] = this.m_Vertices[1 + this.CornerVertexCount + i];
				this.m_Vertices[1 + this.CornerVertexCount * 6 - i] = this.m_Vertices[1 + this.CornerVertexCount * 2 + i];
				this.m_Vertices[1 + this.CornerVertexCount * 5 - i] = this.m_Vertices[1 + this.CornerVertexCount * 3 + i];
				if (this.CreateUV)
				{
					this.m_UV[1 + this.CornerVertexCount * 8 - i] = a * 0.5f + Vector2.one * 0.5f;
					this.m_UV[1 + this.CornerVertexCount * 7 - i] = a2 * 0.5f + Vector2.one * 0.5f;
					this.m_UV[1 + this.CornerVertexCount * 6 - i] = a3 * 0.5f + Vector2.one * 0.5f;
					this.m_UV[1 + this.CornerVertexCount * 5 - i] = a4 * 0.5f + Vector2.one * 0.5f;
				}
			}
		}
		for (int j = 0; j < num4 + 1; j++)
		{
			this.m_Normals[j] = -Vector3.forward;
			if (this.DoubleSided)
			{
				this.m_Normals[num4 + 1 + j] = Vector3.forward;
				if (this.FlipBackFaceUV)
				{
					Vector2 vector = this.m_UV[num4 + 1 + j];
					vector.x = 1f - vector.x;
					this.m_UV[num4 + 1 + j] = vector;
				}
			}
		}
		for (int k = 0; k < num4; k++)
		{
			this.m_Triangles[k * 3] = 0;
			this.m_Triangles[k * 3 + 1] = k + 1;
			this.m_Triangles[k * 3 + 2] = k + 2;
			if (this.DoubleSided)
			{
				this.m_Triangles[(num4 + k) * 3] = num4 + 1;
				this.m_Triangles[(num4 + k) * 3 + 1] = num4 + 1 + k + 1;
				this.m_Triangles[(num4 + k) * 3 + 2] = num4 + 1 + k + 2;
			}
		}
		this.m_Triangles[num4 * 3 - 1] = 1;
		if (this.DoubleSided)
		{
			this.m_Triangles[this.m_Triangles.Length - 1] = num4 + 1 + 1;
		}
		this.m_Mesh.Clear();
		this.m_Mesh.vertices = this.m_Vertices;
		this.m_Mesh.normals = this.m_Normals;
		if (this.CreateUV)
		{
			this.m_Mesh.uv = this.m_UV;
		}
		this.m_Mesh.triangles = this.m_Triangles;
		return this.m_Mesh;
	}

	// Token: 0x0600009F RID: 159 RVA: 0x00002747 File Offset: 0x00000947
	private void Update()
	{
		if (this.AutoUpdate)
		{
			this.UpdateMesh();
		}
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x0000CEF4 File Offset: 0x0000B0F4
	public RoundedQuadMesh()
	{
		this.RoundEdges = 0.5f;
		this.UsePercentage = true;
		this.rect = new Rect(-0.5f, -0.5f, 1f, 1f);
		this.Scale = 1f;
		this.CornerVertexCount = 8;
		this.CreateUV = true;
		this.AutoUpdate = true;
		base..ctor();
	}

	// Token: 0x0400015F RID: 351
	public string authorCredit = "http://answers.unity.com/answers/1018090/view.html";

	// Token: 0x04000160 RID: 352
	public float RoundEdges;

	// Token: 0x04000161 RID: 353
	public float RoundTopLeft;

	// Token: 0x04000162 RID: 354
	public float RoundTopRight;

	// Token: 0x04000163 RID: 355
	public float RoundBottomLeft;

	// Token: 0x04000164 RID: 356
	public float RoundBottomRight;

	// Token: 0x04000165 RID: 357
	public bool UsePercentage;

	// Token: 0x04000166 RID: 358
	public Rect rect;

	// Token: 0x04000167 RID: 359
	public float Scale;

	// Token: 0x04000168 RID: 360
	public int CornerVertexCount;

	// Token: 0x04000169 RID: 361
	public bool CreateUV;

	// Token: 0x0400016A RID: 362
	public bool FlipBackFaceUV;

	// Token: 0x0400016B RID: 363
	public bool DoubleSided;

	// Token: 0x0400016C RID: 364
	public bool AutoUpdate;

	// Token: 0x0400016D RID: 365
	private MeshFilter m_MeshFilter;

	// Token: 0x0400016E RID: 366
	private Mesh m_Mesh;

	// Token: 0x0400016F RID: 367
	private Vector3[] m_Vertices;

	// Token: 0x04000170 RID: 368
	private Vector3[] m_Normals;

	// Token: 0x04000171 RID: 369
	private Vector2[] m_UV;

	// Token: 0x04000172 RID: 370
	private int[] m_Triangles;

	// Token: 0x04000173 RID: 371
	public string authorCredit2 = "https://www.dropbox.com/s/r09e20it689d8if/RoundedQuadMesh.cs?dl=0";
}
