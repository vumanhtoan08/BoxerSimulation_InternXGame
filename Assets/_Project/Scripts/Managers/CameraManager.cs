using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    public Camera mainCamera;
    //2D OrthorCamera
    public void FitCameraToBounds(Bounds bounds)
    {
        float screenRatio = (float)Screen.width / Screen.height;
        float targetRatio = bounds.size.x / bounds.size.y;

        if (screenRatio >= targetRatio)
        {
            mainCamera.orthographicSize = bounds.extents.y;
        }
        else
        {
            mainCamera.orthographicSize = bounds.extents.x / screenRatio;
        }

        mainCamera.transform.position = new Vector3(bounds.center.x, bounds.center.y, transform.transform.position.z);
    }
    public void FitCameraToBounds3D(Bounds targetBounds)
    {
        if (mainCamera == null || !mainCamera.orthographic)
        {
            Debug.LogError("Please assign an orthographic camera.");
            return;
        }
        // Calculate the bounds' corners in the camera's local space
        Vector3[] worldCorners = new Vector3[8];
        worldCorners[0] = targetBounds.min;
        worldCorners[1] = new Vector3(targetBounds.min.x, targetBounds.min.y, targetBounds.max.z);
        worldCorners[2] = new Vector3(targetBounds.min.x, targetBounds.max.y, targetBounds.min.z);
        worldCorners[3] = new Vector3(targetBounds.min.x, targetBounds.max.y, targetBounds.max.z);
        worldCorners[4] = new Vector3(targetBounds.max.x, targetBounds.min.y, targetBounds.min.z);
        worldCorners[5] = new Vector3(targetBounds.max.x, targetBounds.min.y, targetBounds.max.z);
        worldCorners[6] = new Vector3(targetBounds.max.x, targetBounds.max.y, targetBounds.min.z);
        worldCorners[7] = targetBounds.max;

        // Transform the corners to the camera's local space
        Matrix4x4 cameraToWorld = mainCamera.transform.worldToLocalMatrix;
        Vector3[] localCorners = new Vector3[8];
        for (int i = 0; i < worldCorners.Length; i++)
        {
            localCorners[i] = cameraToWorld.MultiplyPoint3x4(worldCorners[i]);
        }

        // Find the bounds in the camera's local space
        Bounds localBounds = new Bounds(localCorners[0], Vector3.zero);
        foreach (Vector3 corner in localCorners)
        {
            localBounds.Encapsulate(corner);
        }

        // Adjust the orthographic size and camera position
        float orthographicSize = Mathf.Max(
            Mathf.Abs(localBounds.extents.y),
            Mathf.Abs(localBounds.extents.x / mainCamera.aspect)
        );

        // if (SdkUtil.isiPad())
        // {
        //     orthographicSize += 2;
        // }

        mainCamera.orthographicSize = orthographicSize;

        // Adjust the camera position
        Vector3 cameraCenter = targetBounds.center;
        mainCamera.transform.position = cameraCenter + (mainCamera.transform.rotation * Vector3.back) * Mathf.Abs(mainCamera.transform.position.z);
        Vector3 pos = mainCamera.transform.position + mainCamera.transform.forward * -60;
        mainCamera.transform.position = pos;
    }
}
