using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FixedJoystick : Joystick
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        // Nếu isPass = fasle thì hiển thị tutorials
        if (!TutorialManager.Instance.Data.isPass)
        {
            TutorialManager.Instance.OnMoveTutorialActive(false);
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        
        if (!TutorialManager.Instance.Data.isPass && !TutorialManager.Instance.IsPassBoxing)
        {
            TutorialManager.Instance.OnInteractWithBoxingTutorial(true);
        }
    }
}