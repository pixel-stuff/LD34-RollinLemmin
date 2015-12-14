using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (EdgeCollider2D))]
public class BezierCollider2D : MonoBehaviour 
{
	public Vector2 firstPoint;
	public Vector2 secondPoint;

	public Vector2 handlerFirstPoint;
	public Vector2 handlerSecondPoint;

	public int pointsQuantity;
	private Vector3 _t;
	List<Vector2> points;

	public Material material;

	public Rect position = new Rect (16, 16, 128, 24);
	public Color color = Color.red;
	bool isDraw=false;

	private static Texture2D _staticRectTexture;
	private static GUIStyle _staticRectStyle;

	public GameObject prefab;
	public float xSize;
	public float securityScale =0.08f;
	Vector3 CalculateBezierPoint(float t,Vector3 p0,Vector3 handlerP0,Vector3 handlerP1,Vector3 p1)
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

	public Vector2[] calculate2DPoints()
	{
		_t = this.transform.position;
		points = new List<Vector2>();

		points.Add(firstPoint);
		for(int i=1;i<pointsQuantity;i++)
		{
			points.Add(CalculateBezierPoint((1f/pointsQuantity)*i,firstPoint,handlerFirstPoint,handlerSecondPoint,secondPoint));
		}
		points.Add(secondPoint);
		this.GetComponent<EdgeCollider2D> ().points = points.ToArray ();
		return points.ToArray();
	}
	void Start(){
		calculate2DPoints ();
		for (int i = 0; i < points.Count ; i++) {
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
		}
		//}
	}
	void OnDrawGizmos(){
		_t = this.transform.position;
		Gizmos.color = Color.blue;
		for(int i = 0; i <pointsQuantity; i++)
		{
			Gizmos.DrawLine(new Vector3(points[i].x+_t.x, points[i].y+_t.y), new Vector3(points[i+1].x+_t.x, points[i+1].y+_t.y));
		}
		Gizmos.color = Color.green;
		//Vector3 a = new Vector3 (firstPoint,0);
		Gizmos.DrawLine (new Vector3 (firstPoint.x+_t.x,firstPoint.y+_t.y,0),new Vector3 (handlerFirstPoint.x+_t.x,handlerFirstPoint.y+_t.y,0));
		Gizmos.DrawLine (new Vector3 (secondPoint.x+_t.x,secondPoint.y+_t.y,0),new Vector3 (handlerSecondPoint.x+_t.x,handlerSecondPoint.y+_t.y,0));
	}

	void OnGUI ()
	{        
		/*_t = this.transform.position;
		if (points == null) {
			calculate2DPoints ();
		}
		DrawRectangle (position, color);
		isDraw =true;*/
	}
	/*
	void DrawRectangle (Rect position, Color color)
	{   
		//if (!isDraw) {
		if( _staticRectTexture == null )
		{
			_staticRectTexture = new Texture2D( 1, 1 );
		}
		if( _staticRectStyle == null )
		{
			_staticRectStyle = new GUIStyle();
		}
		Vector3 guiPositionTransform = WorldToGuiPoint (_t);
		float distx = guiPositionTransform.x;
		float disty = guiPositionTransform.y;
			
		for (int i = 0; i < pointsQuantity-1; i++) {
				Debug.Log ("Draw");

				_staticRectTexture.SetPixel (0, 0, color);
				_staticRectTexture.Apply ();
				_staticRectStyle.normal.background = _staticRectTexture;
		Vector3 guiPositionPoint = WorldToGuiPoint (points[i]);
			Vector3 guiPositionPoint2 = WorldToGuiPoint (points[i+1]);

		position = new Rect (distx,disty, (int)(-(guiPositionPoint.x-guiPositionPoint2.x)), 24);
		Debug.Log (guiPositionPoint.x+ " "+guiPositionPoint.y+ " "+ (guiPositionPoint.x-guiPositionPoint2.x));
		Debug.Log (guiPositionTransform.x+ " "+guiPositionTransform.y);
		GUI.Box (position, GUIContent.none,_staticRectStyle);
		distx += -(guiPositionPoint.x - guiPositionPoint2.x);
		disty+= -(guiPositionPoint.y - guiPositionPoint2.y);
			}
		//}
	}

	public Vector3 WorldToGuiPoint(Vector3 position)
	{
		var guiPosition =Camera.main.WorldToScreenPoint(position);//Camera.main.WorldToScreenPoint(position);
		guiPosition.y = Screen.height - guiPosition.y;
		//guiPosition.x = Screen.width - guiPosition.x;

		return guiPosition;
	}*/
}