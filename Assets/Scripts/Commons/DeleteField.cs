using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeleteField : MonoBehaviour, IPointerExitHandler,IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.isOnDeleteField = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.isOnDeleteField = false;
    }

}
