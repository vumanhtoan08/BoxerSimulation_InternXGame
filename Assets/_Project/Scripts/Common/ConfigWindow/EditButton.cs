using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DX.CF
{
    public class EditButton : MonoBehaviour, IPointerClickHandler, IDragHandler
    {
        private RectTransform _rectTransform;
        public RectTransform rectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = GetComponent<RectTransform>();
                }
                return _rectTransform;
            }
        }
        public UnityEvent onClick;
        private bool isDragging;
        private bool GetDrag()
        {
            if (isDragging)
            {
                isDragging = false;
                return true;
            }
            return false;
        }
        public void OnDrag(PointerEventData eventData)
        {
            isDragging = true;
            rectTransform.anchoredPosition = eventData.position;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (GetDrag()) return;
            onClick?.Invoke();
        }
    }
}
