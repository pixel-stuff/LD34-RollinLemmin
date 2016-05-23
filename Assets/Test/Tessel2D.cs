using UnityEngine;
using System.Collections;

public class Tessel2D : MonoBehaviour {

    private MeshFilter _meshFilter;
    private MeshCollider _meshCollider;
    private Mesh _mesh;

    public Vector3 P0;
    public Vector3 P1;

    public Vector3 handlerP0;
    public Vector3 handlerP1;

    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 handlerP0, Vector3 handlerP1, Vector3 p1)
    {
        float u = 1.0f - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0; //first term
        p += 3f * uu * t * handlerP0; //second term
        p += 3f * u * tt * handlerP1; //third term
        p += ttt * p1; //fourth term

        return p;
    }

    // Use this for initialization
    void Start ()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshCollider = GetComponent<MeshCollider>();
        _mesh = _meshFilter.mesh;
        Vector3[] vertices = _mesh.vertices;
        //wave(0.0f);
        for (int i = 0; i < _mesh.vertexCount; ++i)
        {
            Vector3 vertex = vertices[i];
            Vector3 bzp = CalculateBezierPoint(vertex.x, P0, handlerP0, handlerP1, P1);
            vertices[i].z += bzp.z;
        }
        _mesh.vertices = vertices;
        _mesh.RecalculateBounds();
        //_mesh.normals = normals;
        _mesh.RecalculateNormals();
    }

	// Update is called once per frame
	void Update () {

        Vector3[] vertices = _mesh.vertices;
        //wave(0.0f);
        for (int i = 0; i < _mesh.vertexCount; ++i)
        {
            Vector3 vertex = vertices[i];
            Vector3 bzp = CalculateBezierPoint(vertex.x, P0, handlerP0, handlerP1, P1);
            vertices[i].z += bzp.z;
        }
        _mesh.vertices = vertices;
        _mesh.RecalculateBounds();
        //_mesh.normals = normals;
        _mesh.RecalculateNormals();
    }
}
