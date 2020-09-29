using System;
using UnityEngine;

// Token: 0x02000009 RID: 9
public class AudioScripts : MonoBehaviour
{
	// Token: 0x06000051 RID: 81 RVA: 0x00008838 File Offset: 0x00006A38
	private void Start()
	{
		if (GameObject.FindGameObjectWithTag("Audio"))
		{
			this.volume = GameObject.FindGameObjectWithTag("Audio").GetComponent<MyUnitySingleton>().volume;
		}
		this.state = GameObject.FindGameObjectWithTag("Player").GetComponent<GameState>();
		this.sources = base.GetComponents<AudioSource>();
		if (this.pickupSounds.Length == 0)
		{
			this.pickupSounds = new AudioClip[1];
			this.pickupSounds[0] = (AudioClip)Resources.Load("Audio/Orb_Pickup_1", typeof(AudioClip));
		}
		if (this.movementSounds.Length == 0)
		{
			this.movementSounds = new AudioClip[1];
			this.movementSounds[0] = (AudioClip)Resources.Load("Audio/Orb_Pickup_1", typeof(AudioClip));
		}
		for (int i = 0; i < this.movementSounds.Length; i++)
		{
			this.sources[i].clip = this.movementSounds[i];
		}
		this.sources[3].volume = 0f;
		this.sources[3].loop = true;
		this.paused = new bool[this.sources.Length];
	}

	// Token: 0x06000052 RID: 82 RVA: 0x0000896C File Offset: 0x00006B6C
	private void Update()
	{
		if (this.unFrozen)
		{
			if (this.velocityCounter > 0f)
			{
				this.velocityCounter -= Time.deltaTime;
				if (this.velocityCounter <= 0f && !this.sources[1].isPlaying)
				{
					this.sources[1].Play();
					this.fadeIn = 0.1f;
					this.sources[1].loop = true;
				}
			}
			if (this.fadeIn < 0.5f)
			{
				this.fadeIn += Time.deltaTime;
				this.sources[1].volume = Mathf.Min(this.fadeIn, 0.5f) * this.volume;
				this.sources[0].volume = (0.5f - this.fadeIn) * this.volume;
			}
			else
			{
				this.sources[1].volume = 0.5f * (float)this.state.PlayerVelocity / (float)this.state.MaxSpeed * this.volume;
				if ((float)this.state.PlayerVelocity / (float)this.state.MaxSpeed >= 0.9f)
				{
					if (!this.sources[3].isPlaying)
					{
						this.sources[3].Play();
						this.maxFadeIn = 1f;
						this.maxFadeOut = 1f;
					}
					else if (this.maxFadeIn > 0f)
					{
						this.sources[3].volume = (1f - this.maxFadeIn) * 0.6f * this.volume;
						this.maxFadeIn -= 0.3f * Time.deltaTime;
					}
				}
				else if (this.sources[3].isPlaying)
				{
					if (this.maxFadeOut <= 0f)
					{
						this.sources[3].Stop();
					}
					else
					{
						this.sources[3].volume = this.maxFadeOut * 0.6f * this.volume;
						this.maxFadeOut -= 0.3f * Time.deltaTime;
					}
					this.maxFadeIn = 0f;
				}
			}
		}
	}

	// Token: 0x06000053 RID: 83 RVA: 0x00008BB8 File Offset: 0x00006DB8
	public void Accelerate()
	{
		if (!this.sources[0].isPlaying)
		{
			this.sources[0].volume = 0.2f * this.volume;
			this.sources[0].Play();
			this.velocityCounter = 1.5f;
		}
		this.slowDown = true;
	}

	// Token: 0x06000054 RID: 84 RVA: 0x00008C10 File Offset: 0x00006E10
	public void Decelerate()
	{
		if (!this.sources[2].isPlaying && this.slowDown)
		{
			this.sources[2].volume = 0.1f * this.volume;
			this.sources[2].Play();
		}
		this.slowDown = false;
	}

	// Token: 0x06000055 RID: 85 RVA: 0x00008C68 File Offset: 0x00006E68
	private void OnTriggerEnter(Collider other)
	{
		if (this.pickupSounds != null && this.count < 99)
		{
			this.sources[this.sources.Length - 1].volume = 0.1f * this.volume;
			this.sources[this.sources.Length - 1].PlayOneShot(this.pickupSounds[UnityEngine.Random.Range(0, Mathf.Max(this.pickupSounds.Length - 1, 0))]);
			this.count++;
		}
		else if (this.count == 99)
		{
			this.sources[this.sources.Length - 1].volume = 0.3f * this.volume;
			this.sources[this.sources.Length - 1].PlayOneShot(this.pickupSounds[this.pickupSounds.Length - 1]);
		}
	}

	// Token: 0x06000056 RID: 86 RVA: 0x00008D4C File Offset: 0x00006F4C
	public void Freeze()
	{
		for (int i = 0; i < this.sources.Length; i++)
		{
			if (this.sources[i].isPlaying)
			{
				this.sources[i].Pause();
				this.paused[i] = true;
			}
		}
		this.unFrozen = false;
	}

	// Token: 0x06000057 RID: 87 RVA: 0x00008DA4 File Offset: 0x00006FA4
	public void UnFreeze()
	{
		for (int i = 0; i < this.sources.Length; i++)
		{
			if (this.paused[i])
			{
				this.sources[i].Play();
				this.paused[i] = false;
			}
		}
		this.unFrozen = true;
	}

	// Token: 0x040000E0 RID: 224
	public AudioClip[] movementSounds;

	// Token: 0x040000E1 RID: 225
	public AudioClip[] pickupSounds;

	// Token: 0x040000E2 RID: 226
	private AudioSource[] sources;

	// Token: 0x040000E3 RID: 227
	private GameState state;

	// Token: 0x040000E4 RID: 228
	private float velocityCounter;

	// Token: 0x040000E5 RID: 229
	private float fadeIn = 1f;

	// Token: 0x040000E6 RID: 230
	public bool slowDown;

	// Token: 0x040000E7 RID: 231
	public float maxFadeIn;

	// Token: 0x040000E8 RID: 232
	public float maxFadeOut;

	// Token: 0x040000E9 RID: 233
	private bool unFrozen = true;

	// Token: 0x040000EA RID: 234
	private bool[] paused;

	// Token: 0x040000EB RID: 235
	public float volume = 0.5f;

	// Token: 0x040000EC RID: 236
	private int count;
}
