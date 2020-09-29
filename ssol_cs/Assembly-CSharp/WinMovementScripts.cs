using System;
using UnityEngine;

// Token: 0x02000016 RID: 22
public class WinMovementScripts : MonoBehaviour
{
	// Token: 0x06000095 RID: 149 RVA: 0x00002671 File Offset: 0x00000871
	private void Start()
	{
		Screen.lockCursor = true;
		Screen.showCursor = false;
		this.inverted = -1;
		base.GetComponent<GameState>().SpeedOfLight = 5.0;
		this.activated = false;
	}

	// Token: 0x06000096 RID: 150 RVA: 0x0000BD8C File Offset: 0x00009F8C
	private void LateUpdate()
	{
		if (this.activated && !base.GetComponent<GameState>().MovementFrozen)
		{
			base.GetComponent<GameState>().deltaRotation = Vector3.zero;
			if (Input.GetKeyDown(KeyCode.Y))
			{
				this.inverted *= -1;
			}
			float num = 57.29578f * Mathf.Acos(Vector3.Dot(base.GetComponent<GameState>().PlayerVelocityVector, Vector3.right) / base.GetComponent<GameState>().PlayerVelocityVector.magnitude);
			if (base.GetComponent<GameState>().PlayerVelocityVector.z > 0f)
			{
				num = -num;
			}
			Quaternion rotation = Quaternion.AngleAxis(-num, Vector3.up);
			Quaternion rotation2 = Quaternion.AngleAxis(num, Vector3.up);
			if (base.GetComponent<GameState>().PlayerVelocityVector.sqrMagnitude == 0f)
			{
				rotation = Quaternion.identity;
				rotation2 = Quaternion.identity;
			}
			Vector3 vector = Vector3.zero;
			Quaternion rotation3 = Quaternion.AngleAxis(this.camTransform.eulerAngles.y, Vector3.up);
			if (Input.GetKey(KeyCode.S))
			{
				vector += new Vector3(0f, 0f, this.acceleration * Time.deltaTime);
			}
			else if (Input.GetKey(KeyCode.W))
			{
				vector += new Vector3(0f, 0f, -this.acceleration * Time.deltaTime);
			}
			if (Input.GetKey(KeyCode.A))
			{
				vector += new Vector3(this.acceleration * Time.deltaTime, 0f, 0f);
			}
			else if (Input.GetKey(KeyCode.D))
			{
				vector += new Vector3(-this.acceleration * Time.deltaTime, 0f, 0f);
			}
			vector = rotation3 * vector;
			if (vector.x == 0f)
			{
				if ((double)base.GetComponent<GameState>().PlayerVelocityVector.x > 0.1)
				{
					vector += new Vector3(-3f * Time.deltaTime, 0f, 0f);
				}
				else if ((double)base.GetComponent<GameState>().PlayerVelocityVector.x < -0.1)
				{
					vector += new Vector3(3f * Time.deltaTime, 0f, 0f);
				}
				else if (base.GetComponent<GameState>().PlayerVelocityVector.x != 0f)
				{
					base.GetComponent<GameState>().PlayerVelocityVector = new Vector3(0f, base.GetComponent<GameState>().PlayerVelocityVector.y, base.GetComponent<GameState>().PlayerVelocityVector.z);
				}
			}
			if (vector.z == 0f)
			{
				if ((double)base.GetComponent<GameState>().PlayerVelocityVector.z > 0.1)
				{
					vector += new Vector3(0f, 0f, -3f * Time.deltaTime);
				}
				else if ((double)base.GetComponent<GameState>().PlayerVelocityVector.z < -0.1)
				{
					vector += new Vector3(0f, 0f, 3f * Time.deltaTime);
				}
				else if (base.GetComponent<GameState>().PlayerVelocityVector.z != 0f)
				{
					base.GetComponent<GameState>().PlayerVelocityVector = new Vector3(base.GetComponent<GameState>().PlayerVelocityVector.x, base.GetComponent<GameState>().PlayerVelocityVector.y, 0f);
				}
			}
			if (vector.sqrMagnitude != 0f)
			{
				Vector3 vector2 = rotation * base.GetComponent<GameState>().PlayerVelocityVector;
				vector = rotation * vector;
				float num2 = (float)base.GetComponent<GameState>().SqrtOneMinusVSquaredCWDividedByCSquared;
				vector2 = 1f / (1f + vector2.x * vector.x / (float)base.GetComponent<GameState>().SpeedOfLightSqrd) * new Vector3(vector.x + vector2.x, vector.y * num2, num2 * vector.z);
				vector2 = rotation2 * vector2;
				if (vector2.magnitude > this.maxSpeed)
				{
					vector2 = vector2.normalized * this.maxSpeed;
				}
				MonoBehaviour.print(vector2);
				base.GetComponent<GameState>().PlayerVelocityVector = vector2;
			}
			float num3 = -Input.GetAxisRaw("Mouse X");
			float num4 = (float)this.inverted * Input.GetAxisRaw("Mouse Y");
			float y = -num3 * Time.deltaTime * this.rotSpeed * 0.5f;
			float num5 = num4 * Time.deltaTime * this.rotSpeed * 0.5f;
			this.camTransform.Rotate(new Vector3(0f, y, 0f), Space.World);
			if (this.camTransform.eulerAngles.x + num5 < 90f || this.camTransform.eulerAngles.x + num5 > 270f)
			{
				this.camTransform.Rotate(new Vector3(num5, 0f, 0f));
			}
			if (base.GetComponent<GameState>().SpeedOfLight < 5.0)
			{
				base.GetComponent<GameState>().SpeedOfLight = 5.0;
			}
			Shader.SetGlobalFloat("cx", Mathf.Cos(0.017453292f * this.camTransform.rotation.eulerAngles.x * 1f));
			Shader.SetGlobalFloat("cy", Mathf.Cos(0.017453292f * this.camTransform.rotation.eulerAngles.y * 1f));
			Shader.SetGlobalFloat("cz", Mathf.Cos(0.017453292f * this.camTransform.rotation.eulerAngles.z * -1f));
			Shader.SetGlobalFloat("sx", Mathf.Sin(0.017453292f * this.camTransform.rotation.eulerAngles.x * 1f));
			Shader.SetGlobalFloat("sy", Mathf.Sin(0.017453292f * this.camTransform.rotation.eulerAngles.y * 1f));
			Shader.SetGlobalFloat("sz", Mathf.Sin(0.017453292f * this.camTransform.rotation.eulerAngles.z * -1f));
			Shader.SetGlobalFloat("_spdOfLight", (float)base.GetComponent<GameState>().SpeedOfLight);
			if (Camera.main)
			{
				Shader.SetGlobalFloat("xyr", Camera.main.pixelWidth / Camera.main.pixelHeight);
				Shader.SetGlobalFloat("xs", Mathf.Tan(0.017453292f * Camera.main.fieldOfView / 2f));
				Camera.main.layerCullSpherical = true;
				Camera.main.useOcclusionCulling = false;
			}
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				Debug.Log("load level 0 -- WinMovementScripts.LateUpdate/1");
				Application.LoadLevel(0);
			}
		}
	}

	// Token: 0x0400014D RID: 333
	public float rotSpeed;

	// Token: 0x0400014E RID: 334
	public Transform camTransform;

	// Token: 0x0400014F RID: 335
	private int inverted;

	// Token: 0x04000150 RID: 336
	public float maxSpeed = 5f;

	// Token: 0x04000151 RID: 337
	public float acceleration = 4f;

	// Token: 0x04000152 RID: 338
	public bool activated;
}
