using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class TouchPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Vector2 direction = Vector2.zero;
    public Vector2 delta = Vector2.zero;
    private bool isDragging;
    private Vector2 center;
    private int id;
    [SerializeField] private TouchButton[] touchButtons;
    private void Start()
    {
        for (int i = 0; i < touchButtons.Length; i++)
        {
            touchButtons[i].OnPressDown += OnPointerDown;
            touchButtons[i].OnPressUp += OnPointerUp;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        center = eventData.position;
        id = eventData.pointerId;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        delta = Vector2.zero;
        direction = Vector2.zero;
    }

    private void Update()
    {
        if (Input.touchCount == 0 || !isDragging) return;
        Vector3 diff = Input.touches[id].position - center;
        delta = diff;
        direction = delta.normalized;
        center = Input.touches[id].position;
    }
    public void ResetTouchPad()
    {
        delta = Vector2.zero;
        direction = Vector3.zero;
    }
}
