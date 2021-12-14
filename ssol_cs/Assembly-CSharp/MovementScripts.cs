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
            // NOTE: I reverse-engineered some of the logic to do some comments here and rename some vars -- forgetting that a lot of it is already in OpenRelativity -- check there before trying to read all this if you want to know what's going on.
            Vector3 playerVelocityVector = this.state.PlayerVelocityVector;
            // what is this angle?
            // dot: angle between player's velocity and Right
            // divided by magnitude (normalized-ish)
            // acos: get angle
            // 57.29578f = 180/pi    => converts radians to degrees
            // angle = angle between player and axis Right
            float angle = 57.29578f * Mathf.Acos(Vector3.Dot(playerVelocityVector, Vector3.right) / playerVelocityVector.magnitude);
            // rotate angle degrees around cross of velocity and axis Right
            // rotate -angle degrees around cross of velocity and axis Right
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.Cross(playerVelocityVector, Vector3.right).normalized);
            Quaternion rotation2 = Quaternion.AngleAxis(angle, Vector3.Cross(Vector3.right, playerVelocityVector).normalized);
            if (playerVelocityVector.sqrMagnitude == 0f)
            {
                rotation = Quaternion.identity;
                rotation2 = Quaternion.identity;
            }
            // Track acceleration input
            Vector3 accelVector = Vector3.zero;
            // rotate some degrees around y axis -- this is the rotation for the camera
            Quaternion camRotation = Quaternion.AngleAxis(this.camTransform.eulerAngles.y, Vector3.up);
            float axis;
            // Add to z axis accel
            accelVector += new Vector3(0f, 0f, (axis = Input.GetAxis("Vertical")) * ACCEL_RATE * Time.deltaTime);
            if (axis != 0f)
            {
                if (Mathf.Abs(playerVelocityVector.magnitude) < 0.1f)
                {
                    this.audioScripts.Accelerate();
                }
                this.state.keyHit = true;
                this.audioScripts.slowDown = true;
            }
            // Add to x axis accel
            accelVector += new Vector3((axis = Input.GetAxis("Horizontal")) * ACCEL_RATE * Time.deltaTime, 0f, 0f);
            if (axis != 0f)
            {
                if (Mathf.Abs(playerVelocityVector.magnitude) < 0.1f)
                {
                    this.audioScripts.Accelerate();
                }
                this.state.keyHit = true;
                this.audioScripts.slowDown = true;
            }
            // rotate accel vector by camera rotation
            accelVector = camRotation * accelVector;
            if (accelVector.x == 0f)
            {
                accelVector += new Vector3(-SLOW_DOWN_RATE * playerVelocityVector.x * Time.deltaTime, 0f, 0f);
                this.audioScripts.Decelerate();
            }
            if (accelVector.z == 0f)
            {
                accelVector += new Vector3(0f, 0f, -SLOW_DOWN_RATE * playerVelocityVector.z * Time.deltaTime);
                this.audioScripts.Decelerate();
            }
            // if the acceleration vector has a magnitude
            // I think this entire block just handles speed of light stuff??
            if (accelVector.sqrMagnitude != 0f)
            {
                // vector2 = rotate playerVelocity `angle` degrees around up/down
                Vector3 vPlayerVel = rotation * playerVelocityVector;
                // rotate accel vector `angle` degress around up/down
                accelVector = rotation * accelVector;
                // all velocity now in one direction????
                //
                float num = (float)this.state.SqrtOneMinusVSquaredCWDividedByCSquared;
                vPlayerVel = 1f / (1f + vPlayerVel.x * accelVector.x / (float)this.state.SpeedOfLightSqrd) * new Vector3(accelVector.x + vPlayerVel.x, accelVector.y * num, num * accelVector.z);
                vPlayerVel = rotation2 * vPlayerVel;
                this.state.PlayerVelocityVector = vPlayerVel;
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
            float mouseX = -Input.GetAxisRaw("Mouse X");
            float mouseY = (float)this.inverted * Input.GetAxisRaw("Mouse Y");
            float y = -mouseX * Time.deltaTime * this.rotSpeed * this.mouseSensitivity;
            float num4 = mouseY * Time.deltaTime * this.rotSpeed * this.mouseSensitivity;
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
            //Debug.Log($"Camera FOV: {Camera.main.fieldOfView}");
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
        this.speedOfLightStep = Mathf.Abs((float)(this.state.SpeedOfLight - (double)this.speedOfLightTarget) / ACCEL_RATE);
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
