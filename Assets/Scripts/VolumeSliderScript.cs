using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderScript : MonoBehaviour
{
    private Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        // Find the slider component attached to the GameObject this script is attached to
        volumeSlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        // Ensure that the slider component was found
        if (volumeSlider != null)
        {
            // Find the GameObject named "RadioCanvas"
            GameObject radioObject = GameObject.Find("RadioCanvas");

            if (radioObject != null)
            {
                // Define the minimum and maximum volume values
                float minVolume = 0f;
                float maxVolume = 1f;

                // Calculate the volume value based on the slider's value
                float volumeValue = minVolume + (maxVolume - minVolume) * volumeSlider.value;

                // Update the radio's volume value
                AudioSource radioAudio = radioObject.GetComponent<AudioSource>();
                if (radioAudio != null)
                {
                    radioAudio.volume = volumeValue;
                }
                else
                {
                    Debug.LogWarning("No AudioSource component found on RadioCanvas.");
                }
            }
            else
            {
                Debug.LogWarning("No GameObject named 'RadioCanvas' found.");
            }
        }
        else
        {
            Debug.LogError("Slider component not found.");
        }
    }
}
