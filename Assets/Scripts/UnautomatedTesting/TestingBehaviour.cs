using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hexgrid;

public class TestingBehaviour : MonoBehaviour
{
    [SerializeField] private Vector3 gizmoPos = Vector3.zero;
    [SerializeField] private Transform tracker;
    [SerializeField] private float hexSize = 3.0f;

    [SerializeField] private Transform lineStart, lineEnd;
    private List<Hex> hexes = new();

    void UpdateSingle()
    {
        if (tracker == null) return;
        Vector3 worldPosition = tracker.position;
        gizmoPos = Hex.HexToPixel(Hex.PixelToHex(worldPosition.xz(), hexSize), hexSize);
        gizmoPos.z = gizmoPos.y;
        gizmoPos.y = 0;
    }

    void UpdateLine()
    {
        if (lineStart == null || lineEnd == null) return;
        hexes = Hex.LinearPath(
            lineStart.position.xz(),
            lineEnd.position.xz(),
            hexSize);
    }

    private void DrawHex(Vector3 centre, float size)
    {
        float root3 = Mathf.Sqrt(3);
        Vector3[] points = new Vector3[6]
        {
            new(0, 0,  size), new( size * root3 / 2, 0,  size / 2), new( size * root3 / 2, 0, -size / 2),
            new(0, 0, -size), new(-size * root3 / 2, 0, -size / 2), new(-size * root3 / 2, 0,  size / 2),
        };
        for (int i = 0; i < 6; i++)
            points[i] += centre;
        for (int i = 0; i < 6; i++)
            Gizmos.DrawLine(points[i], points[(i + 1) % 6]);
    }

    private void OnDrawGizmos()
    {
        UpdateSingle();
        UpdateLine();
        
        Gizmos.color = Color.green;
        DrawHex(gizmoPos, hexSize);
        
        Gizmos.color = Color.yellow;
        // string ss = "";
        foreach(Hex hex in hexes)
        {
            // ss = ss + hex + "; ";
            DrawHex(Hex.HexToPixel(hex, hexSize).xz(), hexSize);
        }
        // print(ss);

        //Gizmos.color = Color.black;
        //float root3 = Mathf.Sqrt(3);
        //foreach(Vector2 point in lerpedPoints)
        //{
        //    Vector2 worldPos = new(
        //        hexSize * (root3 * point.x + root3 / 2 * point.y),
        //        hexSize * (3.0f / 2 * point.y)
        //    );
        //    // Gizmos.DrawSphere(worldPos.xz(), 0.1f);
        //    Gizmos.DrawSphere(point.xz(), 0.1f);
        //}
    }
}
