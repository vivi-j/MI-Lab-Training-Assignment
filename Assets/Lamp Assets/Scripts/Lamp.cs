using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour {

    public GameObject LampLight;

    [HideInInspector]
    public GameObject DomeOff;

    [HideInInspector]
    public GameObject DomeOn;

    public bool TurnOn;
    public int intensityVal = 1;
    private Light lampLightComponent;


    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        


        if (TurnOn== true)
        {
            LampLight.SetActive(true);
            lampLightComponent = LampLight.GetComponent<Light>();
            if (lampLightComponent != null)
            {
                lampLightComponent.intensity = intensityVal;
            }
            DomeOff.SetActive(false);
            DomeOn.SetActive(true);

        }
        if (TurnOn == false)
        {
            LampLight.SetActive(false);
            DomeOff.SetActive(true);
            DomeOn.SetActive(false);

        }
    }
}
