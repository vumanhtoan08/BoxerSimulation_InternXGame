using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(BoxCollider))]
public class ColliderButton : MonoBehaviour
{
    public UnityEvent OnClick;
    private void OnMouseDown()
    {
        OnClick?.Invoke();
    }
}
