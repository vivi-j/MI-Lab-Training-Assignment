using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsPointer : MonoBehaviour
{
    public float defaultLength = 3.0f;
    public Transform secondaryPointer; // Assign this in the inspector to another transform for the second ray
    private LineRenderer lineRenderer = null;
    private GameObject currentObject;
    private Outline currentOutline;
    // private bool outlineEnabledOnce = false;


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateLength();
        HandleRaycasts();
        //HandleMenuActivation();
    }

    private void UpdateLength()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, CalculateEnd(transform));
    }

    private Vector3 CalculateEnd(Transform pointer)
    {
        RaycastHit hit = CreateForwardRaycast(pointer);
        Vector3 endPosition = DefaultEnd(pointer, defaultLength);

        if (hit.collider)
            endPosition = hit.point;

        return endPosition;
    }

    private RaycastHit CreateForwardRaycast(Transform pointer)
    {
        RaycastHit hit;
        Ray ray = new Ray(pointer.position, pointer.forward);

        Physics.Raycast(ray, out hit, defaultLength);
        return hit;
    }

    private Vector3 DefaultEnd(Transform pointer, float length)
    {
        return pointer.position + (pointer.forward * length);
    }

    private void HandleRaycasts()
    {
        RaycastHit hitPrimary = CreateForwardRaycast(transform);
        RaycastHit hitSecondary = CreateForwardRaycast(secondaryPointer);
        GameObject objects = null;

        /*if (hitPrimary.collider.tag == "Object" || hitPrimary.collider.tag == "Cube" || hitPrimary.collider.tag == "Sphere")
        {
            objects = hitPrimary.collider.gameObject;
            //outline = objects.GetComponent<Outline>();
            //outline.enabled = true;
            Canvas objectCanvas = currentObject.GetComponentInChildren<Canvas>();
            if (objectCanvas != null && OVRInput.GetDown(OVRInput.Button.Two)) // B
            {
                objectCanvas.enabled = true;
            }
        }
        else if (hitSecondary.collider.tag == "Object" || hitSecondary.collider.tag == "Cube" || hitSecondary.collider.tag == "Sphere")
        {
            objects = hitSecondary.collider.gameObject;
            //outline = objects.GetComponent<Outline>();
            //outline.enabled = true;
            Canvas objectCanvas = currentObject.GetComponentInChildren<Canvas>();
            if (objectCanvas != null && OVRInput.GetDown(OVRInput.Button.Two)) // B
            {
                objectCanvas.enabled = true;
            }
        }*/

        GameObject hitObject = null;

        if (hitPrimary.collider && (hitPrimary.collider.tag == "Object" || hitPrimary.collider.tag == "Cube" || hitPrimary.collider.tag == "Sphere"))
        {
            hitObject = hitPrimary.collider.gameObject;
        }
        else if (hitSecondary.collider && (hitSecondary.collider.tag == "Object" || hitSecondary.collider.tag == "Cube" || hitSecondary.collider.tag == "Sphere"))
        {
            hitObject = hitSecondary.collider.gameObject;
        }

        if (hitObject != null)
        {
            if (currentObject != hitObject)
            {
                if (currentOutline != null)
                {
                    currentOutline.enabled = false;
                }

                currentObject = hitObject;
                currentOutline = currentObject.GetComponent<Outline>();

                if (currentOutline != null)
                {
                    currentOutline.enabled = true;
                }
            }
        }
        else
        {
            if (currentOutline != null)
            {
                currentOutline.enabled = false;
                currentOutline = null;
                currentObject = null;
            }
        }
    }

 /*   private void HandleMenuActivation()
    {
        RaycastHit hit = CreateForwardRaycast();
        var Gameobject objects = null;
        if (hit.collider.tag == "Object" || hit.collider.tag == "Cube" || hit.collider.tag == "Sphere")
        {
            objects = hit.collider.gameObject;
            //outline = objects.GetComponent<Outline>();
            //outline.enabled = true;
            Canvas objectCanvas = currentObject.GetComponentInChildren<Canvas>();
            if (objectCanvas != null)
            {
                objectCanvas.enabled = true;
            }

        }

    }*/

}
