using System;
using UnityEngine;
using UnityEngine.EventSystems;
public class TouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool buttonPressed;
    bool buttonHeld;
    public event Action<PointerEventData> OnPressDown;
    public event Action<PointerEventData> OnPressUp;
    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
        buttonHeld = true;
        OnPressDown?.Invoke(eventData);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
        buttonHeld = false;
        OnPressUp?.Invoke(eventData);
    }
    public bool GetButtonDown()
    {
        if (buttonPressed)
        {
            buttonPressed = false;
            return true;
        }
        return false;
    }
    public bool GetButton()
    {
        return buttonHeld;
    }
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
    public void ResetButton()
    {
        buttonPressed = false;
        buttonHeld = false;
    }
}
