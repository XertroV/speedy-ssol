using System;
using UnityEngine;

// Token: 0x02000018 RID: 24
[RequireComponent(typeof(Camera))]
[AddComponentMenu("")]
public class ImageEffectBase : MonoBehaviour
{
	// Token: 0x0600009A RID: 154 RVA: 0x000026B8 File Offset: 0x000008B8
	protected void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
		if (!this.shader || !this.shader.isSupported)
		{
			base.enabled = false;
		}
	}

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x0600009B RID: 155 RVA: 0x000026F3 File Offset: 0x000008F3
	protected Material material
	{
		get
		{
			if (this.m_Material == null)
			{
				this.m_Material = new Material(this.shader);
				this.m_Material.hideFlags = HideFlags.HideAndDontSave;
			}
			return this.m_Material;
		}
	}

	// Token: 0x0600009C RID: 156 RVA: 0x0000272A File Offset: 0x0000092A
	protected void OnDisable()
	{
		if (this.m_Material)
		{
			UnityEngine.Object.DestroyImmediate(this.m_Material);
		}
	}

	// Token: 0x04000153 RID: 339
	public Shader shader;

	// Token: 0x04000154 RID: 340
	private Material m_Material;
}
