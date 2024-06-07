using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public AudioClip[] audioClips;  // Array to hold audio files
    public AudioSource audioSource;
    private bool isPlaying = false; // Track if audio is currently playing
    private GameObject fillObject;
    private Image fillImage;
    Color originalColor = new Color(164f / 255f, 177f / 255f, 255f / 255f); // A4B1FF in RGB values

    // Start is called before the first frame update
    void Start()
    {
        fillObject = GameObject.FindWithTag("FillRadio");
        audioSource = gameObject.GetComponent<AudioSource>();
        fillImage = fillObject.GetComponent<Image>();
        fillImage.color = originalColor;
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
