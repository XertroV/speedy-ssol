using System;
using UnityEngine;

// Token: 0x02000007 RID: 7
public class MovementScripts : MonoBehaviour
{
    // Token: 0x06000040 RID: 64 RVA: 0x0000707C File Offset: 0x0000527C
    private void Start()
    {
        this.state = base.GetComponent<GameState>();
        Screen.lockCursor = true;
        Screen.showCursor = false;
        this.inverted = -1;
        this.audioScripts = GameObject.FindGameObjectWithTag("Playermesh").GetComponent<AudioScripts>();
        this.tStep = 0;
        this.maxSpeed = (float)this.state.MaxSpeed;
        this.frames = 0;
        if (GameObject.FindGameObjectWithTag("Audio"))
        {
            this.mouseSensitivity = GameObject.FindGameObjectWithTag("Audio").GetComponent<MyUnitySingleton>().mouseSensitivity;
        }
    }

    // Token: 0x06000041 RID: 65 RVA: 0x0000710C File Offset: 0x0000530C
    private void LateUpdate()
    {
        if (this.once)
        {
            this.orbCounts = base.GetComponent<GUIScripts>().orbCount + base.GetComponent<GUIScripts>().infraredCount;
            this.returnGrowth();
            this.once = false;
        }
        if (!this.state.MovementFrozen)
        {
            this.state.deltaRotation = Vector3.zero;
            if (Input.GetAxis("Invert Button") > 0f && !this.invertKeyDown)
            {
                this.inverted *= -1;
                this.invertKeyDown = true;
            }
            else if (Input.GetAxis("Invert Button") <= 0f)
            {
                this.invertKeyDown = false;
            }
            Vector3 playerVelocityVector = this.state.PlayerVelocityVector;
            float angle = 57.29578f * Mathf.Acos(Vector3.Dot(playerVelocityVector, Vector3.right) / playerVelocityVector.magnitude);
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.Cross(playerVelocityVector, Vector3.right).normalized);
            Quaternion rotation2 = Quaternion.AngleAxis(angle, Vector3.Cross(Vector3.right, playerVelocityVector).normalized);
            if (playerVelocityVector.sqrMagnitude == 0f)
            {
                rotation = Quaternion.identity;
                rotation2 = Quaternion.identity;
            }
            Vector3 vector = Vector3.zero;
            Quaternion rotation3 = Quaternion.AngleAxis(this.camTransform.eulerAngles.y, Vector3.up);
            float axis;
            vector += new Vector3(0f, 0f, (axis = Input.GetAxis("Vertical")) * 20f * Time.deltaTime);
            if (axis != 0f)
            {
                if (Mathf.Abs(playerVelocityVector.magnitude) < 0.1f)
                {
                    this.audioScripts.Accelerate();
                }
                this.state.keyHit = true;
                this.audioScripts.slowDown = true;
            }
            vector += new Vector3((axis = Input.GetAxis("Horizontal")) * 20f * Time.deltaTime, 0f, 0f);
            if (axis != 0f)
            {
                if (Mathf.Abs(playerVelocityVector.magnitude) < 0.1f)
                {
                    this.audioScripts.Accelerate();
                }
                this.state.keyHit = true;
                this.audioScripts.slowDown = true;
            }
            vector = rotation3 * vector;
            if (vector.x == 0f)
            {
                vector += new Vector3(-2f * playerVelocityVector.x * Time.deltaTime, 0f, 0f);
                this.audioScripts.Decelerate();
            }
            if (vector.z == 0f)
            {
                vector += new Vector3(0f, 0f, -2f * playerVelocityVector.z * Time.deltaTime);
                this.audioScripts.Decelerate();
            }
            if (vector.sqrMagnitude != 0f)
            {
                Vector3 vector2 = rotation * playerVelocityVector;
                vector = rotation * vector;
                float num = (float)this.state.SqrtOneMinusVSquaredCWDividedByCSquared;
                vector2 = 1f / (1f + vector2.x * vector.x / (float)this.state.SpeedOfLightSqrd) * new Vector3(vector.x + vector2.x, vector.y * num, num * vector.z);
                vector2 = rotation2 * vector2;
                this.state.PlayerVelocityVector = vector2;
            }
            if (this.speedOfLightTarget < 0)
            {
                this.speedOfLightTarget = 0;
            }
            if (this.state.SpeedOfLight < (double)this.speedOfLightTarget * 0.995)
            {
                this.state.SpeedOfLight += (double)this.speedOfLightStep;
            }
            else if (this.state.SpeedOfLight > (double)this.speedOfLightTarget * 1.005)
            {
                this.state.SpeedOfLight -= (double)this.speedOfLightStep;
            }
            else if (this.state.SpeedOfLight != (double)this.speedOfLightTarget)
            {
                this.state.SpeedOfLight = (double)this.speedOfLightTarget;
            }
            float num2 = -Input.GetAxisRaw("Mouse X");
            float num3 = (float)this.inverted * Input.GetAxisRaw("Mouse Y");
            float y = -num2 * Time.deltaTime * this.rotSpeed * this.mouseSensitivity;
            float num4 = num3 * Time.deltaTime * this.rotSpeed * this.mouseSensitivity;
            if (this.frames > 5)
            {
                this.camTransform.Rotate(new Vector3(0f, y, 0f), Space.World);
                if ((this.camTransform.eulerAngles.x + num4 < 90f && this.camTransform.eulerAngles.x + num4 > -90f) || (this.camTransform.eulerAngles.x + num4 > 270f && this.camTransform.eulerAngles.x + num4 < 450f))
                {
                    this.camTransform.Rotate(new Vector3(num4, 0f, 0f));
                }
            }
            this.frames++;
            if (this.state.SpeedOfLight < this.state.MaxSpeed)
            {
                this.state.SpeedOfLight = this.state.MaxSpeed;
            }
            Shader.SetGlobalFloat("_spdOfLight", (float)this.state.SpeedOfLight);
            if (Camera.main)
            {
                Shader.SetGlobalFloat("xyr", Camera.main.pixelWidth / Camera.main.pixelHeight);
                Shader.SetGlobalFloat("xs", Mathf.Tan(0.017453292f * Camera.main.fieldOfView / 2f));
                Camera.main.layerCullSpherical = true;
                Camera.main.useOcclusionCulling = false;
            }
            if ((double)this.speedOfLightTarget == this.state.MaxSpeed && !this.state.GameWin)
            {
                this.state.GameWin = true;
                this.state.WriteOutOrbSplits();
            }
        }
    }

    // Token: 0x06000042 RID: 66 RVA: 0x00007774 File Offset: 0x00005974
    public void returnGrowth()
    {
        float power = -((float)this.tStep * 6f / (float)this.orbCounts);
        int num = (int)(this.startSpeedOfLight - 1f / (1f + Mathf.Exp(power)) * (this.startSpeedOfLight - this.maxSpeed));
        this.speedOfLightTarget = num;
        if (this.tStep >= this.orbCounts)
        {
            this.speedOfLightTarget = (int)this.maxSpeed;
        }
        this.tStep++;
        this.speedOfLightStep = Mathf.Abs((float)(this.state.SpeedOfLight - (double)this.speedOfLightTarget) / 20f);
    }

    // Token: 0x040000C4 RID: 196
    private const float SLOW_DOWN_RATE = 2f;

    // Token: 0x040000C5 RID: 197
    private const float ACCEL_RATE = 20f;

    // Token: 0x040000C6 RID: 198
    private const float ACCEL_SOUND_MAX_SPEED = 0.1f;

    // Token: 0x040000C7 RID: 199
    private const int INIT_FRAME_WAIT = 5;

    // Token: 0x040000C8 RID: 200
    private const float DEGREE_TO_RADIAN_CONST = 57.29578f;

    // Token: 0x040000C9 RID: 201
    private AudioScripts audioScripts;

    // Token: 0x040000CA RID: 202
    public float rotSpeed;

    // Token: 0x040000CB RID: 203
    public Transform camTransform;

    // Token: 0x040000CC RID: 204
    private int inverted;

    // Token: 0x040000CD RID: 205
    public int speedOfLightTarget;

    // Token: 0x040000CE RID: 206
    private float speedOfLightStep;

    // Token: 0x040000CF RID: 207
    private int tStep;

    // Token: 0x040000D0 RID: 208
    private float maxSpeed;

    // Token: 0x040000D1 RID: 209
    private float startSpeedOfLight = 1600f;

    // Token: 0x040000D2 RID: 210
    public float mouseSensitivity = 0.9f;

    // Token: 0x040000D3 RID: 211
    private int orbCounts;

    // Token: 0x040000D4 RID: 212
    public bool invertKeyDown;

    // Token: 0x040000D5 RID: 213
    private int frames;

    // Token: 0x040000D6 RID: 214
    private bool once = true;

    // Token: 0x040000D7 RID: 215
    public bool allowedToMove;

    // Token: 0x040000D8 RID: 216
    private GameState state;
}
