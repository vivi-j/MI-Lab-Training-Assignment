using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasActions : MonoBehaviour
{
    public Canvas testCanvas;
    public Outline outline;

    void Start()
    {
        testCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.RTouch) && outline.enabled == true)
        {
            testCanvas.enabled = true;
        }
    }
}
