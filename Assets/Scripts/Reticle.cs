using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour
{
    public PhysicsPointer pointer;
    public SpriteRenderer circleRenderer;

    public Sprite openSprite;
    public Sprite closedSprite;
    /*
    private void Update()
    {
        pointer.UpdateLength += UpdateSprite;
    }

    private void UpdateSprite(Vector3 position, bool hit)
    {
        transform.position = position;

        if (hit)
        {
            circleRenderer.sprite = closedSprite;
        }
        else
        {
            circleRenderer.sprite = openSprite;
        }
    }

    private void OnDestroy()
    {
        pointer.UpdateLength -= UpdateSprite;
    }*/
}
