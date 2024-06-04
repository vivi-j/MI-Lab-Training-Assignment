using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothTurn : MonoBehaviour
{
    public Transform player;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var joystickAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
        if (joystickAxis.x >= .8f)
            player.transform.RotateAround(player.position, player.up, speed * .1f);
        if (joystickAxis.x <= -.8f)
            player.transform.RotateAround(player.position, player.up, speed * -.1f);
    }
}
