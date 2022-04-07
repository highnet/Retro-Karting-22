using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipPlayer : MonoBehaviour
{
    public enum AudioPlayerType{ Muted, AmbienceLoops, OneShotPlayer, PowerUp, Jukebox, Character, KartBody} // these are all the different types of audio clip player
    public AudioListener listenerToFollow; // the audio listener to follow
    public List<AudioSource> audioSources; // the list of audio sources associated with this audio clip player
    public AudioPlayerType audioPlayerType; // the type of audio clip player we are working with
    public List<AudioClip> audioClips; // the list of audio clips  that this audio clip player can play
    private int jukeboxIndex; // the index of the audio clip to be played by the jukebox
    void Start()
    {
        audioSources = new List<AudioSource>(GetComponents<AudioSource>()); // store a local reference to all audio sources associated with this audio clip player
        if (audioPlayerType == AudioPlayerType.AmbienceLoops) // ambience loop player
        {
            for(int i = 0; i < audioSources.Count; i++) // start playing the correct audio clips
            {
                audioSources[i].clip = audioClips[i];
                audioSources[i].Play();
            }
        }
        if (audioPlayerType == AudioPlayerType.PowerUp) // powerup player
        {

            audioSources[0].clip = audioClips[0]; // start playing the correct audio clips
            audioSources[0].Play();
        }
        if (audioPlayerType == AudioPlayerType.Jukebox) // jukebox player
        {
            audioSources[0].clip = audioClips[0]; // start playing the correct audio clips
            audioSources[0].Play();
        }
        if (audioPlayerType == AudioPlayerType.KartBody) // kartbody player
        {
            audioSources[3].clip = audioClips[7]; // start playing the correct audio clips
            audioSources[3].Play();
            audioSources[4].clip = audioClips[8];
            audioSources[4].Play();
            audioSources[8].clip = audioClips[13];
            audioSources[8].Play();
            audioSources[9].clip = audioClips[14];
            audioSources[9].Play();
            audioSources[10].clip = audioClips[15];
            audioSources[10].Play();
        }
    }

    private void PlayNextJukeboxSong()
    {

        jukeboxIndex++; // increase the jukebox index
        if (jukeboxIndex > audioClips.Count-1) // if we ran out of songs to play, cycle back to the first song
        {
            jukeboxIndex = 0;
        }
        audioSources[0].clip = audioClips[jukeboxIndex]; // set the clip
        audioSources[0].Play(); // play the clip
    }

    // Update is called once per frame
    void Update()
    {
        if (listenerToFollow != null) // if we have defined a listener to follow
        {
            this.transform.position = listenerToFollow.gameObject.transform.position; // teleport to the listener
        }
        if (audioPlayerType == AudioPlayerType.Jukebox && !audioSources[0].isPlaying) // if the jukebox clip has completed playing
        { 
            PlayNextJukeboxSong(); // play the next song
        }        
    }

    public void PlayOneShot(int audioSourceIndex, int audioClipsIndex, float volumeScale, bool playIfActive)
    {
        if (!playIfActive) // check wether the one shot should play if the audio source is already playing
        {
            if (this.audioPlayerType == AudioPlayerType.Muted || audioSources[audioSourceIndex].isPlaying) return; // check wether the one shot should play
        }
        else
        {
            if (this.audioPlayerType == AudioPlayerType.Muted) return; // check wether the one shot should play
        }
        audioSources[audioSourceIndex].PlayOneShot(audioClips[audioClipsIndex], volumeScale); // play the desired one shot
    }

    public void PlayRandomAudioClip(int audioSourceIndex,float volumeScale)
    {
        if (this.audioPlayerType == AudioPlayerType.Muted || audioSources[audioSourceIndex].isPlaying) return; // check wether the one shot should play
        audioSources[audioSourceIndex].PlayOneShot(audioClips[UnityEngine.Random.Range(0,audioClips.Count)], volumeScale); // play the desired one shot
    }

    public void PlayRandomAudioClipRange(int audioSourceIndex, int audioClipsIndexStart, int audioClipsIndexEnd, float volumeScale)
    {
        if (this.audioPlayerType == AudioPlayerType.Muted || audioSources[audioSourceIndex].isPlaying) return; // check wether the one shot should paly
        audioSources[audioSourceIndex].PlayOneShot(audioClips[UnityEngine.Random.Range(audioClipsIndexStart, audioClipsIndexEnd+1)], volumeScale); // play the desired one shot
    }
}
