using UnityEngine;
using UnityEngine.UI;

public class InteractableSlider : MonoBehaviour
{
    public Slider slider;
    public float increaseSpeed = 0.5f; // speed of incraese

    private float currentValue = 0f;
    private bool isIncreasing = false;

    // Update is called once per frame
    void Update()
    {
        /*
        if (isIncreasing)
        {
            currentValue += increaseSpeed * Time.deltaTime;
            slider.value = Mathf.Clamp01(currentValue); // force val between 0 and 1 
        }*/
    }
}
