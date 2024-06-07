using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class IntensitySliderScript : MonoBehaviour
{
    private Slider intensitySlider;

    // Start is called before the first frame update
    void Start()
    {
        // find the slider compojent attached to the gameobject this script is attached to
        intensitySlider = GetComponent<Slider>();

    }

    // Update is called once per frame
    void Update()
    {
        // value on slider = intensity val
        GameObject lampObject = GameObject.FindWithTag("Lamp");
        float minIntensity = 1f;
        float maxIntensity = 5f;
        float intensityValue = minIntensity + (maxIntensity - minIntensity) * intensitySlider.value;
        lampObject.GetComponent<Lamp>().intensityVal = (int)intensityValue;
    }
}
