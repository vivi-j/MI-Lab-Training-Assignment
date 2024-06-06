using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Adde


public class ButtonColorChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color collisionColor; // Color to change to on collision
    private Color originalColor; // Original color of the button image
    private Image buttonImage;   // Reference to the button's image component

    void Start()
    {
        // Get a reference to the button's image component
        buttonImage = GetComponent<Image>();

        // Store the original color of the button
        originalColor = buttonImage.color;
    }

    // Called when pointer enters the button
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Change the color of the button's image
        buttonImage.color = collisionColor;

        Debug.Log("Pointer entered button");
    }

    // Called when pointer exits the button
    public void OnPointerExit(PointerEventData eventData)
    {
        // Restore the original color of the button's image
        buttonImage.color = originalColor;

        Debug.Log("Pointer exited button");
    }
}
