using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicJoystick : Joystick
{
    public float MoveThreshold { get { return moveThreshold; } set { moveThreshold = Mathf.Abs(value); } }

    [SerializeField] private float moveThreshold = 1;
    private bool firstTouch;
    public System.Action OnFirstTouch;
    protected override void Start()
    {
        MoveThreshold = moveThreshold;
        base.Start();
        // background.gameObject.SetActive(false);
    }
    public void ResetJoystick()
    {
        firstTouch = false;
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!firstTouch)
        {
            firstTouch = true;
            OnFirstTouch?.Invoke();
        }
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        // background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        // background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }

    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (magnitude > moveThreshold)
        {
            Vector2 difference = normalised * (magnitude - moveThreshold) * radius;
            background.anchoredPosition += difference;
        }
        base.HandleInput(magnitude, normalised, radius, cam);
    }
}