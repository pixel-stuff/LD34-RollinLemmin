using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (EdgeCollider2D))]
public class BezierCollider2D : MonoBehaviour 
{
	private float yOffset = 0.4f;
	public Vector2 firstPoint;
	public Vector2 secondPoint;

	public Vector2 handlerFirstPoint;
	public Vector2 handlerSecondPoint;

	public int pointsQuantity;

	public int pointsMultiplicator =1;
	private Vector3 _t;
	List<Vector2> points;


	public GameObject prefab;
	public float xSize;
	public float securityScale =0.08f;

	public uint _NFreq = 4;

	// Reference to the mesh we will generate
	private Mesh mesh = null;
	// Mutable lists for all the vertices and triangles of the mesh
	private List<Vector3> vertices = new List<Vector3>();
	private List<int> triangles = new List<int>();




	Vector3 CalculateBezierPoint(float t,Vector3 p0,Vector3 handlerP0,Vector3 handlerP1,Vector3 p1) {
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


	public List<Vector2> calculate2DPoints() {
		_t = this.transform.position;
		points = new List<Vector2>();

		points.Add(firstPoint);
		for(int i=1;i<pointsQuantity;i++)
		{
			points.Add(CalculateBezierPoint((1f/pointsQuantity)*i,firstPoint,handlerFirstPoint,handlerSecondPoint,secondPoint));
		}
		points.Add(secondPoint);
		this.GetComponent<EdgeCollider2D> ().points = points.ToArray ();
		return points;
	}


	void Start() {
		//pointsQuantity = pointsQuantity * pointsMultiplicator;
		//calculate2DPoints ();
		generateMesh ();
		/*for (int i = 0; i < points.Count ; i++) {
			Vector3 guiPositionPoint2;
			Vector3 guiPositionPoint =points[i];
			if(i+1 <points.Count){
				guiPositionPoint2=points[i+1];
			}else{
				guiPositionPoint2 = secondPoint;
			}


			GameObject toto = Instantiate (prefab);
			toto.transform.position = new Vector3 (_t.x+guiPositionPoint.x, _t.y+guiPositionPoint.y-(5.40f*toto.transform.localScale.y) +(guiPositionPoint.y - guiPositionPoint2.y)/2, 0);
			toto.transform.parent = this.transform;
			toto.transform.localScale = new Vector3(securityScale + ((guiPositionPoint2.x - guiPositionPoint.x)/xSize),toto.transform.localScale.y,1);
		}*/
	}

	void OnDrawGizmos() {

		if (points == null) {
			points = calculate2DPoints ();
		}
		if (points != null) {
			_t = this.transform.position;
			Gizmos.color = Color.blue;
			for (int i = 0; i < points.Count - 2; i++) {
				Gizmos.DrawLine (new Vector3 (points [i].x + _t.x, points [i].y + _t.y), new Vector3 (points [i + 1].x + _t.x, points [i + 1].y + _t.y));
			}
		}
		Gizmos.color = Color.green;
		Gizmos.DrawLine (new Vector3 (firstPoint.x+_t.x,firstPoint.y+_t.y,0),new Vector3 (handlerFirstPoint.x+_t.x,handlerFirstPoint.y+_t.y,0));
		Gizmos.DrawLine (new Vector3 (secondPoint.x+_t.x,secondPoint.y+_t.y,0),new Vector3 (handlerSecondPoint.x+_t.x,handlerSecondPoint.y+_t.y,0));
	}




	private void generateMesh()
	{
		// Get a reference to the mesh component and clear it
		MeshFilter filter = GetComponent<MeshFilter>();
		mesh = filter.mesh;
		mesh.Clear();

		triangles.Clear();
		vertices.Clear();

		for (int i = 0; i < _NFreq; i++)
		{
			float t = (float)i / (float)(_NFreq - 1);
			// Get the point on our curve using the 4 points generated above
			Vector3 p = CalculateBezierPoint(t, firstPoint, handlerFirstPoint, handlerSecondPoint, secondPoint);
			AddTerrainPoint(p);
		}

		// Assign the vertices and triangles to the mesh
		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
	}

	void AddTerrainPoint(Vector3 point)
	{
		// Create a corresponding point along the bottom
		point.y += yOffset;
		vertices.Add(new Vector3(point.x, -100f, 0f));
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
}