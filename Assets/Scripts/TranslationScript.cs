using UnityEngine;
using UnityEngine.UI;

public class TranslationScript : MonoBehaviour
{
    public enum ActionType
    {
        Translation,
        Rotation,
        NoAction,
        Exit
    }

    public Color translationColor;
    public ActionType currentAction = ActionType.NoAction;
    private Image buttonImage;

    private void Start()
    {
        buttonImage = GetComponent<Image>();
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One)) // A - NEED TO DO WHILE POINTING!
        {
            currentAction = ActionType.Translation;
           // Debug.Log("Translation");
        }
    }

    public ActionType GetCurrentAction()
    {
        return currentAction;
    }
}