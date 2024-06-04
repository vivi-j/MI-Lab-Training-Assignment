using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PointerEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    //[SerializeField] private Color normalColor = Color.white;
    //[SerializeField] private Color enterColor = Color.white;
    //[SerializeField] private Color downColor = Color.white;
    public Outline outline;
    [SerializeField] private UnityEvent PointerEnter = new UnityEvent();
    [SerializeField] private UnityEvent PointerExit = new UnityEvent();
    [SerializeField] private UnityEvent OnClick = new UnityEvent();

    //private MeshRenderer meshRenderer = null;

    private void Awake()
    {
        outline.enabled = false;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        //meshRenderer.material.color = enterColor;
        Debug.Log("Enter");
        outline.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //meshRenderer.material.color = normalColor;
        print("Exit");
        outline.enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //meshRenderer.material.color = downColor;
        print("Down");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //meshRenderer.material.color = enterColor;
        print("Up");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //OnClick.Invoke();
        print("Click");
    }
}
