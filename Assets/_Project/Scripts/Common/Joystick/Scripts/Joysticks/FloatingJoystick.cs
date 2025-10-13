using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    private Vector2 startPos;
    protected override void Start()
    {
        base.Start();
        startPos.x = Screen.width / 2f;
        startPos.y = Screen.height / 6f;
        background.anchoredPosition = ScreenPointToAnchoredPosition(startPos);
    }
    private void Cancel()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        OnPointerUp(eventData);
    }
    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        base.HandleInput(magnitude, normalised, radius, cam);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(startPos);
        base.OnPointerUp(eventData);
    }
}