using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    private float XMove;
    private float YMove;
    private float XRotation;

    [SerializeField] private Transform PlayerBody;
    public Vector2 LockAxis;
    public float Sensivity = 40f;

    public void OnStart()
    {
    }

    public void OnUpdate()
    {
        if (PlayerController.Instance.StateMachine.CurrentState.ToString() != "PlayerTrainingState")
        {
            XMove = LockAxis.x * Sensivity * Time.deltaTime;
            YMove = LockAxis.y * Sensivity * Time.deltaTime;
            XRotation -= YMove;
            XRotation = Mathf.Clamp(XRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(XRotation, 0, 0);
            PlayerBody.Rotate(Vector3.up * XMove);
        }
    }

    /// <summary>
    /// Xoay camera thủ công theo Vector3 (pitch, yaw, roll)
    /// </summary>
    public void SetCameraRotation(Vector3 rotation)
    {
        // Cập nhật góc XRotation để đồng bộ với clamp
        XRotation = Mathf.Clamp(rotation.x, -90f, 90f);

        // Cập nhật rotation của camera (pitch, roll)
        transform.localRotation = Quaternion.Euler(XRotation, rotation.z, 0);

        // Cập nhật hướng xoay của thân người chơi (yaw)
        PlayerBody.rotation = Quaternion.Euler(0, rotation.y, 0);
    }

    /// <summary>
    /// Xoay camera theo góc nhìn của Transform target
    /// </summary>
    public void LookAtTarget(Transform target, float duration = 1f)
    {
        // Hướng từ camera tới target
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(direction);

        // Tween rotation của camera
        transform.DORotateQuaternion(lookRot, duration)
            .SetEase(Ease.InOutSine);

        // Tween rotation của thân người chơi (yaw)
        Quaternion playerRot = Quaternion.Euler(0, lookRot.eulerAngles.y, 0);
        PlayerBody.DORotateQuaternion(playerRot, duration)
            .SetEase(Ease.InOutSine);

        // Cập nhật XRotation để đồng bộ sau khi tween xong
        DOVirtual.DelayedCall(duration, () =>
        {
            XRotation = lookRot.eulerAngles.x;
        });
    }

}
