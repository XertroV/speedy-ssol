using System;
using UnityEngine;

// Token: 0x02000017 RID: 23
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Color Correction (Ramp)")]
public class ColorCorrectionEffect : ImageEffectBase
{
	// Token: 0x06000098 RID: 152 RVA: 0x000026A9 File Offset: 0x000008A9
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, base.material);
	}
}
