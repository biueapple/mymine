using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointInPolyhedron : MonoBehaviour
{
    // 다면체의 면 목록 (각 면은 삼각형으로 이루어짐)
    //public Vector3[][] faces;
    public Chunk chunk;

    public bool IsPointInside(Vector3 point, List<Vector3> vertices, List<int> index)
    {
        int intersections = 0;
        Vector3 rayDirection = Vector3.right; // 임의의 방향 설정 (Vector3.one 대신 Vector3.right)

        // 삼각형 분할 및 레이 교차 검사
        foreach (var triangle in Triangulate(vertices, index))
        {
            if (RayIntersectsTriangle(point, rayDirection, triangle[0], triangle[1], triangle[2]))
            {
                intersections++;
            }
        }

        return intersections % 2 != 0;
    }

    //vertices와 triangles이 필요 (triangles은 어차피 3개씩 구성됨)
    private Vector3[][] Triangulate(List<Vector3> vertices, List<int> index)
    {
        List<Vector3[]> triangles = new ();

        for (int i = 0; i < chunk.triangles.Count; i += 3)
        {
            Vector3[] triangle = new Vector3[]
            {
            vertices[index[i]],
            vertices[index[i + 1]],
            vertices[index[i + 2]]
            };
            triangles.Add(triangle);
        }

        return triangles.ToArray();
    }

    private bool RayIntersectsTriangle(Vector3 rayOrigin, Vector3 rayDirection, Vector3 v0, Vector3 v1, Vector3 v2)
    {
        Vector3 edge1 = v1 - v0;
        Vector3 edge2 = v2 - v0;
        Vector3 h = Vector3.Cross(rayDirection, edge2);
        float a = Vector3.Dot(edge1, h);

        if (a > -Mathf.Epsilon && a < Mathf.Epsilon)
            return false; // 레이가 삼각형 평면과 평행함

        float f = 1.0f / a;
        Vector3 s = rayOrigin - v0;
        float u = f * Vector3.Dot(s, h);

        if (u < 0.0f || u > 1.0f)
            return false;

        Vector3 q = Vector3.Cross(s, edge1);
        float v = f * Vector3.Dot(rayDirection, q);

        if (v < 0.0f || u + v > 1.0f)
            return false;

        float t = f * Vector3.Dot(edge2, q);

        if (t > Mathf.Epsilon) // 레이가 삼각형과 교차
        {
            Vector3 intersectPoint = rayOrigin + rayDirection * t;
            return IsPointInTriangle(intersectPoint, v0, v1, v2);
        }
        else
            return false;
    }

    private bool IsPointInTriangle(Vector3 pt, Vector3 v0, Vector3 v1, Vector3 v2)
    {
        Vector3 v2v0 = v2 - v0;
        Vector3 v1v0 = v1 - v0;
        Vector3 ptv0 = pt - v0;

        float dot00 = Vector3.Dot(v2v0, v2v0);
        float dot01 = Vector3.Dot(v2v0, v1v0);
        float dot02 = Vector3.Dot(v2v0, ptv0);
        float dot11 = Vector3.Dot(v1v0, v1v0);
        float dot12 = Vector3.Dot(v1v0, ptv0);

        float invDenom = 1.0f / (dot00 * dot11 - dot01 * dot01);
        float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
        float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

        return (u >= 0) && (v >= 0) && (u + v < 1);
    }
}
