using UnityEngine;
using UnityEngine.UI;

public class PhysicsPointer : MonoBehaviour
{
    public float defaultLength;
    public Transform secondaryPointer; // Assign this in the inspector to another transform for the second ray
    public Color translationButtonColor; // Public variable for the button color change

    private LineRenderer lineRenderer = null;
    private GameObject currentObject;
    private Outline currentOutline;
    private Image currentButtonImage;
    public Color originalButtonColor; // Default color
    private TranslationScript translationScript;
    private int mode = 0;
    private Canvas subCanvasCubeT = null;
    private Canvas subCanvasCubeR = null;
    Color originalColor = new Color(164f / 255f, 177f / 255f, 255f / 255f); // A4B1FF in RGB values
    bool isOriginalColor = true; // to toggle between original and random color
    private int lastActionMode = 0; 
    private bool isFirstPress = true;
    private int currentIndex = 0;  // Index of the current song
    int pressCount = 0; // Counter to keep track of A button presses
    int ApressCount = 0; // Counter to keep track of A button presses
    int numSongCount = 1;

    public GameObject collisionSpritePrefab; // Assign this in the inspector to the sprite GameObject
    private GameObject collisionSpriteInstance;
    private Vector3 collisionPoint;

    private bool spriteActive = false;



    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        UpdateLength();
        HandleRaycasts();
    }

    public void UpdateLength()
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

        GameObject hitObject = null;

        if (hitPrimary.collider)
        {
            hitObject = hitPrimary.collider.gameObject;
            collisionPoint = hitPrimary.point;
        }
        else if (hitSecondary.collider)
        {
            hitObject = hitSecondary.collider.gameObject;
            collisionPoint = hitSecondary.point;
        }


        ///////////////////////////////////////////////////////////////////////

        // Check if either pointer collides with something tagged "Translation"
        if (((hitPrimary.collider && hitPrimary.collider.tag == "Translation") || (hitSecondary.collider && hitSecondary.collider.tag == "Translation")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            Debug.Log("TRANSLATION SELECTED");
            Canvas parentCanvas = hitPrimary.collider.GetComponentInParent<Canvas>();
            if (parentCanvas != null)
            {
                Canvas[] subCanvases = parentCanvas.GetComponentsInChildren<Canvas>();
                foreach (Canvas subCanvas in subCanvases)
                {
                    if (subCanvas.CompareTag("SubCanvas"))
                    {
                        subCanvasCubeT = subCanvas;
                        subCanvasCubeT.enabled = !subCanvasCubeT.enabled; // Toggle the enabled state
                    }
                }

            }
        }


        else if (((hitPrimary.collider && hitPrimary.collider.tag == "XAxisTranslate") || (hitSecondary.collider && hitSecondary.collider.tag == "XAxisTranslate")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            mode = 1; // Set mode to 1
            Debug.Log("Mode: " + mode); // Print mode on debug log
        }
        else if (((hitPrimary.collider && hitPrimary.collider.tag == "YAxisTranslate") || (hitSecondary.collider && hitSecondary.collider.tag == "YAxisTranslate")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            mode = 2; // Set mode to 1
            Debug.Log("Mode: " + mode); // Print mode on debug log
        }
        else if (((hitPrimary.collider && hitPrimary.collider.tag == "ZAxisTranslate") || (hitSecondary.collider && hitSecondary.collider.tag == "ZAxisTranslate")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            mode = 3; // Set mode to 1
            Debug.Log("Mode: " + mode); // Print mode on debug log
        }
        else if (((hitPrimary.collider && hitPrimary.collider.tag == "NXAxisTranslate") || (hitSecondary.collider && hitSecondary.collider.tag == "NXAxisTranslate")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            mode = 4; // Set mode to 1
            Debug.Log("Mode: " + mode); // Print mode on debug log
        }
        else if (((hitPrimary.collider && hitPrimary.collider.tag == "NYAxisTranslate") || (hitSecondary.collider && hitSecondary.collider.tag == "NYAxisTranslate")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            mode = 5; // Set mode to 1
            Debug.Log("Mode: " + mode); // Print mode on debug log
        }
        else if (((hitPrimary.collider && hitPrimary.collider.tag == "NZAxisTranslate") || (hitSecondary.collider && hitSecondary.collider.tag == "NZAxisTranslate")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            mode = 6; // Set mode to 1
            lastActionMode = mode;
            Debug.Log("Mode: " + mode); // Print mode on debug log
        }
        else if (((hitPrimary.collider && hitPrimary.collider.tag == "CloseSubCanvas") || (hitSecondary.collider && hitSecondary.collider.tag == "CloseSubCanvas")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            //GameObject parentObject = hitPrimary.collider.gameObject.transform.parent.gameObject;
            // Disable the parent object
            //parentObject.SetActive(false);
            subCanvasCubeT.enabled = false;
            Debug.Log("Disabling subcanvas"); // Print mode on debug log
        }

        ///////////////////////////////////////////////////////////////////////
        
        else if (((hitPrimary.collider && hitPrimary.collider.tag == "Rotation") || (hitSecondary.collider && hitSecondary.collider.tag == "Rotation")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            Debug.Log("ROTATION SELECTED");
            Canvas parentCanvas = hitPrimary.collider.GetComponentInParent<Canvas>();
            if (parentCanvas != null)
            {
                Canvas[] subCanvases = parentCanvas.GetComponentsInChildren<Canvas>();
                foreach (Canvas subCanvas in subCanvases)
                {
                    if (subCanvas.CompareTag("SubCanvasRotation"))
                    {
                        subCanvasCubeR = subCanvas;
                        subCanvasCubeR.enabled = !subCanvasCubeR.enabled; // Toggle the enabled state
                    }
                }

            }

        }

        else if (((hitPrimary.collider && hitPrimary.collider.tag == "XAxisRotate") || (hitSecondary.collider && hitSecondary.collider.tag == "XAxisRotate")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            mode = 7; // Set mode to 1
            Debug.Log("Mode: " + mode); // Print mode on debug log
        }
        else if (((hitPrimary.collider && hitPrimary.collider.tag == "YAxisRotate") || (hitSecondary.collider && hitSecondary.collider.tag == "YAxisRotate")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            mode = 8; // Set mode to 1
            Debug.Log("Mode: " + mode); // Print mode on debug log
        }
        else if (((hitPrimary.collider && hitPrimary.collider.tag == "ZAxisRotate") || (hitSecondary.collider && hitSecondary.collider.tag == "ZAxisRotate")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            mode = 9; // Set mode to 1
            Debug.Log("Mode: " + mode); // Print mode on debug log
        }
        else if (((hitPrimary.collider && hitPrimary.collider.tag == "NXAxisRotate") || (hitSecondary.collider && hitSecondary.collider.tag == "NXAxisRotate")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            mode = 10; // Set mode to 1
            Debug.Log("Mode: " + mode); // Print mode on debug log
        }
        else if (((hitPrimary.collider && hitPrimary.collider.tag == "NYAxisRotate") || (hitSecondary.collider && hitSecondary.collider.tag == "NYAxisRotate")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            mode = 11; // Set mode to 1
            Debug.Log("Mode: " + mode); // Print mode on debug log
        }
        else if (((hitPrimary.collider && hitPrimary.collider.tag == "NZAxisRotate") || (hitSecondary.collider && hitSecondary.collider.tag == "NZAxisRotate")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            mode = 12; // Set mode to 1
            Debug.Log("Mode: " + mode); // Print mode on debug log
        }

        else if (((hitPrimary.collider && hitPrimary.collider.tag == "CloseSubCanvas") || (hitSecondary.collider && hitSecondary.collider.tag == "CloseSubCanvas")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            //GameObject parentObject = hitPrimary.collider.gameObject.transform.parent.gameObject;
            // Disable the parent object
            //parentObject.SetActive(false);
            subCanvasCubeR.enabled = false;
            Debug.Log("Disabling subcanvas"); // Print mode on debug log
        }


        //////////////////////////////////////////////////////////////////////////
        /// ALL SPHERE STUFF HERE

        else if (((hitPrimary.collider && hitPrimary.collider.tag == "ChangeColor") || (hitSecondary.collider && hitSecondary.collider.tag == "ChangeColor")))
        {
            mode = 13; // Set mode to 2
            lastActionMode = mode;
            Debug.Log("Mode: " + mode); // Print mode on debug log
        }
        else if (((hitPrimary.collider && hitPrimary.collider.tag == "Scaling") || (hitSecondary.collider && hitSecondary.collider.tag == "Scaling")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            mode = 14; // Set mode to 2
            lastActionMode = mode;

            Debug.Log("Mode: " + mode); // Print mode on debug log
        }
        else if (((hitPrimary.collider && hitPrimary.collider.tag == "ScalingDecrease") || (hitSecondary.collider && hitSecondary.collider.tag == "ScalingDecrease")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            mode = 15; // Set mode to 2
            lastActionMode = mode;

            Debug.Log("Mode: " + mode); // Print mode on debug log
        }
        else if (((hitPrimary.collider && hitPrimary.collider.tag == "NoActionSphereLeft") || (hitSecondary.collider && hitSecondary.collider.tag == "NoActionSphereLeft")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            mode = 16; // Set mode to 2
            lastActionMode = mode;
            Debug.Log("Mode: " + mode); // Print mode on debug log
        }
        else if (((hitPrimary.collider && hitPrimary.collider.tag == "Exit") || (hitSecondary.collider && hitSecondary.collider.tag == "Exit")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            mode = -1; // Set mode to 2
            Debug.Log("Mode: " + mode); // Print mode on debug log
        }

        /////////////////////////
        /// ALL LMAP STUFF HERE

        /// // 17 for power

        else if (((hitPrimary.collider && hitPrimary.collider.tag == "Power") || (hitSecondary.collider && hitSecondary.collider.tag == "Power")))
        {
            Debug.Log("TURNING ON AND FF LAP");

            // Find the Lamp object in the hierarchy
            GameObject lamp = GameObject.Find("Lamp");
            if (lamp != null)
            {
                // Get the Lamp script attached to the Lamp object
                Lamp lampScript = lamp.GetComponent<Lamp>();
                if (lampScript != null)
                {
                    if (OVRInput.GetDown(OVRInput.Button.One))
                    {
                        mode = 17; // Set mode to 17
                        lastActionMode = mode;
                        Debug.Log("Mode: " + mode); // Print mode on debug log
                        ApressCount++; // Increment the press count on A button press
                        Debug.Log("A Button Press Count for Lamp: " + ApressCount);
                        if (ApressCount % 2 != 0) // Check if press count is odd
                        {
                            lampScript.TurnOn = true;
                            Debug.Log(" Toggling power for lamp - numPresses: " + ApressCount);
                        }
                        else
                        {
                            lampScript.TurnOn = false;
                            Debug.Log(" Disabling power for lamp - numPresses: " + ApressCount);
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("Lamp script not found on Lamp object.");
                }
            }
            else
            {
                Debug.LogWarning("Lamp object not found in the hierarchy.");
            }
        }



        // 18 for no action lamp
        else if (((hitPrimary.collider && hitPrimary.collider.tag == "NoActionLamp") || (hitSecondary.collider && hitSecondary.collider.tag == "NoActionLamp")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            mode = 18; // Set mode to 2
            lastActionMode = mode;
            Debug.Log("Mode: " + mode); // Print mode on debug log
        }


        /////////////////////////
        /// ALL RADIO STUFF HERE
        /// 


        else if (((hitPrimary.collider && hitPrimary.collider.tag == "PowerRadio") || (hitSecondary.collider && hitSecondary.collider.tag == "PowerRadio")))
        {

            // Find the RadioVintage object in the hierarchy
            GameObject radioVintage = GameObject.Find("RadioVintage");
            // Get the AudioSettings component attached to the RadioVintage object
            AudioSettings audioSettings = radioVintage.GetComponent<AudioSettings>();
            if (radioVintage != null)
            {
                if (OVRInput.GetDown(OVRInput.Button.One))
                {
                    mode = 21; // Set mode to 21
                    lastActionMode = mode;
                    Debug.Log("Mode: " + mode); // Print mode on debug log

                    pressCount++; // Increment the press count on A button press
                    Debug.Log("A Button Press Count: " + pressCount);
                    if (pressCount % 2 != 0) // Check if press count is odd
                    {
                        Debug.Log("Playing Audio");
                        audioSettings.PlayAudioClip(currentIndex%5);
                    }
                    else // Even press count
                    {
                        Debug.Log("Stopping Audio");
                        audioSettings.StopAudio();
                    }
                }
                Debug.Log("Is Audio Playing: " + audioSettings.IsAudioPlaying());
            }
        }

        else if (((hitPrimary.collider && hitPrimary.collider.tag == "ChangeSong") || (hitSecondary.collider && hitSecondary.collider.tag == "ChangeSong")))
        {

            // Find the RadioVintage object in the hierarchy
            GameObject radioVintage = GameObject.Find("RadioVintage");
            // Get the AudioSettings component attached to the RadioVintage object
            AudioSettings audioSettings = radioVintage.GetComponent<AudioSettings>();
            if (radioVintage != null)
            {
                if (OVRInput.GetDown(OVRInput.Button.One))
                {
                    mode = 19; // Set mode to 21
                    lastActionMode = mode;
                    Debug.Log("Mode: " + mode); // Print mode on debug log

                    numSongCount++; // Increment the press count on A button press
                    Debug.Log("Num Song Count Count: " + numSongCount);
                    
                    audioSettings.PlayAudioClip(numSongCount%5);
                    
                }
            }





        }

        else if (((hitPrimary.collider && hitPrimary.collider.tag == "NoActionRadio") || (hitSecondary.collider && hitSecondary.collider.tag == "NoActionRadio")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            mode = 20; // Set mode to 2
            lastActionMode = mode;
            Debug.Log("Mode: " + mode); // Print mode on debug log
        }


        /////////////////////////


        else
        {
            // If not colliding with UI elements, check if either pointer is pointing at an object tagged "Object", "Cube", or "Sphere"
            if ((hitPrimary.collider && (hitPrimary.collider.tag == "Object" || hitPrimary.collider.tag == "Cube" || hitPrimary.collider.tag == "Lamp" || hitPrimary.collider.tag == "Radio" || hitPrimary.collider.tag == "Sphere")) ||
                (hitSecondary.collider && (hitSecondary.collider.tag == "Object" ||  hitPrimary.collider.tag == "Lamp"  || hitSecondary.collider.tag == "Radio" || hitSecondary.collider.tag == "Cube" ||hitSecondary.collider.tag == "Sphere")))
            {
                // Enable outline of the object
                if (hitPrimary.collider)
                {
                    hitObject = hitPrimary.collider.gameObject;
                }
                else if (hitSecondary.collider)
                {
                    hitObject = hitSecondary.collider.gameObject;
                }

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
                // If not colliding with "Translation" or pointing at an object tagged "Object", "Cube", or "Sphere", disable current outline
                if (currentOutline != null)
                {
                    currentOutline.enabled = false;
                    currentOutline = null;
                }
                currentObject = null;
            }
        }

        // Check if the "A" button is pressed while the outline of an object is enabled and mode is 1
        if (OVRInput.Get(OVRInput.Button.One) && currentOutline != null && mode == 1)
        {
            // Translate the object to the right
            if (currentObject != null && currentObject.CompareTag("Cube"))
            {
                currentObject.transform.Translate(Vector3.forward * Time.deltaTime); // Translate continuously
            }
        }
        if (OVRInput.Get(OVRInput.Button.One) && currentOutline != null && mode == 2)
        {
            // Translate the object to the right
            if (currentObject != null && currentObject.CompareTag("Cube"))
            {
                currentObject.transform.Translate(Vector3.up * Time.deltaTime); // Translate continuously
            }
        }
        if (OVRInput.Get(OVRInput.Button.One) && currentOutline != null && mode == 3)
        {
            // Translate the object to the right
            if (currentObject != null && currentObject.CompareTag("Cube"))
            {
                currentObject.transform.Translate(Vector3.right * Time.deltaTime); // Translate continuously
            }
        }
        if (OVRInput.Get(OVRInput.Button.One) && currentOutline != null && mode == 4)
        {
            // Translate the object to the right
            if (currentObject != null && (currentObject.CompareTag("Cube") )  )
            {
                currentObject.transform.Translate(Vector3.back * Time.deltaTime); // Translate continuously
            }
        }
        if (OVRInput.Get(OVRInput.Button.One) && currentOutline != null && mode == 5)
        {
            // Translate the object to the right
            if (currentObject != null && currentObject.CompareTag("Cube"))
            {
                currentObject.transform.Translate(Vector3.down * Time.deltaTime); // Translate continuously
            }
        }
        if (OVRInput.Get(OVRInput.Button.One) && currentOutline != null && mode == 6)
        {
            // Translate the object to the right
            if (currentObject != null && (currentObject.CompareTag("Cube") || currentObject.CompareTag("Lamp"))  || currentObject.CompareTag("Radio"))
            {
                currentObject.transform.Translate(Vector3.left * Time.deltaTime); // Translate continuously
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////

        // poisiitve x
        if (OVRInput.Get(OVRInput.Button.One) && currentOutline != null && mode == 7)
        {
            // Translate the object to the right
            if (currentObject != null && currentObject.CompareTag("Cube"))
            {
                currentObject.transform.Rotate(Vector3.forward * 90 * Time.deltaTime); // Rotate continuously
            }
        }
        if (OVRInput.Get(OVRInput.Button.One) && currentOutline != null && mode == 8)
        {
            // Translate the object to the right
            if (currentObject != null && currentObject.CompareTag("Cube"))
            {
                currentObject.transform.Rotate(Vector3.up * 90 * Time.deltaTime); // Rotate continuously
            }
        }
        if (OVRInput.Get(OVRInput.Button.One) && currentOutline != null && mode == 9)
        {
            // Translate the object to the right
            if (currentObject != null && currentObject.CompareTag("Cube"))
            {
                currentObject.transform.Rotate(Vector3.right * 90 * Time.deltaTime); // Rotate continuously
            }
        }
        if (OVRInput.Get(OVRInput.Button.One) && currentOutline != null && mode == 10)
        {
            // Translate the object to the right
            if (currentObject != null && currentObject.CompareTag("Cube"))
            {
                currentObject.transform.Rotate(Vector3.back * 90 * Time.deltaTime); // Rotate continuously
            }
        }
        if (OVRInput.Get(OVRInput.Button.One) && currentOutline != null && mode == 11)
        {
            // Translate the object to the right
            if (currentObject != null && currentObject.CompareTag("Cube"))
            {
                currentObject.transform.Rotate(Vector3.down * 90 * Time.deltaTime); // Rotate continuously
            }
        }
        if (OVRInput.Get(OVRInput.Button.One) && currentOutline != null && mode == 12)
        {
            // Translate the object to the right
            if (currentObject != null && currentObject.CompareTag("Cube"))
            {
                currentObject.transform.Rotate(Vector3.left * 90 * Time.deltaTime); // Rotate continuously
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////

        /* 
         if (OVRInput.Get(OVRInput.Button.One) && currentOutline != null && mode == 2)
         {
             if (currentObject != null && currentObject.CompareTag("Cube"))
             {
                 currentObject.transform.Rotate(Vector3.right * 90 * Time.deltaTime); // Rotate continuously
             }
         } */

        if (OVRInput.GetDown(OVRInput.Button.One) && currentOutline != null && mode == 13)
        {
            if (currentObject.CompareTag("Sphere"))
            {
                Renderer renderer = currentObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Material material = renderer.material;
                    if (material != null)
                    {
                        if (isFirstPress)
                        {
                            // Store the original color
                            originalColor = material.color;

                            // Change to a random color
                            Color newColor = new Color(Random.value, Random.value, Random.value);
                            material.color = newColor;
                        }
                        else
                        {
                            // Change back to the original color
                            material.color = originalColor;
                        }

                        // Toggle the boolean value
                        isFirstPress = !isFirstPress;
                    }
                }
            }
        }


        if (OVRInput.GetDown(OVRInput.Button.One) && currentOutline != null && mode == 14)
        {
            if (currentObject.CompareTag("Sphere"))
            {
                currentObject.transform.localScale *= 1.01f;
            }
        }
        if (OVRInput.GetDown(OVRInput.Button.One) && currentOutline != null && mode == 15)
        {
            if (currentObject.CompareTag("Sphere"))
            {
                currentObject.transform.localScale *= 0.99f;
            }
        }
        if (OVRInput.GetDown(OVRInput.Button.One) && currentOutline != null && mode == 16)
        {
            if (currentObject.CompareTag("Sphere"))
            {
                currentObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

                // Set material color to A4B1FF
                Renderer renderer = currentObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = new Color(164f / 255f, 177f / 255f, 255f / 255f);
                }
            }
        }



        // LAMP STUFF AND POWER FOR RADIO
     /*   if ( mode == 21 && OVRInput.GetDown(OVRInput.Button.One)) // power radio
        {
            if (currentObject.CompareTag("Radio"))
            {
                Debug.Log("TURNING ON AND OFF RADIO");

                // Get the AudioSettings component attached to the radio object
                AudioSettings audioSettings = currentObject.GetComponent<AudioSettings>();
                if (audioSettings != null)
                {
                    // Check if audio is currently playing
                    if (audioSettings.IsAudioPlaying())
                    {
                        // If audio is playing, stop it
                        audioSettings.StopAudio();
                    }
                    else
                    {
                        // If audio is not playing, start playing the current audio clip
                        audioSettings.PlayAudioClip(currentIndex);
                    }
                }
            }
        }*/

        if (OVRInput.GetDown(OVRInput.Button.One) && currentOutline != null && mode == 18) // no action
        {
            if (currentObject.CompareTag("Lamp"))
            {
                //  reset lamp
                //  light off
                Lamp lampScript = currentObject.GetComponent<Lamp>();
                if (lampScript != null)
                {
                    lampScript.TurnOn = false;
                }
                // set intensity: lamp obj has pointlight. change the intensity variable of lamp to 1
                lampScript.intensityVal = 1;
                // Temporarily disable the Rigidbody's physics updates
                Rigidbody rb = currentObject.GetComponent<Rigidbody>();

                // Set the lamp's position
                //currentObject.transform.position = new Vector3(-0.5f, -0.6f, -1.8f);


            }
        }


        if (currentOutline != null && mode == -1)
        {
            Debug.Log("Last mode: " + lastActionMode);
            if (currentObject.CompareTag("Sphere") || (currentObject.CompareTag("Lamp")))
            {
                Canvas canvas = currentObject.GetComponentInChildren<Canvas>();
                if (canvas != null)
                {
                    canvas.enabled = false;
                }
                else
                {
                    Debug.Log("CAnvas is null");
                }
            }
            mode = lastActionMode;

        }

        // power mode is 17
    }

    private void ActivateSpriteAtCollisionPoint()
    {
        if (collisionSpriteInstance != null)
        {
            collisionSpriteInstance.transform.position = collisionPoint;
            collisionSpriteInstance.SetActive(true);
            spriteActive = true;
        }
    }

    private void UpdateSpritePosition()
    {
        if (collisionSpriteInstance != null)
        {
            collisionSpriteInstance.transform.position = collisionPoint;
        }
    }

    private void DeactivateSprite()
    {
        if (collisionSpriteInstance != null)
        {
            collisionSpriteInstance.SetActive(false);
            spriteActive = false;
        }
    }
}