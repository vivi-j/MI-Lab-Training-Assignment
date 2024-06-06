using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    public AudioClip[] audioClips;  // Array to hold audio files
    private AudioSource audioSource;
    private bool isPlaying = false; // Track if audio is currently playing

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayAudioClip(int curInd)
    {
        if (audioClips.Length == 0)
        {
            Debug.LogWarning("No audio clips assigned.");
            return;
        }

        audioSource.clip = audioClips[curInd]; 
        audioSource.Play();
        isPlaying = true;
        Debug.Log("Playing song.");
    }

    public void StopAudio()
    {
        audioSource.Stop();
        isPlaying = false;
        Debug.Log("Stopping song.");
    }

    public bool IsAudioPlaying()
    {
        return isPlaying;
    }
}
