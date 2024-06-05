using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothTurn : MonoBehaviour
{
    public Transform player;
    public Transform target; 
    public float speed;
    //public Vector3 initialForwardDirection = Vector3.forward;

    void Start()
    {
        //player.LookAt(new Vector3(target.position.x, player.position.y, target.position.z));
        Vector3 direction = (target.position - player.position).normalized;
        direction.y = 0; // Keep only the horizontal direction
        player.rotation = Quaternion.LookRotation(direction);
    }



    void Update()
    {
        var joystickAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
        if (joystickAxis.x >= .8f)
            player.transform.RotateAround(player.position, player.up, speed * .1f);
        if (joystickAxis.x <= -.8f)
            player.transform.RotateAround(player.position, player.up, speed * -.1f);
    }
}
