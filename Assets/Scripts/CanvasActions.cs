using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasActions : MonoBehaviour
{
    public Canvas testCanvas;
    public Canvas subCanvasT;
    public Canvas subCanvasR;


    public Outline outline;

    void Start()
    {
        testCanvas.enabled = false;
        subCanvasT.enabled = false;
        subCanvasR.enabled = false;
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
