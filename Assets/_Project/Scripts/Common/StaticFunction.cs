using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class StaticFunction
{
    public static float DistanceRatioScreen()
    {
        return (float)Screen.height / Screen.width - 1920f / 1080;
    }

    public static void ChangeLayerRecursive(Transform targetTransform, int newLayer)
    {
        targetTransform.gameObject.layer = newLayer;

        foreach (Transform child in targetTransform)
        {
            ChangeLayerRecursive(child, newLayer);
        }

    }
    public static void Shuffle<T>(IList<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    public static void SwapRandomElements<T>(IList<T> list, int swapCount)
    {
        System.Random rand = new System.Random();
        for (int k = 0; k < swapCount; k++)
        {
            int index1 = rand.Next(0, list.Count);
            int index2 = rand.Next(0, list.Count);

            T temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }
    }
    public static Quaternion RotateParentToTarget(Transform parent, Transform child, Transform target)
    {
        Vector3 childWorldPosition = child.position;
        Vector3 targetWorldPosition = target.position;

        Vector3 targetDirection = (targetWorldPosition - childWorldPosition).normalized;

        Vector3 childCurrentDirection = child.forward;
        targetDirection.y = 0;
        childCurrentDirection.y = 0;
        Quaternion requiredRotation = Quaternion.FromToRotation(childCurrentDirection, targetDirection);

        Quaternion parentRotation = requiredRotation * parent.rotation;
        parentRotation = Quaternion.Euler(parentRotation.eulerAngles.x, parentRotation.eulerAngles.y, parentRotation.eulerAngles.z);

        Debug.Log($"Required Rotation: {requiredRotation.eulerAngles}");
        Debug.Log($"Target Rotation Needed: {parentRotation.eulerAngles}");
        return parentRotation;
    }
    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }
    public static void ScrollToIndex(RectTransform targetItem, ScrollRect scrollRect,float timeDelay,Action onComplete)
    {

        float xPos = targetItem.anchoredPosition.x;
        float targetX = xPos - scrollRect.viewport.rect.width / 2;
        targetX = Mathf.Clamp(targetX,0, scrollRect.content.sizeDelta.x);
        targetX = -targetX;
        if (timeDelay > 0.01f)
        {
            scrollRect.content.DOAnchorPosX(targetX, timeDelay).OnComplete(()=> {
                onComplete?.Invoke();
            });
        }
        else
        {
            Vector2 targetPos = scrollRect.content.anchoredPosition;
            targetPos.x = targetX;
            scrollRect.content.anchoredPosition = targetPos;
            onComplete?.Invoke();
        }
    }
}
