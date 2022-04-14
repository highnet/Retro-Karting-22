using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class KartController : MonoBehaviour
{
    public bool controllable; // the status of wether the kart is controllable by the user or not
    public Rigidbody rigidBody; // the kart's rigid body
    private float deltaThrustForce; // the delta thrust force
    public float currentThrustForce; // the current thrust force
    private float deltaRotate; // the delta thrust force
    public float currentRotate; // the current rotate
    public float baseMaxThrustForce; // the base max thrust force
    public float thrustForceInterpolationTime; // the thrust force interpolation time
    public float currentMaxThrustForce; // the current max thrust force
    public float baseSteeringStrength; // the base steering strength
    public float gravity; // the gravity
    public float minimumSteerAmount; // the minimum steer amount
    public float maximumSteerAmount; // the maximum steer mount
    public float speedThreshold0; // the speed threshold 0
    public float speedThreshold1; // the speed threshold 1
    public bool headLightsOn; // the status of the head lights
    public bool brakeLightsOn; // the status of the brake lights
    public List<GameObject> headLights; // the list of head light gameobjects
    public List<GameObject> brakeLights; // the list of brake light gameobjects
    public int usableSpeedups; // the number of usable speedups
    public int coinCount; // the coin count
    public Powerup powerup; // the powerup script
    public bool drifting; // the status of wether the kart is drifting or not
    public float driftDirection; // the direction of drifting
    public float baseDriftingStrength; // the base drifting strength
    public bool grounded; // the status of grounded weher the kart is grounded or not
    public List<GameObject> frontWheels; // the list of front wheel game objects
    public List<GameObject> backWheels; // the list of back wheel game objects
    public Animator animator; // the character's animator
    public Transform kartNormal; // the normal of the kart
    public List<VisualEffect> driftParticles; // the list of drift particle visual effects
    public List<TrailRenderer> skidMarks; // the list of skid mark trail renderers
    public ParticleSystem kartSmoke; // the kart smoke particle system
    public ParticleSystem dust; // the dust particle system
    public ParticleSystem engineFire; // the engine fire particle system
    public ParticleSystem landingDust; // the landing dust particle system
    public float driftTimer; // the drift timer
    public float driftTimerThreshold0; // the drift timer threshold 0
    public float driftTimerThreshold1; // the drift timer threshold 1
    public float driftTimerThreshold2; // the drift timer threshold 2
    public float driftTimerThreshold0SpeedBoostForce; // the drift timer threshold 0 boost force
    public float driftTimerThreshold1SpeedBoostForce; // the drift timer threshold 1 boost force
    public float driftTimerThreshold2SpeedBoostForce; // the drift timer threshold 2 boost force
    public float driftCooldown; // the drift cooldown
    public float driftCooldownTimer; // the drift cooldown timer
    public CameraManager cameraManager; // the camera manager script
    public float baseMaxforwardVelocity; // the base max forward velocity
    public float currentMaxForwardVelocity; // the current max forward velocity
    public float maxReverseVelocity; // the max reverse velocity
    public Transform parent; // the kart controller's parent gameobject transform
    public bool hit; // status of wether or not the raycast used to check if the kart is grounded hit or missed
    public AudioClipPlayer characterClipPlayer; // the character audio clip player
    public AudioClipPlayer kartClipPlayer; // the kart audio clip player
    public RacingUIController racingUIController; // the racing ui controller
    public bool burstDriftParticlesEnabled; // the status of wether the burst drift particles are enabled or not
    public int currentEngineStage; // the current engine state
    public int previousEngineStage; // the previous engine state
    public bool engineTransitionBlocked; // the status of wether the engine transition is blocked or not
    public bool engineIsRunning; // the status of wether the engine is running or not
    public bool braking; // the status of wether the kart is braking ro not
    public bool brakeSoundPlayed; // the status of wether the brake sound has already placed once or not
    public OutOfBounds outOfBounds;
    public bool respawning;
    public GhostRacerSaver ghostRacerSaver;
    public RaceController raceController;


    void Start()
    {
        currentMaxForwardVelocity = baseMaxforwardVelocity; // set the current max forward velocity to the base max forward velocity
        currentMaxThrustForce = baseMaxThrustForce; // set the current max thrust force to the base max thrust force
        racingUIController = FindObjectOfType<RacingUIController>(); // store a local reference to the racing ui controller
        outOfBounds = FindObjectOfType<OutOfBounds>();
        raceController = FindObjectOfType<RaceController>();
        ghostRacerSaver = FindObjectOfType<GhostRacerSaver>();
    }

    void Update()
    {
        float steerInput = Input.GetAxis("Horizontal"); // query the steer input
        bool throttle = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W); // query the throttle input
        bool reverse = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S); // query the reverse input
        int steerDirection = steerInput > 0 ? 1 : -1; // calculate the steer direction
        float absoluteSteeringInput = Mathf.Abs(steerInput); // calculate the absolute steering input

        transform.position = rigidBody.transform.position - new Vector3(0, 2f, 0); // teleport to our rigidbody with an offset
        RaycastHit hitDown; // create a raycast that will be used to check wether ot not the kart is grounded or floating
        hit = Physics.Raycast(transform.position + transform.up, Vector3.down, out hitDown, 1.5f); // raycast downwards and check if we are grounded or floating
        if (hit && !grounded) // check if we hit and we are currently flagged as not grounded
        {
            landingDust.Play(); // play the landing dust animation
            kartClipPlayer.PlayOneShot(0, 9, 1.0f, true); // play the landing sound
        }
        if (hit) // check if the raycast hit
        {
            grounded = true; // flag grounded as true
        }
        if (!hit) // check if the raycast missed
        {
            grounded = false; // flag grounded as false
        }
        if (engineIsRunning) // check if the engine is running
        { 
            StartCoroutine("BlendEngineSoundsCoroutine"); // start the engine sound blending co routine
        }
        if (!controllable) { return; } // return if the kart is not controllable
        currentMaxThrustForce = baseMaxThrustForce + (coinCount * powerup.coinSpeedStrength); // calculate the current max thrust force
        animator.SetFloat("Blend", Remap(steerInput, -1, 1, 0, 1)); // set the animator blend tree float value
        if (rigidBody.velocity.magnitude < speedThreshold0) // check if the rigidbody velocity magnitude lies below the speed threshold 0
        {
            absoluteSteeringInput *= 0; // set the steering input to zero
        }
        else if (rigidBody.velocity.magnitude < speedThreshold1) // check if the rigidbody velocity magnitude lies below the speed threshold 1
        {
            absoluteSteeringInput *= 1; // leave the steering input untouched
        }
        else // otherwise
        {
            absoluteSteeringInput = Mathf.Clamp(Mathf.Abs(steerInput) * (1 / Mathf.Log10(rigidBody.velocity.magnitude)), minimumSteerAmount, maximumSteerAmount); // calculate the absolute steering input at higher velocities
        }
        if (steerInput != 0) // check if the steer input is zero
        {
            if (!reverse) // if we are not going in reverse
            {
                deltaRotate = steerDirection * baseSteeringStrength * absoluteSteeringInput; // calculate the delta rotate
            }
            else // otherwise
            {
                deltaRotate = -steerDirection * baseSteeringStrength * absoluteSteeringInput; // calculate the delta rotate
            }
        }

        if (Input.GetKeyUp(KeyCode.Space)) // check for spacebar up input
        {
            braking = false; // flag braking as false
            brakeSoundPlayed = false; // flag brake sound played as false
        }

        if (Input.GetKey(KeyCode.Space))  // check for spacebar input
        {
            braking = true; // flag braking as true
            if (!brakeSoundPlayed) // check if the brake sound is flagged as it hasnt played yet
            {
                kartClipPlayer.PlayOneShot(2, 10, 1.0f, true); // play the kart braking sound
                brakeSoundPlayed = true; // flag the brake sound as played
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl)) // check for left ctrl up or right ctrl up input
        {
            drifting = false;  // flag drifting as true
            driftParticles[0].SetFloat("Spawn Rate", 1f); // set the spawn rate of drift particles 0 to 1
            driftParticles[1].SetFloat("Spawn Rate", 1f); // set the spawn rate of drift particles 1 to 1
            driftParticles[2].SetFloat("Spawn Rate", 1f); // set the spawn rate of drift particles 2 to 1
            driftParticles[3].SetFloat("Spawn Rate", 1f); // set the spawn rate of drift particles 3 to 1
            driftParticles[4].SetFloat("Spawn Rate", 1f); // set the spawn rate of drift particles 4 to 1
            driftParticles[5].SetFloat("Spawn Rate", 1f); // set the spawn rate of drift particles 5 to 1
            foreach (VisualEffect vfx in driftParticles) // for each drift particle visual effect
            {
                vfx.gameObject.SetActive(false); // deactivate the visual effect
            }
            burstDriftParticlesEnabled = false; // flag burst drift particles as disabled
            if (driftTimer > driftTimerThreshold2) // if the drift timer is greater than the drift timer threshold 2
            {
                SpeedBoost(driftTimerThreshold2SpeedBoostForce, Color.red, 0.8f, 0.8f, 1.2f, 25, false, false, ForceMode.Acceleration); // do a speed boost
            }
            else if (driftTimer > driftTimerThreshold1) // if the drift timer is greater than the drift timer threshold 1
            {
                SpeedBoost(driftTimerThreshold1SpeedBoostForce, Color.yellow, 1.0f, 1.0f, 1.0f, 15, false, false, ForceMode.Acceleration); // do a speed boost
            }
            else if (driftTimer > driftTimerThreshold0) // if the drift timer is greater than the drift timer threshold 0
            {
                SpeedBoost(driftTimerThreshold0SpeedBoostForce, Color.white, 1.2f, 1.2f, 1.8f, 10, false, false, ForceMode.Acceleration); // do a speed boost
            }
            driftTimer = 0; // reset the drift timer to 0
        }
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) // check for left ctrl or right ctrl key input ...
            && !drifting && driftCooldownTimer == 0  // ... and if we are not drifting and the drift cooldown timer is zero (off cooldown) ...
            && Input.GetAxis("Horizontal") != 0  // ... and the horizontal input is nonzero ..
            && !reverse && throttle && !braking) // and we are not going in reverse and we are throttling and we are not braking
        {
            drifting = true; // flag drifting as true
            driftDirection = Input.GetAxis("Horizontal") > 0 ? 1 : -1; // calculate the drift direction
            kartClipPlayer.PlayOneShot(1, 11, 1.0f, true); // play the kart drift sound
            if (grounded) // check if we are grounded
            {
                rigidBody.AddForce(Vector3.up * 500f, ForceMode.Acceleration); // add a upwards force to the rigid body
            }
            driftCooldownTimer = driftCooldown; // set the drift cooldown timer to the drift cooldown
        }
        if (drifting) // check if we are drifting
        {
            if (driftDirection == steerDirection) // check if the drift direction equals the steer direction
            {
                deltaRotate += driftDirection * baseDriftingStrength * absoluteSteeringInput; // calculate the delta rotation
            }
            else // otherwise
            {
                deltaRotate /= 2.5f; // reduce the delta rotation
            }
            driftTimer += Time.deltaTime; // increment the drift timer
            if (driftTimer > driftTimerThreshold0) // check if the drift timer is greater than the drift timer threshold 0
            {
                if (!burstDriftParticlesEnabled) // check if the burst drift particles are flagged as disabled
                {
                    driftParticles[0].gameObject.SetActive(true); // activate the drift particles 0
                    driftParticles[1].gameObject.SetActive(true); // activate the drift particles 1
                    driftParticles[0].SetFloat("Spawn Rate", 5f); // set the drift particles 0 spawn rate to 5
                    driftParticles[1].SetFloat("Spawn Rate", 5f); // set the dirft particles 1 spawn rate to 5
                    int random1 = (int)UnityEngine.Random.Range(0f, 3f); // calculate a random integer between 0 and 2
                    int random2 = (int)UnityEngine.Random.Range(0f, 3f); // calculate another random integer between 0 and 2
                    switch (random1) // random1 fork
                    {
                        case 0:
                            driftParticles[6].gameObject.SetActive(true); // activate drift particles 6
                            break;
                        case 1:
                            driftParticles[8].gameObject.SetActive(true); // activate drift particles 8
                            break;
                        case 2:
                            driftParticles[10].gameObject.SetActive(true); // activate drift particles 10
                            break;
                    }
                    switch (random2) // random2 fork
                    {
                        case 0:
                            driftParticles[7].gameObject.SetActive(true); // activate drift particles 7
                            break;
                        case 1:
                            driftParticles[9].gameObject.SetActive(true); // activate drift particles 9
                            break;
                        case 2:
                            driftParticles[11].gameObject.SetActive(true); // activate drift particles 11
                            break;
                    }
                }
                burstDriftParticlesEnabled = true; // flag the brust drift particles as enabled
            }
            if (driftTimer > driftTimerThreshold1) // check if the drift timer is greater than the drift timer threshold 1
            {
                driftParticles[2].gameObject.SetActive(true); // activate drift particles 2
                driftParticles[3].gameObject.SetActive(true); // activate drift particles 3
                driftParticles[0].SetFloat("Spawn Rate", 10f); // set the drift particles 0 spawn rate to 10
                driftParticles[1].SetFloat("Spawn Rate", 10f); // set the drift particles 1 spawn rate to 10
                driftParticles[2].SetFloat("Spawn Rate", 5f); // set the drift particles 2 spawn rate to 5
                driftParticles[3].SetFloat("Spawn Rate", 5f); // set the drift particles 3 spawn rate to 5
            }
            if (driftTimer > driftTimerThreshold2) // check if the drift timer is greater than the drift timer threshold 2
            {
                driftParticles[4].gameObject.SetActive(true); // activate drift particles 4
                driftParticles[5].gameObject.SetActive(true); // activate drift particles 5
                driftParticles[0].SetFloat("Spawn Rate", 15f); // set the drift particles 0 spawn rate to 15
                driftParticles[1].SetFloat("Spawn Rate", 15f); // set the drift particles 1 spawn rate to 15
                driftParticles[2].SetFloat("Spawn Rate", 20f); // set the drift particles 2 spawn rate to 20
                driftParticles[3].SetFloat("Spawn Rate", 20f); // set the drift particles 3 spawn rate to 20
                driftParticles[4].SetFloat("Spawn Rate", 25f); // set the drift particles 4 spawn rate to 25
                driftParticles[5].SetFloat("Spawn Rate", 25f); // set the drift particles 5 spawn rate to 25
            }

        }
        if (grounded && // check if we are grounded and ...
            (braking || drifting || absoluteSteeringInput > 0.6f || rigidBody.velocity.magnitude > currentMaxThrustForce - 30 && absoluteSteeringInput > 0.5f)) // ... are braking or drifting or the absolute steering input is greater than 0.6 or the rigid velocit magnitude is greater than the current max thrust force - 30 and the absolute steering input is greater than 0.5
        {
            for (int i = 0; i < skidMarks.Count; i++) // for each skidmark
            {
                skidMarks[i].emitting = true; // begin emitting the skid mark
            }
        }
        else // otherwise
        {
            for (int i = 0; i < skidMarks.Count; i++) // for each skidmark
            {
                skidMarks[i].emitting = false; // stop emitting the skid mark
            }
        }
        currentRotate = Mathf.Lerp(currentRotate, deltaRotate, Time.deltaTime * 4f); // calculate the current rotate
        deltaRotate = 0f; // set the delta rotate to 0 for next frame
        if (throttle && !braking && rigidBody.velocity.magnitude < currentMaxForwardVelocity) // if we are throttling and not braking and the rigidbody velocity magnitude is less than the current max forward velocity
        {
            deltaThrustForce = currentMaxThrustForce; // set the delta thrust force to the current max thrust force
        }
        else if (reverse && !braking && rigidBody.velocity.magnitude < maxReverseVelocity) // otherwise if we are reversing and not braking and the rigidbody velocity magnitude is less than the max reverse velocity
        {
            deltaThrustForce = -currentMaxThrustForce; // set the delta thrust force to the negative current max thrust force
        }
        if (grounded && absoluteSteeringInput > 0) // if we are grounded and the absolute steering input is greater than zero
        {
            if (!drifting) // if we are not drifting
            {
                currentThrustForce -= absoluteSteeringInput * .1f; // slow down the car 
                deltaThrustForce -= absoluteSteeringInput * .1f; // slow down the car 
            }
            else // otherwise
            {
                currentThrustForce -= absoluteSteeringInput * .1f; // slow down the car 
                deltaThrustForce -= absoluteSteeringInput * .1f; // slow down the car 
            }
        }
        if (throttle || reverse) // if we are throttling or reversing
        {
            if (grounded) // if we are grounded
            {
                if (!dust.isPlaying) // if the dust particle system is not playing
                {
                    dust.Play(); // play the dust
                }
            }
        }
        else // otherwise 
        {
            dust.Stop(); // stop the dust
        }
        var smoke = kartSmoke.emission; // get the kart smoke emission
        if (!drifting) // if we are not drifting
        {
            smoke.rateOverTime = Remap(Mathf.Abs(currentThrustForce), 0.0f, currentMaxThrustForce, 10.0f, 300.0f); // set the smoke rate over time
        }
        else // otherwise
        {
            smoke.rateOverTime = Remap(Mathf.Abs(currentThrustForce), 0.0f, currentMaxThrustForce, 100.0f, 600.0f); // set the smoke rate over time
        }
        if (drifting && driftTimer > driftTimerThreshold2) // if we are drifting and the drift timer is greater than the drift timer threshold 2
        {
            deltaThrustForce /= ((driftTimer/2) * 0.8f); // slow down the car
        }

        if (braking) // if we are braking
        {
            deltaThrustForce *= .9f; // slow down the car
        }

        if (!grounded) // if we are not grounded
        {
            deltaThrustForce = 0; // slow down the car
        }
        currentThrustForce = Mathf.SmoothStep(currentThrustForce, deltaThrustForce, Time.deltaTime * thrustForceInterpolationTime); // calculate the current thrust force
        deltaThrustForce = 0f; // reset the delta thrust force for next update
        if (Input.GetKeyUp(KeyCode.L)) // check for L key input
        {
            if (headLightsOn) // check if the headlights are on
            {
                kartClipPlayer.PlayOneShot(5, 0, 1.0f, true); // play the head lights turning off sound
            }
            else // otherwise
            {
                kartClipPlayer.PlayOneShot(5, 1, 1.0f, true); // play the head lights turning on sound
            }
            headLightsOn = !headLightsOn; // toggle the headlightson flag
            foreach (GameObject light in headLights) // for each head light game object
            {
                light.gameObject.SetActive(headLightsOn); // update the activity status of the head light
            }
        }
        if (braking) // check if we are braking
        {
            brakeLightsOn = true; // flag brakelightson as true
        }
        else // otherwise
        {
            brakeLightsOn = false; // flag brakelightson as false
        }
        foreach (GameObject light in brakeLights) // for each brakelight gameobject
        {
            light.gameObject.SetActive(brakeLightsOn); // update the activity status of the brake lights
        }
        if (Input.GetKeyDown(KeyCode.Return) && usableSpeedups > 0) // check for return key down input and if usable speed ups is greater than zero
        {
            this.GetComponent<Powerup>().DoSpeedUp(); // activate the usable speedup
            usableSpeedups--; // decrement the number of usable speedups
            racingUIController.usableSpeedsupText.text = usableSpeedups.ToString(); // update the UI to reflect the new number of usable speedups
        }
        foreach (GameObject wheel in frontWheels) // for each front wheel
        {
            wheel.transform.Rotate(new Vector3(0f, 0f, -rigidBody.velocity.magnitude)); // rotate the front wheel according to the rigidbody velocity magnitude
            wheel.transform.localEulerAngles = new Vector3((Input.GetAxis("Horizontal") * 15), wheel.transform.localEulerAngles.y, wheel.transform.localEulerAngles.z); // rotate the front wheel according to the player input
        }
        foreach (GameObject wheel in backWheels) // for each back wheel
        {
            wheel.transform.Rotate(new Vector3(0f, 0f, -rigidBody.velocity.magnitude)); // rotate the back wheel according to the rigidbody velocity magnitude
        }
    }

    public void SpeedBoost(float force, Color color, float engineFireDuration, float cameraZoomOutDuration, float cameraZoomIntDuration, float deltaFoV, bool playKartSound, bool playCharacterSound, ForceMode forceMode)
    {
        if (!engineFire.isPlaying) // check if the engine fire is not playing
        {
            engineFire.Play(); // play the engine fire
        }
        var fire = engineFire.main; // get the engine fire settings module
        fire.startColor = color; // set the engine fire color
        if (playKartSound) // check if we should play the kart sound
        {
            kartClipPlayer.PlayOneShot(7, 12, 1.0f, true); // play the kart sound
        }
        if (playCharacterSound) // check if we should play the character sound
        {
            characterClipPlayer.PlayOneShot(0, 2, 1.0f, true); // play the character sound
        }
        StartCoroutine(DisableParticleSystemAfterSeconds(engineFire, engineFireDuration)); // start the couroutine which disables the engine fire after engineFireDuration seconds
        cameraManager.DoSpeedBoostMovement(cameraZoomOutDuration, cameraZoomIntDuration, deltaFoV); // do the camera manager speed boost movement
        rigidBody.AddForce(transform.forward * force, forceMode); // add a forward force to the kart body
        Sequence sequence = DOTween.Sequence(); // create a new dotween sequence
        sequence.Append(DOTween.To(() => currentMaxForwardVelocity, (newValue) => currentMaxForwardVelocity = newValue, currentMaxForwardVelocity + force, 1.0f)); // add to the sequence a tween that changes the current max forward velocity upwards
        sequence.Append(DOTween.To(() => currentMaxForwardVelocity, (newValue) => currentMaxForwardVelocity = newValue, baseMaxforwardVelocity, 1.0f)); // add to the sequence a tween that changes the current max forward velocity back to normal
    }
    IEnumerator DisableParticleSystemAfterSeconds(ParticleSystem ps, float seconds)
    {
        yield return new WaitForSeconds(seconds); // wait seconds seconds
        ps.Stop(); // stop the particle system
    }

    private void FixedUpdate()
    {
        if (raceController.racePhase == RaceController.RacePhase.TimeTrialRace)
        {
            ghostRacerSaver.Add(transform.position,raceController.totalLapTimer);
        }

        rigidBody.AddForce(Vector3.down * gravity, ForceMode.Acceleration); // add a downwards gravity force to the kart
        if (!controllable) // check if we are not controllable
        {
            animator.SetFloat("Blend", 0.5f); // set the animator blend tree float value to idle neutral 0.5
            return; // return
        }
        RaycastHit hitNear; // create a raycasthit that will be used to calculate the kart normal
        Physics.Raycast(transform.position + (transform.up * .1f), Vector3.down, out hitNear, 2.0f); // send a raycast downwards to calculate the kart's normal
        kartNormal.up = Vector3.Lerp(kartNormal.up, hitNear.normal, Time.deltaTime * 8.0f); // set the kart normal
        kartNormal.Rotate(0, transform.eulerAngles.y, 0); // set the kart normal rotation
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y + currentRotate, 0), Time.deltaTime * 5f); // set the kart rotation
        if (drifting) // check if we are drifting
        {
            float control = (driftDirection == 1) ? Remap(Input.GetAxis("Horizontal"), -1, 1, .5f, 2) : Remap(Input.GetAxis("Horizontal"), -1, 1, 2, .5f); // control the force drift direction
            parent.localRotation = Quaternion.Euler(0, Mathf.LerpAngle(parent.localEulerAngles.y, (control * 15) * driftDirection, .2f), 0); // set the force drifting rotation
        }
        else // otherwise
        {
            parent.localRotation = Quaternion.identity; // go back to regular rotation
        }
        if (grounded) // check if we are grounded
        {
            rigidBody.AddForce(kartNormal.forward * currentThrustForce, ForceMode.Acceleration); // add force parallel to the normal
        }
        else // otherwise
        { 
            rigidBody.AddForce((transform.forward * currentThrustForce), ForceMode.Acceleration); // add force parallel to the forward
        }
        if (driftCooldownTimer > 0) // check if the drift cooldown timer is greater than 0
        {
            driftCooldownTimer -= Time.deltaTime; // subtract from the drift cooldown timer
            if (driftCooldownTimer < 0) // if it has gone below zero
            {
                driftCooldownTimer = 0; // set it back to zero
            }
        }


    }

    public void DoSteering(int direction, float amount)
    {
        deltaRotate = direction * amount; // calculate the deltarotate
    }

    public float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2; // remap a value linearly from [from1 to1] to [from2 to2]
    }

    public IEnumerator BlendEngineSoundsCoroutine()                                         // ENGINE SOOUND 
    {
        for (; ; )
        {
            if (!engineTransitionBlocked)
            {
                float currentVelocity = rigidBody.velocity.magnitude;
                currentEngineStage = 3;
                if (currentVelocity > 15 && currentVelocity < 75) currentEngineStage = 4;
                if (currentVelocity >= 75 && currentVelocity < 100) currentEngineStage = 8;
                if (currentVelocity >= 100 && currentVelocity < 120) currentEngineStage = 9;
                if (currentVelocity >= 120) currentEngineStage = 10;
            }
            if (previousEngineStage != currentEngineStage)
            {
                StartCoroutine(CrossFadeEngineSoundTransitionCoroutine(previousEngineStage, currentEngineStage));
            }
            kartClipPlayer.audioSources[currentEngineStage].pitch = Mathf.Lerp(kartClipPlayer.audioSources[4].pitch, Remap(Mathf.Abs(currentThrustForce), 0, Mathf.Abs(currentMaxThrustForce), 0.8f, 1.3f), 0.1f * Time.deltaTime);
            previousEngineStage = currentEngineStage;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator UnblockEngineSoundTransitionCoroutine()                            // RELEASES THE BLOCK ON ENGINE SOUNDS TRANSITIONS
    {
        yield return new WaitForSeconds(2f);
        engineTransitionBlocked = false;
    }

    public IEnumerator CrossFadeEngineSoundTransitionCoroutine(int previousStage, int currentStage)
    {
        if (previousStage < currentStage && !engineTransitionBlocked)             // check if upshift
        {
            engineTransitionBlocked = true;
            StartCoroutine("UnblockEngineSoundTransitionCoroutine");
            for (float i = 1; i > 0; i -= 0.1f)                                         // FADE OUT OTHER ENGINE SOUNDS
            {
                kartClipPlayer.audioSources[3].volume = Mathf.Min(kartClipPlayer.audioSources[3].volume, i);
                kartClipPlayer.audioSources[4].volume = Mathf.Min(kartClipPlayer.audioSources[4].volume, i);
                kartClipPlayer.audioSources[8].volume = Mathf.Min(kartClipPlayer.audioSources[8].volume, i);
                kartClipPlayer.audioSources[9].volume = Mathf.Min(kartClipPlayer.audioSources[9].volume, i);
                kartClipPlayer.audioSources[10].volume = Mathf.Min(kartClipPlayer.audioSources[10].volume, i);
                yield return new WaitForSeconds(0.001f);
            }
            kartClipPlayer.audioSources[3].volume = 0;
            kartClipPlayer.audioSources[4].volume = 0;
            kartClipPlayer.audioSources[8].volume = 0;
            kartClipPlayer.audioSources[9].volume = 0;
            kartClipPlayer.audioSources[10].volume = 0;
            kartClipPlayer.audioSources[11].volume = 1;
            if (previousStage == 3)                           // PLAY UP SHIFT TRANSITION
            {
                kartClipPlayer.PlayOneShot(11, 16, 1f, true);
            }
            else if (previousStage == 4)
            {
                kartClipPlayer.PlayOneShot(11, 17, 1f, true);
            }
            else if (previousStage == 8)
            {
                kartClipPlayer.PlayOneShot(11, 18, 1f, true);
            }
            else if (previousStage == 8)
            {
                kartClipPlayer.PlayOneShot(11, 19, 1f, true);
            }
            yield return new WaitForSeconds(0.5f);
            for (float i = 0; i <= 1; i += 0.1f)                                        // FADE IN NEW ENGINE SOUNDS
            {
                kartClipPlayer.audioSources[currentStage].volume = i;
                yield return new WaitForSeconds(0.001f);
            }
            kartClipPlayer.audioSources[currentStage].volume = 1f;
        }
        else if (previousStage > currentStage)                      // check if downshift
        {
            for (float i = 1; i > 0; i -= 0.1f)                                         // FADE OUT OTHER ENGINE SOUNDS
            {
                kartClipPlayer.audioSources[3].volume = Mathf.Min(kartClipPlayer.audioSources[3].volume, i);
                kartClipPlayer.audioSources[4].volume = Mathf.Min(kartClipPlayer.audioSources[4].volume, i);
                kartClipPlayer.audioSources[8].volume = Mathf.Min(kartClipPlayer.audioSources[8].volume, i);
                kartClipPlayer.audioSources[9].volume = Mathf.Min(kartClipPlayer.audioSources[9].volume, i);
                kartClipPlayer.audioSources[10].volume = Mathf.Min(kartClipPlayer.audioSources[10].volume, i);
                kartClipPlayer.audioSources[11].volume = Mathf.Min(kartClipPlayer.audioSources[10].volume, i); ;
                yield return new WaitForSeconds(0.001f);
            }
            kartClipPlayer.audioSources[3].volume = 0;
            kartClipPlayer.audioSources[4].volume = 0;
            kartClipPlayer.audioSources[8].volume = 0;
            kartClipPlayer.audioSources[9].volume = 0;
            kartClipPlayer.audioSources[10].volume = 0;
            kartClipPlayer.audioSources[11].volume = 0;
            for (float i = 0; i <= 1; i += 0.1f)                                        // FADE IN NEW ENGINE SOUNDS
            {
                kartClipPlayer.audioSources[currentStage].volume = i;
                yield return new WaitForSeconds(0.001f);
            }
            kartClipPlayer.audioSources[currentStage].volume = 1f;
        }
    }

    public void AttachToCharacterAndKart()
    {
        headLights[0] = GameObject.FindGameObjectWithTag("Head Lights"); // store local references and deactivate the appropiate game objects and components
        brakeLights[0] = GameObject.FindGameObjectWithTag("Back Lights"); 
        frontWheels[0] = GameObject.FindGameObjectWithTag("Wheel FL"); 
        frontWheels[1] = GameObject.FindGameObjectWithTag("Wheel FR"); 
        backWheels[0] = GameObject.FindGameObjectWithTag("Wheel BL"); 
        backWheels[1] = GameObject.FindGameObjectWithTag("Wheel BR");
        animator = GameObject.FindGameObjectsWithTag("Character")[0].GetComponentInChildren<Animator>(); 
        characterClipPlayer = GameObject.FindGameObjectWithTag("Character").GetComponent<AudioClipPlayer>(); 
        kartClipPlayer = GameObject.FindGameObjectWithTag("Kart Body").GetComponent<AudioClipPlayer>(); 
        driftParticles.Add(GameObject.FindGameObjectsWithTag("Drift Particles 0")[0].GetComponent<VisualEffect>()); 
        driftParticles.Add(GameObject.FindGameObjectsWithTag("Drift Particles 1")[0].GetComponent<VisualEffect>()); 
        driftParticles.Add(GameObject.FindGameObjectsWithTag("Drift Particles 2")[0].GetComponent<VisualEffect>()); 
        driftParticles.Add(GameObject.FindGameObjectsWithTag("Drift Particles 3")[0].GetComponent<VisualEffect>()); 
        driftParticles.Add(GameObject.FindGameObjectsWithTag("Drift Particles 4")[0].GetComponent<VisualEffect>());  
        driftParticles.Add(GameObject.FindGameObjectsWithTag("Drift Particles 5")[0].GetComponent<VisualEffect>()); 
        driftParticles.Add(GameObject.FindGameObjectsWithTag("Drift Particles 6")[0].GetComponent<VisualEffect>()); 
        driftParticles.Add(GameObject.FindGameObjectsWithTag("Drift Particles 7")[0].GetComponent<VisualEffect>()); 
        driftParticles.Add(GameObject.FindGameObjectsWithTag("Drift Particles 8")[0].GetComponent<VisualEffect>());
        driftParticles.Add(GameObject.FindGameObjectsWithTag("Drift Particles 9")[0].GetComponent<VisualEffect>()); 
        driftParticles.Add(GameObject.FindGameObjectsWithTag("Drift Particles 10")[0].GetComponent<VisualEffect>()); 
        driftParticles.Add(GameObject.FindGameObjectsWithTag("Drift Particles 11")[0].GetComponent<VisualEffect>());
        driftParticles[0].gameObject.SetActive(false); 
        driftParticles[1].gameObject.SetActive(false);  
        driftParticles[2].gameObject.SetActive(false); 
        driftParticles[3].gameObject.SetActive(false);
        driftParticles[4].gameObject.SetActive(false);
        driftParticles[5].gameObject.SetActive(false);
        driftParticles[6].gameObject.SetActive(false);
        driftParticles[7].gameObject.SetActive(false);
        driftParticles[8].gameObject.SetActive(false);
        driftParticles[9].gameObject.SetActive(false);
        driftParticles[10].gameObject.SetActive(false);
        driftParticles[11].gameObject.SetActive(false);
        skidMarks.Add(GameObject.FindGameObjectsWithTag("Skid Marks")[0].GetComponent<TrailRenderer>()); 
        skidMarks.Add(GameObject.FindGameObjectsWithTag("Skid Marks")[1].GetComponent<TrailRenderer>());
        skidMarks.Add(GameObject.FindGameObjectsWithTag("Skid Marks")[2].GetComponent<TrailRenderer>());
        skidMarks.Add(GameObject.FindGameObjectsWithTag("Skid Marks")[3].GetComponent<TrailRenderer>());
        skidMarks.Add(GameObject.FindGameObjectsWithTag("Skid Marks")[4].GetComponent<TrailRenderer>());
        skidMarks.Add(GameObject.FindGameObjectsWithTag("Skid Marks")[5].GetComponent<TrailRenderer>());
        skidMarks.Add(GameObject.FindGameObjectsWithTag("Skid Marks")[6].GetComponent<TrailRenderer>());
        skidMarks.Add(GameObject.FindGameObjectsWithTag("Skid Marks")[7].GetComponent<TrailRenderer>());
        skidMarks.Add(GameObject.FindGameObjectsWithTag("Skid Marks")[8].GetComponent<TrailRenderer>());
        skidMarks.Add(GameObject.FindGameObjectsWithTag("Skid Marks")[9].GetComponent<TrailRenderer>());
        skidMarks.Add(GameObject.FindGameObjectsWithTag("Skid Marks")[10].GetComponent<TrailRenderer>());
        skidMarks.Add(GameObject.FindGameObjectsWithTag("Skid Marks")[11].GetComponent<TrailRenderer>());
        engineFire = GameObject.FindGameObjectsWithTag("Boostburner")[0].GetComponent<ParticleSystem>();
        kartSmoke = GameObject.FindGameObjectsWithTag("Smoke")[0].GetComponent<ParticleSystem>();
        dust = GameObject.FindGameObjectsWithTag("Dust")[0].GetComponent<ParticleSystem>();
        landingDust = GameObject.FindGameObjectsWithTag("Landing Dust")[0].GetComponent<ParticleSystem>();
        cameraManager = GameObject.FindObjectOfType<CameraManager>();
    }

    public IEnumerator RespawnAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GameObject respawnPoint = outOfBounds.GetFurthrestRespawnPoint();
        GameObject collider = GameObject.Find("Collider");
        collider.transform.position = respawnPoint.transform.position;
        transform.eulerAngles = new Vector3(0, respawnPoint.transform.eulerAngles.y, 0); // set the kart rotation
        respawning = false;
        controllable = true;
    }
    public void Respawn()
    {
        if (!respawning)
        {
            StartCoroutine("RespawnAfterSeconds", 2.0f);
            cameraManager.FadeMainCamInOutBlack();
            respawning = true;
            controllable = false;
        }
        
    }
}
