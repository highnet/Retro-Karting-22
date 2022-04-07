using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.VFX;

public class KartController : MonoBehaviour
{
    public bool controllable;
    public Rigidbody rigidBody;
    private float deltaThrustForce;
    public float currentThrustForce;
    private float deltaRotate;
    public float currentRotate;
    public float baseMaxThrustForce;
    public float thrustForceInterpolationTime;
    public float currentMaxThrustForce;
    public float baseSteeringStrength;
    public float gravity;
    public float minimumSteerAmount;
    public float maximumSteerAmount;
    public float speedThreshold0;
    public float speedThreshold1;
    public bool headLightsOn;
    public bool brakeLightsOn;
    public List<GameObject> headLights;
    public List<GameObject> brakeLights;
    public int usableSpeedups;
    public Material carTexture;
    public int coinCount;
    public Powerup powerup;
    public bool drifting;
    public float driftDirection;
    public float baseDriftingStrength;
    public bool grounded;
    public List<GameObject> frontWheels;
    public List<GameObject> backWheels;
    public Animator animator;
    public Transform kartNormal;
    public List<VisualEffect> driftParticles;
    public List<TrailRenderer> skidMarks;
    public ParticleSystem kartSmoke;
    public ParticleSystem dust;
    public ParticleSystem engineFire;
    public ParticleSystem landingDust;
    public float driftTimer;
    public float driftTimerThreshold0;
    public float driftTimerThreshold1;
    public float driftTimerThreshold2;
    public float driftTimerThreshold0SpeedBoostForce;
    public float driftTimerThreshold1SpeedBoostForce;
    public float driftTimerThreshold2SpeedBoostForce;
    public float driftCooldown;
    public float driftCooldownTimer;
    public CameraManager cameraManager;
    public float baseMaxforwardVelocity;
    public float currentMaxForwardVelocity;
    public float maxReverseVelocity;
    public Transform parent;
    public bool hit;
    public AudioClipPlayer characterClipPlayer;
    public AudioClipPlayer kartClipPlayer;
    public RacingUIController racingUIController;
    public bool burstDriftParticlesEnabled;
    public int currentEngineStage;
    public int previousEngineStage;
    public bool engineTransitionBlocked;
    public bool engineIsRunning;

    void Start()
    {
        currentMaxForwardVelocity = baseMaxforwardVelocity;
        currentMaxThrustForce = baseMaxThrustForce;
        racingUIController = FindObjectOfType<RacingUIController>();
    }

    void Update()
    {
        transform.position = rigidBody.transform.position - new Vector3(0, 2f, 0); // teleport to our collider & rigidbody
        RaycastHit hitDown;
        hit = Physics.Raycast(transform.position + transform.up, Vector3.down, out hitDown, 1.5f);


        if (hit && !grounded)
        {
            landingDust.Play();
            kartClipPlayer.PlayOneShot(0, 9, 1.0f, true);
        }

        if (hit)
        {
            grounded = true;

        }
        if (!hit)
        {
            grounded = false;
        }

        if (engineIsRunning) { 
        StartCoroutine("BlendEngineSoundsCoroutine");
        }

        if (!controllable)
        {
            return;
        }

        bool brake = Input.GetKey(KeyCode.Space);
        float steerInput = Input.GetAxis("Horizontal");
        bool throttle = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
        bool reverse = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
        

        currentMaxThrustForce = baseMaxThrustForce + (coinCount * powerup.coinSpeedStrength); // set max speed

        int steerDirection = steerInput > 0 ? 1 : -1;
        float absoluteSteeringInput = Mathf.Abs(steerInput);

        animator.SetFloat("Blend", Remap(steerInput, -1, 1, 0, 1));


        if (rigidBody.velocity.magnitude < speedThreshold0)
        {
            absoluteSteeringInput *= 0;
        }
        else if (rigidBody.velocity.magnitude < speedThreshold1)
        {
            absoluteSteeringInput *= 1;
        }
        else
        {
            absoluteSteeringInput = Mathf.Clamp(Mathf.Abs(steerInput) * (1 / Mathf.Log10(rigidBody.velocity.magnitude)), minimumSteerAmount, maximumSteerAmount);
        }


        if (steerInput  != 0)
        {
            if (!reverse)
            {
                deltaRotate = steerDirection * baseSteeringStrength * absoluteSteeringInput;
            } else 
            {
                deltaRotate = -steerDirection * baseSteeringStrength * absoluteSteeringInput;
            }
        }


        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
        {
            drifting = false;

            driftParticles[0].SetFloat("Spawn Rate", 1f);
            driftParticles[1].SetFloat("Spawn Rate", 1f);
            driftParticles[2].SetFloat("Spawn Rate", 1f);
            driftParticles[3].SetFloat("Spawn Rate", 1f);
            driftParticles[4].SetFloat("Spawn Rate", 1f);
            driftParticles[5].SetFloat("Spawn Rate", 1f);
            
            foreach (VisualEffect vfx in driftParticles)
            {
                vfx.gameObject.SetActive(false);
            }

            burstDriftParticlesEnabled = false;

            if (driftTimer > driftTimerThreshold2)
            {
                SpeedBoost(driftTimerThreshold2SpeedBoostForce, Color.red,0.8f,0.8f,1.2f,25,false,false);
            }
            else if (driftTimer > driftTimerThreshold1)
            {
                SpeedBoost(driftTimerThreshold1SpeedBoostForce, Color.yellow,1.0f,1.0f,1.0f,15,false,false);
            }
            else if (driftTimer > driftTimerThreshold0)
            {
                SpeedBoost(driftTimerThreshold0SpeedBoostForce, Color.white,1.2f,1.2f,1.8f,10,false,false);

            }

            driftTimer = 0;
        }

        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))&& !drifting && driftCooldownTimer == 0 && Input.GetAxis("Horizontal") != 0 && !reverse && throttle &&!brake)
        {
            drifting = true;
            driftDirection = Input.GetAxis("Horizontal") > 0 ? 1 : -1;
            kartClipPlayer.PlayOneShot(1, 11, 1.0f, true);

            if (grounded)
            {
                rigidBody.AddForce(Vector3.up * 500f, ForceMode.Acceleration);
            }

            driftCooldownTimer = driftCooldown;

        }

        if (drifting)
        {
            if (driftDirection == steerDirection)
            {
                deltaRotate += driftDirection * baseDriftingStrength * absoluteSteeringInput;
            } else
            {
                deltaRotate /= 2.5f;
            }

            driftTimer += Time.deltaTime;

            if (driftTimer > driftTimerThreshold0)
            {

                if (!burstDriftParticlesEnabled)
                {
                driftParticles[0].gameObject.SetActive(true);
                driftParticles[1].gameObject.SetActive(true);
                driftParticles[0].SetFloat("Spawn Rate", 5f);
                driftParticles[1].SetFloat("Spawn Rate", 5f);

                int random1 = (int) UnityEngine.Random.Range(0f, 3f);
                int random2 = (int)UnityEngine.Random.Range(0f, 3f);

                switch (random1)
                {
                    case 0:
                        driftParticles[6].gameObject.SetActive(true);
                        break;
                    case 1:
                        driftParticles[8].gameObject.SetActive(true);
                        break;
                    case 2:
                        driftParticles[10].gameObject.SetActive(true);
                        break;
                }
                switch (random2)
                {
                    case 0:
                        driftParticles[7].gameObject.SetActive(true);
                        break;
                    case 1:
                        driftParticles[9].gameObject.SetActive(true);
                        break;
                    case 2:
                        driftParticles[11].gameObject.SetActive(true);
                        break;
                }
                }
                burstDriftParticlesEnabled = true;
            }
            if (driftTimer > driftTimerThreshold1)
            {
                driftParticles[2].gameObject.SetActive(true);
                driftParticles[3].gameObject.SetActive(true);
                driftParticles[0].SetFloat("Spawn Rate", 10f);
                driftParticles[1].SetFloat("Spawn Rate", 10f);
                driftParticles[2].SetFloat("Spawn Rate", 5f);
                driftParticles[3].SetFloat("Spawn Rate", 5f);
            }
            if (driftTimer > driftTimerThreshold2)
            {
                driftParticles[4].gameObject.SetActive(true);
                driftParticles[5].gameObject.SetActive(true);
                driftParticles[0].SetFloat("Spawn Rate", 15f);
                driftParticles[1].SetFloat("Spawn Rate", 15f);
                driftParticles[2].SetFloat("Spawn Rate", 20f);
                driftParticles[3].SetFloat("Spawn Rate", 20f);
                driftParticles[4].SetFloat("Spawn Rate", 25f);
                driftParticles[5].SetFloat("Spawn Rate", 25f);
            }

        }


        if (grounded && (brake || drifting || absoluteSteeringInput > 0.6f || rigidBody.velocity.magnitude > currentMaxThrustForce - 30 && absoluteSteeringInput > 0.5f))
        {
            for (int i = 0; i < skidMarks.Count; i++)
            {
                skidMarks[i].emitting = true;
            }
        } else
        {
            for (int i = 0; i < skidMarks.Count; i++)
            {
                skidMarks[i].emitting = false;
            }
        }

        currentRotate = Mathf.Lerp(currentRotate, deltaRotate, Time.deltaTime * 4f);
        deltaRotate = 0f;

        
        if (throttle && !brake && rigidBody.velocity.magnitude < currentMaxForwardVelocity)
        {
            deltaThrustForce = currentMaxThrustForce;
        } else if (reverse && !brake && rigidBody.velocity.magnitude < maxReverseVelocity)
        {
            deltaThrustForce = -currentMaxThrustForce;
        }
        

        if (grounded && absoluteSteeringInput > 0)
        {
            if (!drifting)
            {
                currentThrustForce -= absoluteSteeringInput * .1f; // slow down the car when turning
                deltaThrustForce -= absoluteSteeringInput * .1f; // slow down the car when turning
            }else
            {
                currentThrustForce -= absoluteSteeringInput * .1f; // slow down the car when drifting
                deltaThrustForce -= absoluteSteeringInput * .1f; // slow down the car when drifting
            }
        }
       


        if (throttle || reverse)
        {
            if (grounded)
            {
                if (!dust.isPlaying)
                {
                    dust.Play();
                }
            }
        }
        else
        {
            dust.Stop();
        }

        var smoke = kartSmoke.emission;

        if (!drifting)
        {
            smoke.rateOverTime = Remap(Mathf.Abs(currentThrustForce), 0.0f, currentMaxThrustForce, 10.0f, 300.0f);
        } else
        {
            smoke.rateOverTime = Remap(Mathf.Abs(currentThrustForce), 0.0f, currentMaxThrustForce, 100.0f, 600.0f);
        }


        if (drifting && driftTimer > driftTimerThreshold1)
        {
            deltaThrustForce /= (driftTimer * 0.5f);
        }

        if (!grounded)
        {
            deltaThrustForce = 0;
        }

        currentThrustForce = Mathf.SmoothStep(currentThrustForce, deltaThrustForce, Time.deltaTime * thrustForceInterpolationTime); // idle acceleration


        deltaThrustForce = 0f;

        if (Input.GetKeyUp(KeyCode.L))
        {

            if (headLightsOn)
            {
                kartClipPlayer.PlayOneShot(5, 0, 1.0f, true);
            } else
            {
                kartClipPlayer.PlayOneShot(5, 1, 1.0f, true);
            }

            headLightsOn = !headLightsOn;
            foreach (GameObject light in headLights)
            {
                light.gameObject.SetActive(headLightsOn);
            }
        }

        if (brake)
        {
            brakeLightsOn = true;
            kartClipPlayer.PlayOneShot(2, 10, 1.0f, false);
        }else
        {
            brakeLightsOn = false;
        }

        foreach (GameObject light in brakeLights)
        {
            light.gameObject.SetActive(brakeLightsOn);
        }

        if (Input.GetKeyDown(KeyCode.Return) && usableSpeedups > 0)
        {
            this.GetComponent<Powerup>().DoSpeedUp();
            usableSpeedups--;
            racingUIController.usableSpeedsupText.text = usableSpeedups.ToString();
        }

        foreach (GameObject wheel in frontWheels)
        {
            wheel.transform.Rotate(new Vector3(0f, 0f, -rigidBody.velocity.magnitude));
            wheel.transform.localEulerAngles = new Vector3((Input.GetAxis("Horizontal") * 15), wheel.transform.localEulerAngles.y, wheel.transform.localEulerAngles.z);
        }
        foreach (GameObject wheel in backWheels)
        {
            wheel.transform.Rotate(new Vector3(0f, 0f, -rigidBody.velocity.magnitude));
        }

    }

    public void SpeedBoost(float force,Color color,float engineFireDuration,float cameraZoomOutDuration, float cameraZoomIntDuration, float deltaFoV,bool playKartSound, bool playCharacterSound)
    {
        if (!engineFire.isPlaying)
        {
            engineFire.Play();

        }
        var fire = engineFire.main;
        fire.startColor = color;

        if (playKartSound)
        {
            kartClipPlayer.PlayOneShot(7, 12, 1.0f, true);
        }
        if (playCharacterSound)
        {
            characterClipPlayer.PlayOneShot(0, 2, 1.0f, true);
        }

        StartCoroutine(DisableParticleSystemAfterSeconds(engineFire, engineFireDuration));
        cameraManager.DoSpeedBoostMovement(cameraZoomOutDuration, cameraZoomIntDuration, deltaFoV);
        rigidBody.AddForce(transform.forward * force, ForceMode.Acceleration);


        Sequence sequence = DOTween.Sequence();
        sequence.Append(DOTween.To(() => currentMaxForwardVelocity, (newValue) => currentMaxForwardVelocity = newValue, currentMaxForwardVelocity + force, 1.0f));
        sequence.Append(DOTween.To(() => currentMaxForwardVelocity, (newValue) => currentMaxForwardVelocity = newValue, baseMaxforwardVelocity, 1.0f));

    }
    IEnumerator DisableParticleSystemAfterSeconds(ParticleSystem ps, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ps.Stop();
    }

    private void FixedUpdate()
    {
        rigidBody.AddForce(Vector3.down * gravity, ForceMode.Acceleration);


        if (!controllable)
        {
            animator.SetFloat("Blend", 0.5f);
            return;
        }

        bool brake = Input.GetKey(KeyCode.Space);
        float steerInput = Input.GetAxis("Horizontal");
        bool throttle = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);


        RaycastHit hitNear;
        Physics.Raycast(transform.position + (transform.up * .1f), Vector3.down, out hitNear, 2.0f);
        kartNormal.up = Vector3.Lerp(kartNormal.up, hitNear.normal, Time.deltaTime * 8.0f);
        kartNormal.Rotate(0, transform.eulerAngles.y, 0);

        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y + currentRotate, 0), Time.deltaTime * 5f);

        if (drifting)
        {
            float control = (driftDirection == 1) ? Remap(Input.GetAxis("Horizontal"), -1, 1, .5f, 2) : Remap(Input.GetAxis("Horizontal"), -1, 1, 2, .5f);
            parent.localRotation = Quaternion.Euler(0, Mathf.LerpAngle(parent.localEulerAngles.y, (control * 15) * driftDirection, .2f), 0);
        } else
        {
            parent.localRotation = Quaternion.identity;

        }

        if (grounded)
        {

            rigidBody.AddForce(kartNormal.forward * currentThrustForce, ForceMode.Acceleration);
        }
        else
        {
            rigidBody.AddForce((transform.forward * currentThrustForce), ForceMode.Acceleration);
        }


        if (driftCooldownTimer > 0)
        {
            driftCooldownTimer -= Time.deltaTime;
            if (driftCooldownTimer < 0)
            {
                driftCooldownTimer = 0;
            }
        }
    }


    public void DoSteering(int direction, float amount)
    {
        deltaRotate = direction * amount;
    }


    public float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public IEnumerator BlendEngineSoundsCoroutine()                                         // ENGINE SOOUND 
    {
        for (;;)
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


        if (previousStage < currentStage && !engineTransitionBlocked) {             // check if upshift

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
                //   kartClipPlayer.PlayOneShot(12, 20, 1f, true);
            }
            else if (previousStage == 4)
            {
                kartClipPlayer.PlayOneShot(11, 17, 1f, true);
                //   kartClipPlayer.PlayOneShot(12, 21, 1f, true);
            }
            else if (previousStage == 8)
            {
                kartClipPlayer.PlayOneShot(11, 18, 1f, true);
                //   kartClipPlayer.PlayOneShot(12, 22, 1f, true);
            }

            else if (previousStage == 8)
            {
                kartClipPlayer.PlayOneShot(11, 19, 1f, true);
                // kartClipPlayer.PlayOneShot(12, 20, 1f, true);
            }

            yield return new WaitForSeconds(0.5f);




        for (float i = 0; i <= 1; i += 0.1f)                                        // FADE IN NEW ENGINE SOUNDS
        {

            kartClipPlayer.audioSources[currentStage].volume = i;


            yield return new WaitForSeconds(0.001f);
        }

        kartClipPlayer.audioSources[currentStage].volume = 1f;

    } else if ( previousStage > currentStage)                      // check if downshift
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
        headLights[0] = GameObject.FindGameObjectWithTag("Head Lights");
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
}
