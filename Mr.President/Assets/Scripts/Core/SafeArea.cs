using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    RectTransform rectTransform;
    Rect Safearea;
    Vector2 minanchor;
    Vector2 maxanchor;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        Safearea = Screen.safeArea;
        minanchor = Safearea.position;
        maxanchor = minanchor + Safearea.size;

        minanchor.x /= Screen.width;
        minanchor.y /= Screen.height;
        maxanchor.x /= Screen.width;
        maxanchor.y /= Screen.height;

        rectTransform.anchorMin = minanchor;
        rectTransform.anchorMax = maxanchor;

    }


}
