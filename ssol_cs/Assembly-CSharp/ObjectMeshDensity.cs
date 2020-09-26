using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000008 RID: 8
public class ObjectMeshDensity : MonoBehaviour
{
	// Token: 0x06000044 RID: 68 RVA: 0x0000781C File Offset: 0x00005A1C
	private void Start()
	{
		MeshFilter component = base.GetComponent<MeshFilter>();
		if (component != null)
		{
			this.original = new Mesh();
			this.original.vertices = component.mesh.vertices;
			this.original.uv = component.mesh.uv;
			this.original.triangles = component.mesh.triangles;
			this.original.RecalculateBounds();
			this.original.RecalculateNormals();
			this.original.name = component.mesh.name;
			this.change = new Mesh();
			this.change.vertices = component.mesh.vertices;
			this.change.uv = component.mesh.uv;
			this.change.triangles = component.mesh.triangles;
			this.change.RecalculateBounds();
			this.change.RecalculateNormals();
			this.change.name = component.mesh.name;
			bool flag = true;
			int num = 0;
			while (flag)
			{
				flag = this.Subdivide(this.change, component.transform);
				num++;
			}
			if (num == 1)
			{
				this.change = null;
			}
		}
		else
		{
			this.original = null;
			this.change = null;
		}
	}

	// Token: 0x06000045 RID: 69 RVA: 0x0000247A File Offset: 0x0000067A
	private void Update()
	{
	}

	// Token: 0x06000046 RID: 70 RVA: 0x00007978 File Offset: 0x00005B78
	public bool ReturnVerts(Mesh mesh, bool Subdivide)
	{
		if (Subdivide)
		{
			this.state = true;
			mesh.Clear();
			mesh.vertices = this.change.vertices;
			mesh.uv = this.change.uv;
			mesh.triangles = this.change.triangles;
			mesh.RecalculateBounds();
			mesh.RecalculateNormals();
			return true;
		}
		if (!Subdivide)
		{
			this.state = false;
			mesh.Clear();
			mesh.vertices = this.original.vertices;
			mesh.uv = this.original.uv;
			mesh.triangles = this.original.triangles;
			mesh.RecalculateBounds();
			mesh.RecalculateNormals();
			return true;
		}
		return false;
	}

	// Token: 0x06000047 RID: 71 RVA: 0x00007A30 File Offset: 0x00005C30
	private int[] CopyOverT(List<int> newT, int[] T)
	{
		T = new int[newT.Count];
		for (int i = 0; i < newT.Count; i++)
		{
			T[i] = newT[i];
		}
		return T;
	}

	// Token: 0x06000048 RID: 72 RVA: 0x00007A6C File Offset: 0x00005C6C
	private Vector3[] CopyOverV(List<Vector3> newT, Vector3[] T)
	{
		T = new Vector3[newT.Count];
		for (int i = 0; i < newT.Count; i++)
		{
			T[i].x = newT[i].x;
			T[i].y = newT[i].y;
			T[i].z = newT[i].z;
		}
		return T;
	}

	// Token: 0x06000049 RID: 73 RVA: 0x00007AF0 File Offset: 0x00005CF0
	private Vector2[] CopyOverV2(List<Vector2> newT, Vector2[] T)
	{
		T = new Vector2[newT.Count];
		for (int i = 0; i < newT.Count; i++)
		{
			T[i].x = newT[i].x;
			T[i].y = newT[i].y;
		}
		return T;
	}

	// Token: 0x0600004A RID: 74 RVA: 0x00007B58 File Offset: 0x00005D58
	private void printTriangles(Vector3[] oVerts, int[] triangles)
	{
		MonoBehaviour.print(triangles.Length);
		for (int i = 0; i < triangles.Length; i += 3)
		{
			MonoBehaviour.print(string.Concat(new object[]
			{
				"triangle ",
				i,
				" ",
				oVerts[triangles[i]],
				" ",
				oVerts[triangles[i + 1]],
				" ",
				oVerts[triangles[i + 2]]
			}));
		}
	}

	// Token: 0x0600004B RID: 75 RVA: 0x00007C08 File Offset: 0x00005E08
	private Vector3 Mid(Vector3 one, Vector3 two)
	{
		Vector3 result;
		result.x = (one.x + two.x) / 2f;
		result.y = (one.y + two.y) / 2f;
		result.z = (one.z + two.z) / 2f;
		return result;
	}

	// Token: 0x0600004C RID: 76 RVA: 0x00007C6C File Offset: 0x00005E6C
	private Vector2 Mid2(Vector2 one, Vector2 two)
	{
		Vector2 result;
		result.x = (one.x + two.x) / 2f;
		result.y = (one.y + two.y) / 2f;
		return result;
	}

	// Token: 0x0600004D RID: 77 RVA: 0x00007CB4 File Offset: 0x00005EB4
	public bool Subdivide(Mesh mesh, Transform transform)
	{
		bool result = false;
		Vector2[] array = new Vector2[mesh.uv.Length];
		int[] array2 = new int[mesh.triangles.Length];
		Vector3[] array3 = new Vector3[mesh.vertices.Length];
		Array.Copy(mesh.triangles, array2, mesh.triangles.Length);
		Array.Copy(mesh.uv, array, mesh.uv.Length);
		Array.Copy(mesh.vertices, array3, mesh.vertices.Length);
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < mesh.triangles.Length / 3; i++)
		{
			Vector3 rhs = this.RecursiveTransform(mesh.vertices[mesh.triangles[num]], transform) - this.RecursiveTransform(mesh.vertices[mesh.triangles[num + 1]], transform);
			Vector3 lhs = this.RecursiveTransform(mesh.vertices[mesh.triangles[num + 2]], transform) - this.RecursiveTransform(mesh.vertices[mesh.triangles[num + 1]], transform);
			float num3 = Vector3.Cross(lhs, rhs).magnitude / 2f;
			if ((double)num3 > this.constant)
			{
				result = true;
				this.newVerts.Add(array3[array2[num]]);
				this.newVerts.Add(array3[array2[num + 1]]);
				this.newVerts.Add(array3[array2[num + 2]]);
				this.newVerts.Add(this.Mid(array3[array2[num]], array3[array2[num + 1]]));
				this.newVerts.Add(this.Mid(array3[array2[num]], array3[array2[num + 2]]));
				this.newVerts.Add(this.Mid(array3[array2[num + 1]], array3[array2[num + 2]]));
				this.newTriangles.Add(num2);
				this.newTriangles.Add(num2 + 3);
				this.newTriangles.Add(num2 + 4);
				this.newTriangles.Add(num2 + 5);
				this.newTriangles.Add(num2 + 4);
				this.newTriangles.Add(num2 + 3);
				this.newTriangles.Add(num2 + 4);
				this.newTriangles.Add(num2 + 5);
				this.newTriangles.Add(num2 + 2);
				this.newTriangles.Add(num2 + 5);
				this.newTriangles.Add(num2 + 3);
				this.newTriangles.Add(num2 + 1);
				this.newUV.Add(array[array2[num]]);
				this.newUV.Add(array[array2[num + 1]]);
				this.newUV.Add(array[array2[num + 2]]);
				this.newUV.Add(this.Mid2(array[array2[num]], array[array2[num + 1]]));
				this.newUV.Add(this.Mid2(array[array2[num]], array[array2[num + 2]]));
				this.newUV.Add(this.Mid2(array[array2[num + 1]], array[array2[num + 2]]));
				num += 3;
				num2 += 6;
			}
			else
			{
				this.newVerts.Add(array3[array2[num]]);
				this.newVerts.Add(array3[array2[num + 1]]);
				this.newVerts.Add(array3[array2[num + 2]]);
				this.newTriangles.Add(num2);
				this.newTriangles.Add(num2 + 1);
				this.newTriangles.Add(num2 + 2);
				this.newUV.Add(array[array2[num]]);
				this.newUV.Add(array[array2[num + 1]]);
				this.newUV.Add(array[array2[num + 2]]);
				num += 3;
				num2 += 3;
			}
		}
		mesh.Clear();
		array3 = this.CopyOverV(this.newVerts, array3);
		mesh.vertices = array3;
		array = this.CopyOverV2(this.newUV, array);
		mesh.uv = array;
		array2 = this.CopyOverT(this.newTriangles, array2);
		mesh.triangles = array2;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		this.newTriangles.Clear();
		this.newVerts.Clear();
		this.newUV.Clear();
		return result;
	}

	// Token: 0x0600004E RID: 78 RVA: 0x000081EC File Offset: 0x000063EC
	public bool SubdivideSubMesh(Mesh mesh, Transform transform, int count)
	{
		bool result = false;
		Vector2[] array = new Vector2[mesh.uv.Length];
		Vector3[] array2 = new Vector3[mesh.vertices.Length];
		Array.Copy(mesh.uv, array, mesh.uv.Length);
		Array.Copy(mesh.vertices, array2, mesh.vertices.Length);
		int[][] array3 = new int[count][];
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < mesh.subMeshCount; i++)
		{
			array3[i] = mesh.GetTriangles(i);
			for (int j = 0; j < array3[i].Length / 3; j++)
			{
				Vector3 rhs = this.RecursiveTransform(mesh.vertices[mesh.triangles[num]], transform) - this.RecursiveTransform(mesh.vertices[mesh.triangles[num + 1]], transform);
				Vector3 lhs = this.RecursiveTransform(mesh.vertices[mesh.triangles[num + 2]], transform) - this.RecursiveTransform(mesh.vertices[mesh.triangles[num + 1]], transform);
				float num4 = Vector3.Cross(lhs, rhs).magnitude / 2f;
				if (j == 0)
				{
					MonoBehaviour.print(num4);
				}
				if ((double)num4 > this.constant)
				{
					result = true;
					this.newVerts.Add(array2[array3[i][num]]);
					this.newVerts.Add(array2[array3[i][num + 1]]);
					this.newVerts.Add(array2[array3[i][num + 2]]);
					this.newVerts.Add(this.Mid(array2[array3[i][num]], array2[array3[i][num + 1]]));
					this.newVerts.Add(this.Mid(array2[array3[i][num]], array2[array3[i][num + 2]]));
					this.newVerts.Add(this.Mid(array2[array3[i][num + 1]], array2[array3[i][num + 2]]));
					this.newTriangles.Add(num3 + num2);
					this.newTriangles.Add(num3 + num2 + 3);
					this.newTriangles.Add(num3 + num2 + 4);
					this.newTriangles.Add(num3 + num2 + 5);
					this.newTriangles.Add(num3 + num2 + 4);
					this.newTriangles.Add(num3 + num2 + 3);
					this.newTriangles.Add(num3 + num2 + 4);
					this.newTriangles.Add(num3 + num2 + 5);
					this.newTriangles.Add(num3 + num2 + 2);
					this.newTriangles.Add(num3 + num2 + 5);
					this.newTriangles.Add(num3 + num2 + 3);
					this.newTriangles.Add(num3 + num2 + 1);
					this.newUV.Add(array[array3[i][num]]);
					this.newUV.Add(array[array3[i][num + 1]]);
					this.newUV.Add(array[array3[i][num + 2]]);
					this.newUV.Add(this.Mid2(array[array3[i][num]], array[array3[i][num + 1]]));
					this.newUV.Add(this.Mid2(array[array3[i][num]], array[array3[i][num + 2]]));
					this.newUV.Add(this.Mid2(array[array3[i][num + 1]], array[array3[i][num + 2]]));
					num += 3;
					num2 += 6;
				}
				else
				{
					this.newVerts.Add(array2[array3[i][num]]);
					this.newVerts.Add(array2[array3[i][num + 1]]);
					this.newVerts.Add(array2[array3[i][num + 2]]);
					this.newTriangles.Add(num3 + num2);
					this.newTriangles.Add(num3 + num2 + 1);
					this.newTriangles.Add(num3 + num2 + 2);
					this.newUV.Add(array[array3[i][num]]);
					this.newUV.Add(array[array3[i][num + 1]]);
					this.newUV.Add(array[array3[i][num + 2]]);
					num += 3;
					num2 += 3;
				}
			}
			array3[i] = this.CopyOverT(this.newTriangles, array3[i]);
			this.newTriangles.Clear();
		}
		MonoBehaviour.print(count);
		mesh.Clear();
		array2 = this.CopyOverV(this.newVerts, array2);
		mesh.vertices = array2;
		mesh.subMeshCount = count;
		for (int k = 0; k < count; k++)
		{
			mesh.SetTriangles(array3[k], k);
		}
		array = this.CopyOverV2(this.newUV, array);
		mesh.uv = array;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		this.newTriangles.Clear();
		this.newVerts.Clear();
		this.newUV.Clear();
		return result;
	}

	// Token: 0x0600004F RID: 79 RVA: 0x000087F8 File Offset: 0x000069F8
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

	// Token: 0x040000D9 RID: 217
	private List<int> newTriangles = new List<int>();

	// Token: 0x040000DA RID: 218
	private List<Vector3> newVerts = new List<Vector3>();

	// Token: 0x040000DB RID: 219
	private List<Vector2> newUV = new List<Vector2>();

	// Token: 0x040000DC RID: 220
	public bool state;

	// Token: 0x040000DD RID: 221
	public Mesh change;

	// Token: 0x040000DE RID: 222
	public Mesh original;

	// Token: 0x040000DF RID: 223
	private double constant = 20.0;
}
