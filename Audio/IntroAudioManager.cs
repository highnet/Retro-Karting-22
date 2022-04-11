using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class IntroAudioManager : MonoBehaviour
{
   
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    public AudioSource introAudioSource;
    public AudioSource arcadeAudioSource;
    public AudioLowPassFilter introLPFilter;
    public AudioHighPassFilter introHPFilter;
    public AudioReverbFilter introReverb;
    public GameStateManager gameStateManager;

    private CinemachineTrackedDolly dolly;

    bool introSkipTransitionOver;
    bool introMusicFadedOut;


    private void Awake()
    {
        Object.DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {


        dolly = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();

        introHPFilter.cutoffFrequency = 1000;
        introLPFilter.cutoffFrequency = 2000;
        introAudioSource.volume = 0.1f;
        arcadeAudioSource.volume = 0.2f;


    }

    // Update is called once per frame
    void Update()
    {
        if (gameStateManager.currentGameState == GameStateManager.GameState.Intro)
        {

            introHPFilter.cutoffFrequency = Remap(dolly.m_PathPosition, 0, 2, 6000, 0);
            introLPFilter.cutoffFrequency = Remap(dolly.m_PathPosition, 0, 2, 8000, 22000);
            introAudioSource.volume = Remap(dolly.m_PathPosition, 0, 2, 0.1f, 1f);

            if (arcadeAudioSource != null)
            {
                arcadeAudioSource.volume = Remap(dolly.m_PathPosition, 0, 2, 0.2f, 0f);
            }



            introReverb.dryLevel = Remap(dolly.m_PathPosition, 0, 2, -1000f, 0);
            introReverb.reverbLevel = Remap(dolly.m_PathPosition, 0, 2, 2000f, -3700f);
            introReverb.reverbDelay = Remap(dolly.m_PathPosition, 0, 2, 0.2f, 0.03f);
        }


        if (gameStateManager.currentGameState == GameStateManager.GameState.MainMenu && !introSkipTransitionOver) {


            StartCoroutine(IntroSkipTransitionMusicCoroutine()) ;

            introSkipTransitionOver = true;

        }



        if (gameStateManager.currentGameState == GameStateManager.GameState.MainMenu && introSkipTransitionOver)
        {


            if (introMusicFadedOut) {
                StartCoroutine(FadeInIntroMusicCoroutine());
            }



        }


        if (gameStateManager.currentGameState == GameStateManager.GameState.Racing)
        {
            introSkipTransitionOver = true;
            if (!introMusicFadedOut)
            {
                StartCoroutine(FadeOutIntroMusicCoroutine());
            }

        }

    }

    public IEnumerator FadeInIntroMusicCoroutine()
    {

        for (float i = 0f; i < 1f; i += 0.1f)
        {

            introAudioSource.volume = Mathf.Max(introAudioSource.volume, i);
            yield return new WaitForSeconds(0.001f);
        }

        introAudioSource.volume = 1f;
        introMusicFadedOut = false;

    }

    public IEnumerator FadeOutIntroMusicCoroutine()
    {

        for (float i = 1f; i > 0; i -= 0.1f)
        {

            introAudioSource.volume = Mathf.Min(introAudioSource.volume, i);
            yield return new WaitForSeconds(0.001f);
        }

        introAudioSource.volume = 0f;
        introMusicFadedOut = true;
     
    }


        public IEnumerator IntroSkipTransitionMusicCoroutine()
    {

            for (float i = 0; i < 1; i += 0.05f)
            {

              //   Mathf.Min(kartClipPlayer.audioSources[3].volume, i)
                introHPFilter.cutoffFrequency = Mathf.Min(introHPFilter.cutoffFrequency, 1 - i);
                introLPFilter.cutoffFrequency = Mathf.Max(introLPFilter.cutoffFrequency, 22000f * i);
                introAudioSource.volume = Mathf.Max(introAudioSource.volume, i);

                introReverb.dryLevel = Mathf.Min(introReverb.dryLevel, Remap(i, 0f, 1f, -1000f , 0f ));
                introReverb.reverbLevel = Mathf.Min( introReverb.reverbLevel,  Remap(i,0f,1f,2000f, -3700f));
                introReverb.reverbDelay = Mathf.Max(introReverb.reverbDelay, Remap(i, 0f, 1f, 0.2f, 0.03f ));


                yield return new WaitForSeconds(0.001f);
            }

            introHPFilter.cutoffFrequency = 0;
            introLPFilter.cutoffFrequency = 22000;
            introAudioSource.volume = 1f;

            introReverb.dryLevel = 0;
            introReverb.reverbLevel = -3700;
            introReverb.reverbDelay = 0.03f;
       

    }


    public float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2; // remap a value linearly from [from1 to1] to [from2 to2]
    }
}
