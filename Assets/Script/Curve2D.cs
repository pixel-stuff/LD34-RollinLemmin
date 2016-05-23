using UnityEngine;
using System.Collections.Generic;

public class Curve2D : MonoBehaviour
{
    public Transform firstPoint;
    public Transform handlerFirstPoint;
    public Transform handlerSecondPoint;
    public Transform secondPoint;
  
    public uint _NFreq = 4;

    // Reference to the mesh we will generate
    private Mesh mesh = null;

    // Mutable lists for all the vertices and triangles of the mesh
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();

    void Start()
    {
        generateMesh();
    }

    private void generateMesh()
    {
        // Get a reference to the mesh component and clear it
        MeshFilter filter = GetComponent<MeshFilter>();
        mesh = filter.mesh;
        mesh.Clear();

        triangles.Clear();
        vertices.Clear();
        
        /*for (int i = 0; i < points.Length; i++)
        {
            points[i] = new Vector3(0.5f * (float)i, Random.Range(1f, 2f), 0f);
            //AddTerrainPoint(points[i]);
        }*/
        /*AddTerrainPoint(firstPoint.position);
        AddTerrainPoint(handlerFirstPoint.position);
        AddTerrainPoint(handlerSecondPoint.position);
        AddTerrainPoint(secondPoint.position);*/

        for (int i = 0; i < _NFreq; i++)
        {
            float t = (float)i / (float)(_NFreq - 1);
            // Get the point on our curve using the 4 points generated above
            Vector3 p = CalculateBezierPoint(t, firstPoint.position, handlerFirstPoint.position, handlerSecondPoint.position, secondPoint.position);
            AddTerrainPoint(p);
        }

        // Assign the vertices and triangles to the mesh
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
    }

    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {

        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }

    void AddTerrainPoint(Vector3 point)
    {
        // Create a corresponding point along the bottom
        vertices.Add(new Vector3(point.x, 0f, 0f));
        // Then add our top point
        vertices.Add(point);
        if (vertices.Count >= 4)
        {
            // We have completed a new quad, create 2 triangles
            int start = vertices.Count - 4;
            triangles.Add(start + 0);
            triangles.Add(start + 1);
            triangles.Add(start + 2);
            triangles.Add(start + 1);
            triangles.Add(start + 3);
            triangles.Add(start + 2);
        }
    }

    void OnDrawGizmos()
    {
        Vector3 _t = transform.position;

        if (vertices == null)
        {
            generateMesh();
        }
        Gizmos.color = Color.blue;
        for (int i = 1; i < _NFreq; i++)
        {
            // Get the point on our curve using the 4 points generated above
            Vector3 p = CalculateBezierPoint((float)(i-1) / (float)(_NFreq - 1), firstPoint.position, handlerFirstPoint.position, handlerSecondPoint.position, secondPoint.position);
            Vector3 p1 = CalculateBezierPoint((float)i / (float)(_NFreq - 1), firstPoint.position, handlerFirstPoint.position, handlerSecondPoint.position, secondPoint.position);
            Gizmos.DrawLine(new Vector3(p.x + _t.x, p.y + _t.y), new Vector3(p1.x + _t.x, p1.y + _t.y));
        }
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(firstPoint.position.x + _t.x, firstPoint.position.y + _t.y, _t.z), new Vector3(handlerFirstPoint.position.x + _t.x, handlerFirstPoint.position.y + _t.y, _t.z));
        Gizmos.DrawLine(new Vector3(secondPoint.position.x + _t.x, secondPoint.position.y + _t.y, _t.z), new Vector3(handlerSecondPoint.position.x + _t.x, handlerSecondPoint.position.y + _t.y, _t.z));
    }

    void Update()
    {
        if(firstPoint.hasChanged || handlerFirstPoint.hasChanged || handlerSecondPoint.hasChanged || secondPoint.hasChanged)
        {
            generateMesh();
        }
    }
}