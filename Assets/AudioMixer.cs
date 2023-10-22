using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMixer : MonoBehaviour
{
    public AudioSource audioSource1; // Reference to the first audio source
    public AudioSource audioSource2; // Reference to the second audio source
    public float crossfadeDuration = 1.0f; // Duration of the crossfade effect

    private bool isFirstAudioSourcePlaying = true;
    private AudioClip originalTrack;
    private AudioClip newTrack;       // Reference to the new audio clip
    private bool isOriginalTrackPlaying = true;

    private void Start()
    {
        originalTrack = audioSource1.clip;
        newTrack = audioSource2.clip;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Determine which track to crossfade to
            AudioClip nextTrack = isOriginalTrackPlaying ? newTrack : originalTrack;

            // Start the crossfade coroutine
            StartCoroutine(CrossfadeToTrack(nextTrack));
        }
    }

    private IEnumerator CrossfadeToTrack(AudioClip trackToPlay)
    {
        // Determine which audio source is playing and which is silent
        AudioSource activeAudioSource = isFirstAudioSourcePlaying ? audioSource1 : audioSource2;
        AudioSource silentAudioSource = isFirstAudioSourcePlaying ? audioSource2 : audioSource1;

        // Set the clip of the silent audio source to the next track
        silentAudioSource.clip = trackToPlay;
        silentAudioSource.Play();

        // Fade in the volume of the silent audio source
        float t = 0.0f;
        while (t < crossfadeDuration)
        {
            t += Time.deltaTime;
            silentAudioSource.volume = t / crossfadeDuration;
            activeAudioSource.volume = 1 - (t / crossfadeDuration);
            yield return null;
        }

        // Stop the active audio source and set the flag to indicate which audio source is playing
        activeAudioSource.Stop();
        isFirstAudioSourcePlaying = !isFirstAudioSourcePlaying;
        isOriginalTrackPlaying = !isOriginalTrackPlaying;
    }
}
