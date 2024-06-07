using UnityEngine;
using UnityEngine.UI;

public class PhysicsPointer : MonoBehaviour
{
    public GameObject openReticleSprite;
    public GameObject closedReticleSprite;
    public Material normalMaterial;
    public Material pressedMaterial;
    public Renderer pointerRenderer;
    public float lampIntensity = 0.0f; // Default lamp intensity
    public float defaultLength;
    public Slider intensitySlider; // Reference to the slider controlling lamp intensity
    public Slider intensitySliderR;
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
    // Define a variable to track the direction of movement
    private bool increasing = true;
    private bool increasingR = true;
    private GameObject fillObject;
    private GameObject fillObjectR;

    public Renderer otherRenderer;
    private LineRenderer otherLineRenderer = null;


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        otherLineRenderer = otherRenderer.GetComponent<LineRenderer>();
    }

    private void Update()
    {
        // if there is any press on the controller, change the material of the pointer

        // Check if any button on the controller is pressed
        if (OVRInput.Get(OVRInput.Button.Any))
        {
            pointerRenderer.material = pressedMaterial;
        }
        else
        {
            pointerRenderer.material = normalMaterial;
        }

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
        {
        endPosition = hit.point + hit.normal * 0.05f; // Adjust the multiplier as needed

            // Activate and set position of open reticle sprite
            openReticleSprite.transform.position = hit.point;
            openReticleSprite.SetActive(true);
            // Make open reticle face towards the center eye camera
            openReticleSprite.transform.localScale = Vector3.one * 0.1f;
            openReticleSprite.transform.LookAt(Camera.main.transform.position);

            // Activate and set position of closed reticle sprite when any button is pressed
            if (OVRInput.Get(OVRInput.Button.Any))
            {
                closedReticleSprite.transform.position = hit.point;
                closedReticleSprite.SetActive(true);
                // Make closed reticle face towards the center eye camera
                closedReticleSprite.transform.LookAt(Camera.main.transform.position);
                closedReticleSprite.transform.localScale = Vector3.one * 0.1f;
            }
            else // Disable closed reticle sprite if no button is pressed
            {
                closedReticleSprite.SetActive(false);
            }
        }
        else // Disable both reticle sprites if there is no collision
        {
            openReticleSprite.SetActive(false);
            closedReticleSprite.SetActive(false);
        }

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
        }
        else if (hitSecondary.collider)
        {
            hitObject = hitSecondary.collider.gameObject;
        }
        ///////////////////////////////////////////////////////////////////////
        ///

        // Button color change logic
        if (hitObject != null && (hitObject.name == "Button" || hitObject.name == "Handle" || hitObject.name == "HandleRadio"))
        {
            Image buttonImage = hitObject.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = new Color32(0xFF, 0x56, 0x56, 0xFF);
            }
        }

        else
        {
            string[] tagsToCheck = { "Translation", "Rotation", "RotationM", "RotationR", "NoAction", "Exit", "XAxisTranslate", "TranslationM", "TranslationR", "NXAxisTranslate", "YAxisTranslate", "NYAxisTranslate", "ZAxisTranslate", "NZAxisTranslate", "CloseSubCanvas", "CloseSubCanvasR", "XAxisRotate", "NXAxisRotate", "YAxisRotate", "NYAxisRotate", "ZAxisRotate", "NZAxisRotate", "ChangeColor", "Scaling", "ScalingDecrease", "PowerRadio", "ChangeSong", "NoActionRadio", "Power", "NoActionLamp", "NoActionSphereLeft", "Handle", "HandleRadio", "ExitRadio", "ExitLamp" };
            foreach (string tagToCheck in tagsToCheck)
            {
                GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tagToCheck);
                foreach (GameObject obj in objectsWithTag)
                {
                    Image objectImage = obj.GetComponent<Image>();
                    if (objectImage != null)
                    {
                        objectImage.color = new Color32(0xFF, 0xFF, 0xE3, 0xFF);
                    }
                }

            }

        }



        ///////////////////////////////////////////////////////////////////////

        // Check if either pointer collides with something tagged "Translation"
        if (((hitPrimary.collider && hitPrimary.collider.tag == "Translation") || (hitSecondary.collider && hitSecondary.collider.tag == "Translation")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            Canvas[] canvases = FindObjectsOfType<Canvas>();
            Canvas subCanvas = null;

            foreach (Canvas canvas in canvases)
            {
                if (canvas.gameObject.name == "SubCanvasL")
                {
                    subCanvas = canvas;
                    break;
                }
            }

            if (subCanvas != null)
            {
                Debug.Log("TRANSLATION SELECTED");
                subCanvas.enabled = true;
            }
        }

        // Check if either pointer collides with something tagged "Translation"
        else if (((hitPrimary.collider && hitPrimary.collider.tag == "TranslationM") || (hitSecondary.collider && hitSecondary.collider.tag == "TranslationM")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            Canvas[] canvases = FindObjectsOfType<Canvas>();
            Canvas subCanvas = null;

            foreach (Canvas canvas in canvases)
            {
                if (canvas.gameObject.name == "SubCanvasM")
                {
                    subCanvas = canvas;
                    break;
                }
            }

            if (subCanvas != null)
            {
                Debug.Log("TRANSLATION SELECTED");
                subCanvas.enabled = true;
            }
        }


        // Check if either pointer collides with something tagged "Translation"
        else if (((hitPrimary.collider && hitPrimary.collider.tag == "TranslationR") || (hitSecondary.collider && hitSecondary.collider.tag == "TranslationR")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            Canvas[] canvases = FindObjectsOfType<Canvas>();
            Canvas subCanvas = null;

            foreach (Canvas canvas in canvases)
            {
                if (canvas.gameObject.name == "SubCanvasR")
                {
                    subCanvas = canvas;
                    break;
                }
            }

            if (subCanvas != null)
            {
                Debug.Log("TRANSLATION SELECTED");
                subCanvas.enabled = true;
            }
        }



        else if (((hitPrimary.collider && hitPrimary.collider.tag == "XAxisTranslate") || (hitSecondary.collider && hitSecondary.collider.tag == "XAxisTranslate")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            mode = 1; // Set mode to 1
            lastActionMode = mode;
            Debug.Log("Mode: " + mode); // Print mode on debug log
        }
        else if (((hitPrimary.collider && hitPrimary.collider.tag == "YAxisTranslate") || (hitSecondary.collider && hitSecondary.collider.tag == "YAxisTranslate")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            mode = 2; // Set mode to 1
            lastActionMode = mode;
            Debug.Log("Mode: " + mode); // Print mode on debug log
        }
        else if (((hitPrimary.collider && hitPrimary.collider.tag == "ZAxisTranslate") || (hitSecondary.collider && hitSecondary.collider.tag == "ZAxisTranslate")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            mode = 3; // Set mode to 1
            lastActionMode = mode;
            Debug.Log("Mode: " + mode); // Print mode on debug log
        }
        else if (((hitPrimary.collider && hitPrimary.collider.tag == "NXAxisTranslate") || (hitSecondary.collider && hitSecondary.collider.tag == "NXAxisTranslate")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            mode = 4; // Set mode to 1
            lastActionMode = mode;
            Debug.Log("Mode: " + mode); // Print mode on debug log
        }
        else if (((hitPrimary.collider && hitPrimary.collider.tag == "NYAxisTranslate") || (hitSecondary.collider && hitSecondary.collider.tag == "NYAxisTranslate")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            mode = 5; // Set mode to 1
            lastActionMode = mode;
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
            // Determine which collider was hit
            Collider hitCollider = hitPrimary.collider ? hitPrimary.collider : hitSecondary.collider;

            // Check if the hitCollider is not null
            if (hitCollider != null)
            {
                // Find the parent object of the hit collider and disable it
                Canvas parentCanvas = hitCollider.GetComponentInParent<Canvas>();

                if (parentCanvas != null)
                {
                    parentCanvas.enabled = false;
                    Debug.Log("Parent object of CloseSubCanvas disabled.");
                }
                else
                {
                    Debug.LogWarning("No parent object found for the CloseSubCanvas collider.");
                }
            }
            else
            {
                Debug.LogWarning("No valid collider hit for CloseSubCanvas.");
            }
        }


        else if (((hitPrimary.collider && hitPrimary.collider.tag == "CloseSubCanvasR") || (hitSecondary.collider && hitSecondary.collider.tag == "CloseSubCanvasR")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            // Determine which collider was hit
            Collider hitCollider = hitPrimary.collider ? hitPrimary.collider : hitSecondary.collider;

            // Check if the hitCollider is not null
            if (hitCollider != null)
            {
                // Find the parent object of the hit collider and disable it
                Canvas parentCanvas = hitCollider.GetComponentInParent<Canvas>();

                if (parentCanvas != null)
                {
                    parentCanvas.enabled = false;
                    Debug.Log("Parent object of CloseSubCanvasR disabled.");
                }
                else
                {
                    Debug.LogWarning("No parent object found for the CloseSubCanvas collider.");
                }
            }
            else
            {
                Debug.LogWarning("No valid collider hit for CloseSubCanvas.");
            }
        }



        ///////////////////////////////////////////////////////////////////////

        else if (((hitPrimary.collider && hitPrimary.collider.tag == "Rotation") || (hitSecondary.collider && hitSecondary.collider.tag == "Rotation")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            Canvas[] canvases = FindObjectsOfType<Canvas>();
            Canvas subCanvas = null;

            foreach (Canvas canvas in canvases)
            {
                if (canvas.gameObject.name == "SubCanvas1")
                {
                    subCanvas = canvas;
                    break;
                }
            }

            if (subCanvas != null)
            {
                Debug.Log("ROTATION SELECTED");
                subCanvas.enabled = true;
            }

        }

        else if (((hitPrimary.collider && hitPrimary.collider.tag == "RotationM") || (hitSecondary.collider && hitSecondary.collider.tag == "RotationM")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            Canvas[] canvases = FindObjectsOfType<Canvas>();
            Canvas subCanvas = null;

            foreach (Canvas canvas in canvases)
            {
                if (canvas.gameObject.name == "SubCanvas1M")
                {
                    subCanvas = canvas;
                    break;
                }
            }

            if (subCanvas != null)
            {
                Debug.Log("ROTATION SELECTED");
                subCanvas.enabled = true;
            }

        }

        else if (((hitPrimary.collider && hitPrimary.collider.tag == "RotationR") || (hitSecondary.collider && hitSecondary.collider.tag == "RotationR")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            Canvas[] canvases = FindObjectsOfType<Canvas>();
            Canvas subCanvas = null;

            foreach (Canvas canvas in canvases)
            {
                if (canvas.gameObject.name == "SubCanvas1R")
                {
                    subCanvas = canvas;
                    break;
                }
            }

            if (subCanvas != null)
            {
                Debug.Log("ROTATION SELECTED");
                subCanvas.enabled = true;
            }

        }

        else if (((hitPrimary.collider && hitPrimary.collider.tag == "XAxisRotate") || (hitSecondary.collider && hitSecondary.collider.tag == "XAxisRotate")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            mode = 7; // Set mode to 1
            lastActionMode = mode;
            Debug.Log("Mode: " + mode); // Print mode on debug log
        }
        else if (((hitPrimary.collider && hitPrimary.collider.tag == "YAxisRotate") || (hitSecondary.collider && hitSecondary.collider.tag == "YAxisRotate")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            mode = 8; // Set mode to 1
            lastActionMode = mode;
            Debug.Log("Mode: " + mode); // Print mode on debug log
        }
        else if (((hitPrimary.collider && hitPrimary.collider.tag == "ZAxisRotate") || (hitSecondary.collider && hitSecondary.collider.tag == "ZAxisRotate")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            mode = 9; // Set mode to 1
            lastActionMode = mode;
            Debug.Log("Mode: " + mode); // Print mode on debug log
        }
        else if (((hitPrimary.collider && hitPrimary.collider.tag == "NXAxisRotate") || (hitSecondary.collider && hitSecondary.collider.tag == "NXAxisRotate")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            mode = 10; // Set mode to 1
            lastActionMode = mode;
            Debug.Log("Mode: " + mode); // Print mode on debug log
        }
        else if (((hitPrimary.collider && hitPrimary.collider.tag == "NYAxisRotate") || (hitSecondary.collider && hitSecondary.collider.tag == "NYAxisRotate")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            mode = 11; // Set mode to 1
            lastActionMode = mode;
            Debug.Log("Mode: " + mode); // Print mode on debug log
        }
        else if (((hitPrimary.collider && hitPrimary.collider.tag == "NZAxisRotate") || (hitSecondary.collider && hitSecondary.collider.tag == "NZAxisRotate")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            mode = 12; // Set mode to 1
            lastActionMode = mode;
            Debug.Log("Mode: " + mode); // Print mode on debug log
        }


        else if (((hitPrimary.collider && hitPrimary.collider.tag == "NoAction") || (hitSecondary.collider && hitSecondary.collider.tag == "NoAction")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            mode = 36;
            lastActionMode = mode;
            Debug.Log("Mode: " + mode); // Print mode on debug log
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
        else if (((hitPrimary.collider && hitPrimary.collider.tag == "ExitRadio") || (hitSecondary.collider && hitSecondary.collider.tag == "ExitRadio")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            //mode = -2; // Set mode to 2
            Debug.Log("Mode: " + mode); // Print mode on debug log

            Debug.Log("Last mode: " + lastActionMode);

            // Find all Canvas objects with the tag "RadioCanvas"
            Canvas[] radioCanvases = GameObject.FindObjectsOfType<Canvas>();
            foreach (Canvas radioCanvas in radioCanvases)
            {
                if (radioCanvas.CompareTag("RadioCanvas"))
                {
                    Debug.Log("Canvas name: " + radioCanvas.gameObject.name);
                    // Disable the Canvas
                    radioCanvas.enabled = false;
                }
            }

            mode = lastActionMode;

        }
        else if (((hitPrimary.collider && hitPrimary.collider.tag == "ExitLamp") || (hitSecondary.collider && hitSecondary.collider.tag == "ExitLamp")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            //mode = -3; // Set mode to 2
            Debug.Log("Mode: " + mode); // Print mode on debug log

            Debug.Log("Last mode: " + lastActionMode);

            // Find all Canvas objects with the tag "RadioCanvas"
            Canvas[] radioCanvases = GameObject.FindObjectsOfType<Canvas>();
            foreach (Canvas radioCanvas in radioCanvases)
            {
                if (radioCanvas.CompareTag("LampCanvas"))
                {
                    Debug.Log("Canvas name: " + radioCanvas.gameObject.name);
                    // Disable the Canvas
                    radioCanvas.enabled = false;
                }
            }

            mode = lastActionMode;

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


        else if (((hitPrimary.collider && hitPrimary.collider.tag == "Handle") || (hitSecondary.collider && hitSecondary.collider.tag == "Handle")))
        {
            if (OVRInput.Get(OVRInput.Button.One))
            {
                mode = 20; // Set mode to 20
                lastActionMode = mode;
                Debug.Log("Mode: " + mode); // Print mode on debug log

                Vector3 pointerPosition = (hitPrimary.collider != null) ? hitPrimary.point : hitSecondary.point;

                // Calculate the position of the pointer relative to the handle
                float distanceFromHandle = pointerPosition.x - hitPrimary.collider.transform.position.x;

                // Calculate the intensity slider value based on the pointer position
                float sliderValue = Mathf.Clamp01((distanceFromHandle)); // 10 = maxDistance

                // Update the intensity slider value
                intensitySlider.value = sliderValue;
                Debug.Log("distancefromhandle: " + distanceFromHandle);
                Debug.Log("sliderval: " + sliderValue);
                Debug.Log("pointerposition: " + pointerPosition.x);
                Debug.Log("collider pos: " + hitPrimary.collider.transform.position.x);



            }

            // find obhect with tag fill and change its image's color to red
            fillObject = GameObject.FindWithTag("Fill");

            // Check if the fillObject is found
            if (fillObject != null)
            {
                Image fillImage = fillObject.GetComponent<Image>();
                fillImage.color = originalColor;
            }
            else
            {
                Debug.Log("Fill onj is null");
            }


        }



        // 18 for no action lamp
        else if (((hitPrimary.collider && hitPrimary.collider.tag == "NoActionLamp") || (hitSecondary.collider && hitSecondary.collider.tag == "NoActionLamp")) && OVRInput.GetDown(OVRInput.Button.One))
        {
            mode = 18; // Set mode to 2
            lastActionMode = mode;
            Debug.Log("Mode: " + mode); // Print mode on debug log

            GameObject lamp = GameObject.Find("Lamp");
            Rigidbody rb = lamp.GetComponent<Rigidbody>();
            if (lamp != null)
            {
                // Set RadioVintage's position
                Transform parentTransform = lamp.transform.parent;
                if (parentTransform != null)
                {
                    rb.useGravity = false; // Disable gravity
                    // Assuming the parent is the table and we want to set the RadioVintage on top of it
                    // Adjust x and z coordinates as needed, keeping y relative to the table's height
                    lamp.transform.position = new Vector3(
                        -1f,
                        parentTransform.position.y + 0.532999992f, // Adjusted to keep it on top of the table
                        2.5f
                    );
                    lamp.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
                }

                Lamp lampScript = lamp.GetComponent<Lamp>();
                if (lampScript != null)
                {
                    rb.useGravity = true;
                    lampScript.TurnOn = false; // Turn off the lamp
                    lampScript.intensityVal = 1;
                }


            }
            else
            {
                Debug.LogWarning("RadioVintage object not found.");
            }


        }


        /////////////////////////
        /// ALL RADIO STUFF HERE
        /// 

        else if (((hitPrimary.collider && hitPrimary.collider.tag == "HandleRadio") || (hitSecondary.collider && hitSecondary.collider.tag == "HandleRadio")))
        {
            if (OVRInput.Get(OVRInput.Button.One))
            {
                mode = 25; // Set mode to 20
                lastActionMode = mode;
                Debug.Log("Mode: " + mode); // Print mode on debug log

                // Check if the slider value is increasing and within the range (0, 1)
                if (increasingR && intensitySliderR.value < 1)
                {
                    // Increase the slider value
                    intensitySliderR.value += 0.03f;
                }
                // Check if the slider value is decreasing and within the range (0, 1)
                else if (!increasingR && intensitySliderR.value > 0)
                {
                    // Decrease the slider value
                    intensitySliderR.value -= 0.03f;
                }
                // If the slider value reaches the upper limit (1), switch to decreasing
                else if (intensitySliderR.value >= 1)
                {
                    increasingR = false;
                }
                // If the slider value reaches the lower limit (0), switch to increasing
                else if (intensitySliderR.value <= 0)
                {
                    increasingR = true;
                }

                Debug.Log("HERE: " + intensitySliderR.value + " end");
                GameObject radioObject = GameObject.FindWithTag("Radio");
                AudioSource audioSettings = radioObject.GetComponent<AudioSource>();
                audioSettings.volume = intensitySliderR.value;

            }

            // find obhect with tag fill and change its image's color to red
            fillObjectR = GameObject.FindWithTag("FillRadio");

            // Check if the fillObject is found
            if (fillObjectR != null)
            {
                Image fillImage = fillObjectR.GetComponent<Image>();
                fillImage.color = originalColor;
                Debug.Log("Fill obj radio exists");
            }
            else
            {
                Debug.Log("Fill onj is null");
            }

        }


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
            // Set mode to 23
            mode = 23;
            lastActionMode = mode;
            Debug.Log("Mode: " + mode); // Print mode on debug log

            // Find the RadioVintage object
            GameObject radioVintage = GameObject.Find("RadioVintage");
            Rigidbody rb = radioVintage.GetComponent<Rigidbody>();
            if (radioVintage != null)
            {
                // Set RadioVintage's position
                Transform parentTransform = radioVintage.transform.parent;
                if (parentTransform != null)
                {
                    rb.useGravity = false; // Disable gravity
                    // Assuming the parent is the table and we want to set the RadioVintage on top of it
                    // Adjust x and z coordinates as needed, keeping y relative to the table's height
                    radioVintage.transform.position = new Vector3(
                        0.910338998f,
                        parentTransform.position.y + 0.532999992f, // Adjusted to keep it on top of the table
                        2.5f
                    );
                    radioVintage.transform.rotation = Quaternion.Euler(0f, -180f, 0f);
                }

                // Call the StopAudio() function from the AudioSettings script
                AudioSettings audioSettings = radioVintage.GetComponent<AudioSettings>();
                if (audioSettings != null)
                {
                    audioSettings.StopAudio();
                }

                // Set RadioVintage's AudioSource volume to 1
                AudioSource audioSource = radioVintage.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    rb.useGravity = true;
                    audioSource.volume = 1f;
                }
            }
            else
            {
                Debug.LogWarning("RadioVintage object not found.");
            }
        }


        /////////////////////////


        else
        {
            // If not colliding with UI elements, check if either pointer is pointing at an object tagged "Object", "Cube", or "Sphere"
            if ((hitPrimary.collider && (hitPrimary.collider.tag == "Object" || hitPrimary.collider.tag == "Cube" || hitPrimary.collider.tag == "CubeM" || hitPrimary.collider.tag == "CubeR" ||  hitPrimary.collider.tag == "Lamp" || hitPrimary.collider.tag == "Radio" || hitPrimary.collider.tag == "Sphere")) ||
                (hitSecondary.collider && (hitSecondary.collider.tag == "Object" ||  hitSecondary.collider.tag == "Lamp"  || hitSecondary.collider.tag == "Radio" || hitSecondary.collider.tag == "Cube" || hitSecondary.collider.tag == "CubeM" || hitSecondary.collider.tag == "CubeR" || hitSecondary.collider.tag == "Sphere")))
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
            if (currentObject != null && (currentObject.CompareTag("Cube") || currentObject.CompareTag("CubeM") || currentObject.CompareTag("CubeR")   ))
            {
                currentObject.transform.Translate(Vector3.forward * Time.deltaTime); // Translate continuously
            }
        }
        if (OVRInput.Get(OVRInput.Button.One) && currentOutline != null && mode == 2)
        {
            // Translate the object to the right
            if (currentObject != null && (currentObject.CompareTag("Cube") || currentObject.CompareTag("CubeM") || currentObject.CompareTag("CubeR")))
            {
                currentObject.transform.Translate(Vector3.up * Time.deltaTime); // Translate continuously
            }
        }
        if (OVRInput.Get(OVRInput.Button.One) && currentOutline != null && mode == 3)
        {
            // Translate the object to the right
            if (currentObject != null && (currentObject.CompareTag("Cube") || currentObject.CompareTag("CubeM") || currentObject.CompareTag("CubeR")))
            {
                currentObject.transform.Translate(Vector3.right * Time.deltaTime); // Translate continuously
            }
        }
        if (OVRInput.Get(OVRInput.Button.One) && currentOutline != null && mode == 4)
        {
            // Translate the object to the right
            if (currentObject != null && (currentObject.CompareTag("Cube") || currentObject.CompareTag("CubeM") || currentObject.CompareTag("CubeR")))
            {
                currentObject.transform.Translate(Vector3.back * Time.deltaTime); // Translate continuously
            }
        }
        if (OVRInput.Get(OVRInput.Button.One) && currentOutline != null && mode == 5)
        {
            // Translate the object to the right
            if (currentObject != null && (currentObject.CompareTag("Cube") || currentObject.CompareTag("CubeM") || currentObject.CompareTag("CubeR")))
            {
                currentObject.transform.Translate(Vector3.down * Time.deltaTime); // Translate continuously
            }
        }
        if (OVRInput.Get(OVRInput.Button.One) && currentOutline != null && mode == 6)
        {
            // Translate the object to the right
            if (currentObject != null && (currentObject.CompareTag("Cube") || currentObject.CompareTag("CubeM") || currentObject.CompareTag("CubeR") || currentObject.CompareTag("Lamp"))  || currentObject.CompareTag("Radio"))
            {
                currentObject.transform.Translate(Vector3.left * Time.deltaTime); // Translate continuously
            }
        }

        // no action cube
        if (OVRInput.Get(OVRInput.Button.One) && currentOutline != null && mode == 36)
        {
            currentObject.transform.rotation = Quaternion.identity;
            if (currentObject != null && (currentObject.CompareTag("Cube")))
            {
                // set position adn rotation here
                currentObject.transform.position = new Vector3(-7f, 1.5f, -7f);
            }
            else if (currentObject.CompareTag("CubeM"))
            {
                currentObject.transform.position = new Vector3(-7f, 1.5f, -2f);
            }
            else if (currentObject.CompareTag("CubeR"))
            {
                currentObject.transform.position = new Vector3(-7f, 2.5f, 3f);
            }
        }



        ////////////////////////////////////////////////////////////////////////////////////////

        // poisiitve x
        if (OVRInput.Get(OVRInput.Button.One) && currentOutline != null && mode == 7)
        {
            // Translate the object to the right
            if (currentObject != null && (currentObject.CompareTag("Cube") || currentObject.CompareTag("CubeM") || currentObject.CompareTag("CubeR")))
            {
                currentObject.transform.Rotate(Vector3.forward * 90 * Time.deltaTime); // Rotate continuously
            }
        }
        if (OVRInput.Get(OVRInput.Button.One) && currentOutline != null && mode == 8)
        {
            // Translate the object to the right
            if (currentObject != null && (currentObject.CompareTag("Cube") || currentObject.CompareTag("CubeM") || currentObject.CompareTag("CubeR")))
            {
                currentObject.transform.Rotate(Vector3.up * 90 * Time.deltaTime); // Rotate continuously
            }
        }
        if (OVRInput.Get(OVRInput.Button.One) && currentOutline != null && mode == 9)
        {
            // Translate the object to the right
            if (currentObject != null && (currentObject.CompareTag("Cube") || currentObject.CompareTag("CubeM") || currentObject.CompareTag("CubeR")))
            {
                currentObject.transform.Rotate(Vector3.right * 90 * Time.deltaTime); // Rotate continuously
            }
        }
        if (OVRInput.Get(OVRInput.Button.One) && currentOutline != null && mode == 10)
        {
            // Translate the object to the right
            if (currentObject != null && (currentObject.CompareTag("Cube") || currentObject.CompareTag("CubeM") || currentObject.CompareTag("CubeR")))
            {
                currentObject.transform.Rotate(Vector3.back * 90 * Time.deltaTime); // Rotate continuously
            }
        }
        if (OVRInput.Get(OVRInput.Button.One) && currentOutline != null && mode == 11)
        {
            // Translate the object to the right
            if (currentObject != null && (currentObject.CompareTag("Cube") || currentObject.CompareTag("CubeM") || currentObject.CompareTag("CubeR")))
            {
                currentObject.transform.Rotate(Vector3.down * 90 * Time.deltaTime); // Rotate continuously
            }
        }
        if (OVRInput.Get(OVRInput.Button.One) && currentOutline != null && mode == 12)
        {
            // Translate the object to the right
            if (currentObject != null && (currentObject.CompareTag("Cube") || currentObject.CompareTag("CubeM") || currentObject.CompareTag("CubeR")))
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




        if (OVRInput.GetDown(OVRInput.Button.One) && currentOutline != null && mode == -1)
        {
            Debug.Log("Last mode: " + lastActionMode);
                Canvas canvas = currentObject.GetComponentInChildren<Canvas>();
                Debug.Log("Canvas name: " + canvas.gameObject.name);
                if (canvas != null)
                {
                    canvas.enabled = false;
                }
                else
                {
                    Debug.Log("CAnvas is null");
                }
            
            mode = lastActionMode;
        }




        // power mode is 17
    }

}