using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileInputButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public string keyName;

    public void OnPointerDown(PointerEventData eventData)
    {
        RPGInputManager.SetButtonDown(keyName);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        RPGInputManager.SetButtonUp(keyName);
    }
}
