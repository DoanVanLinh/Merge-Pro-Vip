using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SplitField : MonoBehaviour,  IPointerExitHandler, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.isOnUndoField = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.isOnUndoField = false;
    }
}
