using UnityEngine;
using System.Collections;

public class foxGuide : MonoBehaviour {

	public Vector2 hit2DPoint;

	public Vector2 orientation;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		RaycastHit2D hit = Physics2D.Raycast(transform.position, orientation, LayerMask.NameToLayer ("Neige"));
		if (hit.collider != null) {
			hit2DPoint = hit.point;
			//float distance = Mathf.Abs(hit.point.y - transform.position.y);
		}
	}


	void OnDrawGizmos() {

	
		Gizmos.color = Color.red;
		if (hit2DPoint != Vector2.zero)
		Gizmos.DrawLine (transform.position,new Vector3 (hit2DPoint.x,hit2DPoint.y,0));
	//	Gizmos.DrawLine (new Vector3 (secondPoint.x+_t.x,secondPoint.y+_t.y,0),new Vector3 (handlerSecondPoint.x+_t.x,handlerSecondPoint.y+_t.y,0));
	}

}
