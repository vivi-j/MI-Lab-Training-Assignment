using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Movement : MonoBehaviour
{
    public float speed = 3.0f; // Movement speed
    public float rotationSpeed = 50.0f; // Rotation speed
    public Transform cameraTransform; // Character

    private Quaternion initialRotation; // Initial rotation of the camera

    void Start()
    {
        // Store the initial rotation of the camera
        initialRotation = cameraTransform.rotation;
    }

    void Update()
    {
        // Controller stuff
        Vector2 leftPrimaryAxis = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick, OVRInput.Controller.LTouch);
        Vector2 rightPrimaryAxis = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick, OVRInput.Controller.RTouch);

        // Calculate movement direction based on camera forward direction
        Vector3 forward = cameraTransform.forward;
        forward.y = 0; // Ignore vertical movement
        forward.Normalize();

        Vector3 right = cameraTransform.right;
        right.y = 0; // Ignore vertical movement
        right.Normalize();

        // Apply rotation
        cameraTransform.Rotate(0, rightPrimaryAxis.x * rotationSpeed * Time.deltaTime, 0);

        // Recalculate forward and right vectors based on the updated rotation
        forward = cameraTransform.forward;
        forward.y = 0; // Ignore vertical movement
        forward.Normalize();

        right = cameraTransform.right;
        right.y = 0; // Ignore vertical movement
        right.Normalize();

        // Apply movement
        Vector3 moveDirection = forward * leftPrimaryAxis.y + right * leftPrimaryAxis.x;
        moveDirection.y = 0; // Keep movement in the horizontal plane
        cameraTransform.position += moveDirection * speed * Time.deltaTime;
    }
}