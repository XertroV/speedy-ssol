using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000004 RID: 4
public class GameState : MonoBehaviour
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x0600000D RID: 13
	// (set) Token: 0x0600000E RID: 14
	public bool MovementFrozen
	{
		get
		{
			return this.movementFrozen;
		}
		set
		{
			this.movementFrozen = value;
		}
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x0600000F RID: 15
	// (set) Token: 0x06000010 RID: 16
	public bool GameOver
	{
		get
		{
			return this.gameOver;
		}
		set
		{
			this.gameOver = value;
		}
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000011 RID: 17
	// (set) Token: 0x06000012 RID: 18
	public bool GameWin
	{
		get
		{
			return this.gameWin;
		}
		set
		{
			this.gameWin = value;
		}
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000013 RID: 19
	public Matrix4x4 WorldRotation
	{
		get
		{
			return this.worldRotation;
		}
	}

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000014 RID: 20
	public Quaternion Orientation
	{
		get
		{
			return this.orientation;
		}
	}

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000015 RID: 21
	// (set) Token: 0x06000016 RID: 22
	public Vector3 PlayerVelocityVector
	{
		get
		{
			return this.playerVelocityVector;
		}
		set
		{
			this.playerVelocityVector = value;
		}
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x06000017 RID: 23
	// (set) Token: 0x06000018 RID: 24
	public double PctOfSpdUsing
	{
		get
		{
			return this.pctOfSpdUsing;
		}
		set
		{
			this.pctOfSpdUsing = value;
		}
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x06000019 RID: 25
	public double PlayerVelocity
	{
		get
		{
			return this.playerVelocity;
		}
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x0600001A RID: 26
	public double SqrtOneMinusVSquaredCWDividedByCSquared
	{
		get
		{
			return this.sqrtOneMinusVSquaredCWDividedByCSquared;
		}
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x0600001B RID: 27
	public double DeltaTimeWorld
	{
		get
		{
			return this.deltaTimeWorld;
		}
	}

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x0600001C RID: 28
	public double DeltaTimePlayer
	{
		get
		{
			return this.deltaTimePlayer;
		}
	}

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x0600001D RID: 29
	public double TotalTimePlayer
	{
		get
		{
			return this.totalTimePlayer;
		}
	}

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x0600001E RID: 30
	public double TotalTimeWorld
	{
		get
		{
			return this.totalTimeWorld;
		}
	}

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x0600001F RID: 31
	// (set) Token: 0x06000020 RID: 32
	public double SpeedOfLight
	{
		get
		{
			return this.c;
		}
		set
		{
			this.c = value;
			this.cSqrd = value * value;
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x06000021 RID: 33
	public double SpeedOfLightSqrd
	{
		get
		{
			return this.cSqrd;
		}
	}

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x06000022 RID: 34
	// (set) Token: 0x06000023 RID: 35
	public float OrbCounter
	{
		get
		{
			return this.orbCounter;
		}
		set
		{
			this.orbCounter = value;
		}
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x06000024 RID: 36
	// (set) Token: 0x06000025 RID: 37
	public double MaxSpeed
	{
		get
		{
			return this.maxSpeed;
		}
		set
		{
			this.maxSpeed = value;
		}
	}

	// Token: 0x06000026 RID: 38
	public void Awake()
	{
		this.playerVelocityVector = Vector3.zero;
		this.playerVelocity = 0.0;
		this.MaxSpeed = 32.0;
		this.pctOfSpdUsing = 0.625;
		this.movementFrozen = false;
		if (!GameObject.FindGameObjectWithTag("Audio"))
		{
			UnityEngine.Object.Instantiate(Resources.Load("GameObjects/StatsandSound", typeof(GameObject)), base.transform.position, base.transform.rotation);
		}
		base.GetComponentInChildren<ColorCorrectionEffect>().enabled = !GameObject.FindGameObjectWithTag("Audio").GetComponent<MyUnitySingleton>().saturated;
		this.arch = GameObject.FindGameObjectWithTag("Finish");
		this.arch.SetActiveRecursively(false);
		if (this.arch == null)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("Audio");
			for (int i = 0; i < array.Length; i++)
			{
				array[i].GetComponent<MyUnitySingleton>().gameState = this;
			}
		}
		this.orbsList = new List<GameObject>();
		GameObject[] array2 = (GameObject[])UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
		Debug.Log("Got n objects: " + array2.Length.ToString());
		foreach (GameObject gameObject in array2)
		{
			if (gameObject.name == "orb")
			{
				this.orbsList.Add(gameObject);
			}
		}
		Debug.Log("GameState.Awake: this.orbs.Length=" + this.orbsList.Count.ToString());
		this.orbs = this.orbsList.ToArray();
		this.orbState = new bool[this.orbs.Length];
		this.UpdateOrbState();
	}

	// Token: 0x06000027 RID: 39
	public void reset()
	{
		this.playerRotation.x = 0f;
		this.playerRotation.y = 0f;
		this.playerRotation.z = 0f;
		this.pctOfSpdUsing = 0.0;
	}

	// Token: 0x06000028 RID: 40
	public void OrbPicked()
	{
		this.orbCounter = 2f;
		this.pctOfSpdUsing += 0.05000000074505806;
		if (this.pctOfSpdUsing > (double)this.finalMaxSpeed)
		{
			this.pctOfSpdUsing = (double)this.finalMaxSpeed;
		}
	}

	// Token: 0x06000029 RID: 41
	public void ChangeState()
	{
		if (this.movementFrozen)
		{
			this.movementFrozen = false;
			Screen.showCursor = false;
			Screen.lockCursor = true;
			GameObject.FindGameObjectWithTag("Playermesh").GetComponent<AudioScripts>().UnFreeze();
			return;
		}
		if (!this.GameWin)
		{
			this.movementFrozen = true;
			Screen.showCursor = true;
			Screen.lockCursor = false;
			GameObject.FindGameObjectWithTag("Playermesh").GetComponent<AudioScripts>().Freeze();
			return;
		}
		if (!GameObject.FindGameObjectWithTag("Finish").GetComponent<WinLevelScript>().ending)
		{
			GameObject.FindGameObjectWithTag("Audio").GetComponent<MyUnitySingleton>().audio.PlayOneShot((AudioClip)Resources.Load("Audio/orb11", typeof(AudioClip)));
			GameObject.FindGameObjectWithTag("Finish").GetComponent<WinLevelScript>().ending = true;
		}
	}

	// Token: 0x0600002A RID: 42
	public void LateUpdate()
	{
		if (Input.GetAxis("Menu Key") > 0f && !this.menuKeyDown)
		{
			this.menuKeyDown = true;
			this.ChangeState();
		}
		else if (Input.GetAxis("Menu Key") <= 0f)
		{
			this.menuKeyDown = false;
		}
		if (!this.movementFrozen)
		{
			Shader.SetGlobalVector("_playerOffset", new Vector4(this.playerTransform.position.x, this.playerTransform.position.y, this.playerTransform.position.z, 0f));
			if (this.orbCounter <= 0f && this.pctOfSpdUsing > 0.625 && !this.GameWin)
			{
				this.pctOfSpdUsing -= (double)(0.6f * Time.deltaTime);
				if (this.pctOfSpdUsing < 0.625)
				{
					this.pctOfSpdUsing = 0.625;
				}
			}
			else if (this.orbCounter > 0f)
			{
				this.orbCounter -= Time.deltaTime;
			}
			if ((double)this.playerVelocityVector.magnitude >= (double)((float)this.MaxSpeed) * this.pctOfSpdUsing)
			{
				this.playerVelocityVector = this.playerVelocityVector.normalized * (float)this.MaxSpeed * (float)this.pctOfSpdUsing;
			}
			this.playerVelocity = (double)this.playerVelocityVector.magnitude;
			if (this.GameWin)
			{
				Shader.SetGlobalFloat("_colorShift", 0f);
			}
			else
			{
				Shader.SetGlobalFloat("_colorShift", 1f);
			}
			Shader.SetGlobalVector("_vpc", new Vector4(-this.playerVelocityVector.x, -this.playerVelocityVector.y, -this.playerVelocityVector.z, 0f) / (float)this.c);
			Shader.SetGlobalFloat("_wrldTime", (float)this.TotalTimeWorld);
			this.sqrtOneMinusVSquaredCWDividedByCSquared = Math.Sqrt(1.0 - Math.Pow(this.playerVelocity, 2.0) / this.cSqrd);
			this.deltaTimePlayer = (double)Time.deltaTime;
			if (this.keyHit)
			{
				this.totalTimePlayer += this.deltaTimePlayer;
				if (!double.IsNaN(this.sqrtOneMinusVSquaredCWDividedByCSquared))
				{
					this.deltaTimeWorld = this.deltaTimePlayer / this.sqrtOneMinusVSquaredCWDividedByCSquared;
					this.totalTimeWorld += this.deltaTimeWorld;
				}
			}
			if (!double.IsNaN(this.deltaTimePlayer) && !double.IsNaN(this.sqrtOneMinusVSquaredCWDividedByCSquared))
			{
				GameObject.FindGameObjectWithTag("Playermesh").GetComponent<Rigidbody>().velocity = -1f * (this.playerVelocityVector / (float)this.sqrtOneMinusVSquaredCWDividedByCSquared);
			}
			else
			{
				GameObject.FindGameObjectWithTag("Playermesh").GetComponent<Rigidbody>().velocity = Vector3.zero;
			}
			this.orientation = Quaternion.AngleAxis(this.playerRotation.y, Vector3.up) * Quaternion.AngleAxis(this.playerRotation.x, Vector3.right);
			Quaternion q = Quaternion.Inverse(this.orientation);
			this.Normalize(this.orientation);
			this.worldRotation = this.CreateFromQuaternion(q);
			this.playerRotation += this.deltaRotation;
			if (this.GameWin && !this.timeSet)
			{
				GameObject.FindGameObjectWithTag("Audio").GetComponent<MyUnitySingleton>().playerTime = (float)this.totalTimePlayer;
				GameObject.FindGameObjectWithTag("Audio").GetComponent<MyUnitySingleton>().worldTime = (float)this.totalTimeWorld;
				base.GetComponent<GUIScripts>().GetTimes();
				this.arch.SetActiveRecursively(true);
				Screen.showCursor = false;
				Screen.lockCursor = true;
				this.timeSet = true;
				this.pctOfSpdUsing = (double)this.finalMaxSpeed;
			}
			this.UpdateOrbState();
		}
	}

	// Token: 0x0600002B RID: 43
	public Matrix4x4 CreateFromQuaternion(Quaternion q)
	{
		double num = (double)q.w;
		double num2 = (double)q.x;
		double num3 = (double)q.y;
		double num4 = (double)q.z;
		Matrix4x4 result;
		result.m00 = (float)(Math.Pow(num, 2.0) + Math.Pow(num2, 2.0) - Math.Pow(num3, 2.0) - Math.Pow(num4, 2.0));
		result.m01 = (float)(2.0 * num2 * num3 - 2.0 * num * num4);
		result.m02 = (float)(2.0 * num2 * num4 + 2.0 * num * num3);
		result.m03 = 0f;
		result.m10 = (float)(2.0 * num2 * num3 + 2.0 * num * num4);
		result.m11 = (float)(Math.Pow(num, 2.0) - Math.Pow(num2, 2.0) + Math.Pow(num3, 2.0) - Math.Pow(num4, 2.0));
		result.m12 = (float)(2.0 * num3 * num4 + 2.0 * num * num2);
		result.m13 = 0f;
		result.m20 = (float)(2.0 * num2 * num4 - 2.0 * num * num3);
		result.m21 = (float)(2.0 * num3 * num4 - 2.0 * num * num2);
		result.m22 = (float)(Math.Pow(num, 2.0) - Math.Pow(num2, 2.0) - Math.Pow(num3, 2.0) + Math.Pow(num4, 2.0));
		result.m23 = 0f;
		result.m30 = 0f;
		result.m31 = 0f;
		result.m32 = 0f;
		result.m33 = 1f;
		return result;
	}

	// Token: 0x0600002C RID: 44
	public Quaternion Normalize(Quaternion quat)
	{
		Quaternion quaternion = quat;
		if (Math.Pow((double)quaternion.w, 2.0) + Math.Pow((double)quaternion.x, 2.0) + Math.Pow((double)quaternion.y, 2.0) + Math.Pow((double)quaternion.z, 2.0) != 1.0)
		{
			double num = Math.Sqrt(Math.Pow((double)quaternion.w, 2.0) + Math.Pow((double)quaternion.x, 2.0) + Math.Pow((double)quaternion.y, 2.0) + Math.Pow((double)quaternion.z, 2.0));
			quaternion.w = (float)((double)quaternion.w / num);
			quaternion.x = (float)((double)quaternion.x / num);
			quaternion.y = (float)((double)quaternion.y / num);
			quaternion.z = (float)((double)quaternion.z / num);
		}
		return quaternion;
	}

	// Token: 0x0600002D RID: 45
	public Vector3 TransformNormal(Vector3 normal, Matrix4x4 matrix)
	{
		Vector3 result;
		result.x = matrix.m00 * normal.x + matrix.m10 * normal.y + matrix.m20 * normal.z;
		result.y = matrix.m02 * normal.x + matrix.m11 * normal.y + matrix.m21 * normal.z;
		result.z = matrix.m03 * normal.x + matrix.m12 * normal.y + matrix.m22 * normal.z;
		return result;
	}

	// Token: 0x0600002E RID: 46
	public int NumOrbs()
	{
		return this.orbs.Length;
	}

	// Token: 0x0600002F RID: 47
	private void UpdateOrbState()
	{
		for (int i = 0; i < this.orbs.Length; i++)
		{
			bool next = this.orbs[i] == null;
			if (next && !this.orbState[i])
			{
				for (int s = 0; s < GameState.splitOn.Length; s++)
				{
					if (i == GameState.splitOn[s])
					{
						this.splits[s] = this.totalTimePlayer;
					}
				}
				this.orbCollectionList.Add(i);
			}
			this.orbState[i] = next;
		}
	}

	// Token: 0x04000051 RID: 81
	public const int splitDistance = 21000;

	// Token: 0x04000052 RID: 82
	private const float ORB_SPEED_INC = 0.05f;

	// Token: 0x04000053 RID: 83
	private const float ORB_DECEL_RATE = 0.6f;

	// Token: 0x04000054 RID: 84
	private const float ORB_SPEED_DUR = 2f;

	// Token: 0x04000055 RID: 85
	private const float MAX_SPEED = 32f;

	// Token: 0x04000056 RID: 86
	public const float NORM_PERCENT_SPEED = 0.625f;

	// Token: 0x04000057 RID: 87
	private TextWriter stateStream;

	// Token: 0x04000058 RID: 88
	private Quaternion orientation = Quaternion.identity;

	// Token: 0x04000059 RID: 89
	private Matrix4x4 worldRotation;

	// Token: 0x0400005A RID: 90
	private Vector3 playerVelocityVector;

	// Token: 0x0400005B RID: 91
	public Transform playerTransform;

	// Token: 0x0400005C RID: 92
	private bool movementFrozen;

	// Token: 0x0400005D RID: 93
	private bool gameOver;

	// Token: 0x0400005E RID: 94
	private bool gameWin;

	// Token: 0x0400005F RID: 95
	public double playerVelocity;

	// Token: 0x04000060 RID: 96
	private double deltaTimeWorld;

	// Token: 0x04000061 RID: 97
	private double deltaTimePlayer;

	// Token: 0x04000062 RID: 98
	private double totalTimePlayer;

	// Token: 0x04000063 RID: 99
	private double totalTimeWorld;

	// Token: 0x04000064 RID: 100
	private double c = 200.0;

	// Token: 0x04000065 RID: 101
	public double totalC = 200.0;

	// Token: 0x04000066 RID: 102
	public double maxPlayerSpeed;

	// Token: 0x04000067 RID: 103
	private double maxSpeed;

	// Token: 0x04000068 RID: 104
	private double cSqrd;

	// Token: 0x04000069 RID: 105
	private float orbCounter;

	// Token: 0x0400006A RID: 106
	private bool timeSet;

	// Token: 0x0400006B RID: 107
	public bool menuKeyDown;

	// Token: 0x0400006C RID: 108
	private double sqrtOneMinusVSquaredCWDividedByCSquared;

	// Token: 0x0400006D RID: 109
	public Vector3 playerRotation = new Vector3(0f, 0f, 0f);

	// Token: 0x0400006E RID: 110
	public Vector3 deltaRotation = new Vector3(0f, 0f, 0f);

	// Token: 0x0400006F RID: 111
	public double pctOfSpdUsing;

	// Token: 0x04000070 RID: 112
	public bool rotated;

	// Token: 0x04000071 RID: 113
	private GameObject arch;

	// Token: 0x04000072 RID: 114
	public float finalMaxSpeed = 0.99f;

	// Token: 0x04000073 RID: 115
	public bool keyHit;

	// Token: 0x04000074 RID: 116
	public GameObject[] orbs;

	// Token: 0x04000075 RID: 117
	public List<GameObject> orbsList;

	// Token: 0x04000076 RID: 118
	public bool[] orbState;

	// Token: 0x0400052D RID: 1325
	public double[] splits = new double[RouteWr20200921Long.wrSplits.Length];

	// Token: 0x0400052E RID: 1326
	public static int[] splitOn = RouteWr20200921Long.splitOn;

	// Token: 0x040008F4 RID: 2292
	public static string[] splitNames = RouteWr20200921Long.splitNames;

	// Token: 0x04001B1F RID: 6943
	public static float[] wrSplits = RouteWr20200921Long.wrSplits;

	// Token: 0x04001EC9 RID: 7881
	public List<int> orbCollectionList = new List<int>();
}
