using UnityEngine;
using System;
[Serializable]
public struct Quadrangle
{
    public Vector2 up;
    public Vector2 down;
    public Vector2 left;
    public Vector2 right;
    public Quadrangle(Vector2 center)
    {
        this.up = center + Vector2.up;
        this.down = center + Vector2.down;
        this.right = center + Vector2.right;
        this.left = center + Vector2.left;

    }
    public Quadrangle(Vector2 up, Vector2 down, Vector2 left, Vector2 right)
    {
        this.up = up;
        this.down = down;
        this.right = right;
        this.left = left;
    }
    public bool IsContains(Vector2 position)
    {
        if (!CheckLineIntersectWithShape(position, position + Vector2.up * float.MaxValue))
        {
            return false;
        }
        if (!CheckLineIntersectWithShape(position, position + Vector2.right * float.MaxValue))
        {
            return false;
        }
        if (!CheckLineIntersectWithShape(position, position - Vector2.up * float.MaxValue))
        {
            return false;
        }
        if (!CheckLineIntersectWithShape(position, position - Vector2.right * float.MaxValue))
        {
            return false;
        }
        return true;
    }
    public bool IsIntersect(Bounds bounds)
    {
        return false;
    }
    public static bool LineSegmentsIntersect(Vector2 lineOneA, Vector2 lineOneB, Vector2 lineTwoA, Vector2 lineTwoB)
    {
        return (((lineTwoB.y - lineOneA.y) * (lineTwoA.x - lineOneA.x) > (lineTwoA.y - lineOneA.y) * (lineTwoB.x - lineOneA.x)) != ((lineTwoB.y - lineOneB.y) * (lineTwoA.x - lineOneB.x) > (lineTwoA.y - lineOneB.y) * (lineTwoB.x - lineOneB.x)) && ((lineTwoA.y - lineOneA.y) * (lineOneB.x - lineOneA.x) > (lineOneB.y - lineOneA.y) * (lineTwoA.x - lineOneA.x)) != ((lineTwoB.y - lineOneA.y) * (lineOneB.x - lineOneA.x) > (lineOneB.y - lineOneA.y) * (lineTwoB.x - lineOneA.x)));
    }
    public bool CheckLineIntersectWithShape(Vector2 pointOne, Vector2 pointTwo)
    {
        if (LineSegmentsIntersect(right, up, pointOne, pointTwo))
        {
            return true;
        }
        if (LineSegmentsIntersect(up, left, pointOne, pointTwo))
        {
            return true;
        }
        if (LineSegmentsIntersect(left, down, pointOne, pointTwo))
        {
            return true;
        }
        if (LineSegmentsIntersect(down, right, pointOne, pointTwo))
        {
            return true;
        }
        return false;
    }
}
