using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(QuadrangleBounds))]
public class QuadrangleBoundsEditor : Editor
{
    private void OnSceneGUI()
    {
        QuadrangleBounds area = target as QuadrangleBounds;
        area.quadrangle.up = Handles.PositionHandle(area.quadrangle.up, Quaternion.identity);
        area.quadrangle.down = Handles.PositionHandle(area.quadrangle.down, Quaternion.identity);
        area.quadrangle.left = Handles.PositionHandle(area.quadrangle.left, Quaternion.identity);
        area.quadrangle.right = Handles.PositionHandle(area.quadrangle.right, Quaternion.identity);
    }
}
#endif
public class QuadrangleBounds : MonoBehaviour
{
    public Quadrangle quadrangle;
    private void Reset()
    {
        quadrangle = new Quadrangle(transform.position);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(quadrangle.right, quadrangle.up);
        Gizmos.DrawLine(quadrangle.up, quadrangle.left);
        Gizmos.DrawLine(quadrangle.left, quadrangle.down);
        Gizmos.DrawLine(quadrangle.down, quadrangle.right);
    }
}
