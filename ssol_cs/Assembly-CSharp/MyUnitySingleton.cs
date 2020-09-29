using System;
using UnityEngine;

// Token: 0x0200000B RID: 11
public class MyUnitySingleton : MonoBehaviour
{
    public MenuComponentSelectSplits SelectSplits { get; private set; }
    // Token: 0x17000012 RID: 18
    // (get) Token: 0x0600005E RID: 94 RVA: 0x000024C6 File Offset: 0x000006C6
    public static MyUnitySingleton Instance
    {
        get
        {
            return MyUnitySingleton.instance;
        }
    }

    // Token: 0x0600005F RID: 95 RVA: 0x00009A44 File Offset: 0x00007C44
    private void Awake()
    {
        if (MyUnitySingleton.instance != null && MyUnitySingleton.instance != this)
        {
            MyUnitySingleton.instance.audio.Stop();
            MyUnitySingleton.instance.fadeOut = false;
            MyUnitySingleton.instance.audio.clip = (AudioClip)Resources.Load("Audio/Relativity_Music", typeof(AudioClip));
            MyUnitySingleton.instance.audio.Play();
            MyUnitySingleton.instance.end = false;
            UnityEngine.Object.DestroyImmediate(base.gameObject);
            return;
        }
        if (GameObject.FindGameObjectsWithTag("Audio").Length > 1)
        {
            MyUnitySingleton.instance.audio.Stop();
            MyUnitySingleton.instance.fadeOut = false;
            MyUnitySingleton.instance.audio.clip = (AudioClip)Resources.Load("Audio/Relativity_Music", typeof(AudioClip));
            MyUnitySingleton.instance.audio.Play();
            MyUnitySingleton.instance.end = false;
            UnityEngine.Object.DestroyImmediate(base.gameObject);
            return;
        }
        MyUnitySingleton.instance = this;
        UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 60;

        SelectSplits = new MenuComponentSelectSplits();
    }

    // Token: 0x06000060 RID: 96 RVA: 0x00009B5C File Offset: 0x00007D5C
    private void Update()
    {
        if (!this.fadeOut && MyUnitySingleton.instance != null && !this.end)
        {
            MyUnitySingleton.instance.audio.volume = this.volume;
        }
        else if (this.fadeOut && base.audio.volume > 0f)
        {
            base.audio.volume -= Time.deltaTime;
        }
        else if (this.fadeOut && base.audio.volume <= 0f)
        {
            base.audio.Stop();
        }
        if (!this.fadeOut && this.end && base.audio.volume <= 0.6f * this.volume)
        {
            base.audio.volume += 0.4f * Time.deltaTime;
        }
    }

    // Token: 0x06000061 RID: 97 RVA: 0x00009C3C File Offset: 0x00007E3C
    public void PlayEndMusic()
    {
        this.fadeOut = false;
        this.end = true;
        base.audio.volume = 0f;
        base.audio.clip = (AudioClip)Resources.Load("Audio/Ending_Music", typeof(AudioClip));
        base.audio.loop = true;
        base.audio.Play();
    }

    // Token: 0x04000108 RID: 264
    public AudioSource effect;

    // Token: 0x04000109 RID: 265
    public bool fadeOut;

    // Token: 0x0400010A RID: 266
    public float playerTime;

    // Token: 0x0400010B RID: 267
    public float worldTime;

    // Token: 0x0400010C RID: 268
    public float mouseSensitivity = 0.9f;

    // Token: 0x0400010D RID: 269
    public float volume = 0.5f;

    // Token: 0x0400010E RID: 270
    public bool saturated = true;

    // Token: 0x0400010F RID: 271
    public bool end;

    // Token: 0x04000110 RID: 272
    private static MyUnitySingleton instance;

    // Token: 0x04000111 RID: 273
    public GameState gameState;
}
