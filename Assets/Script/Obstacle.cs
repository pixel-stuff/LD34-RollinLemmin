using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

	public int destructPercent;
	public Vector3 destructForce;
	public int survivePercent;
	public Vector3 surviceForce;



	public Vector3 destructAndAddForce(){
		this.GetComponent<EdgeCollider2D> ().enabled = false;
		this.enabled = false;
		//todo animation de destruction
		return destructForce;
	}

	public Vector3 surviveAndAddForce(){
		this.GetComponent<EdgeCollider2D> ().enabled = false;
		//animation de shake
		return surviceForce;
	}

	public float getDestrucFactor(){
		return (float)(destructPercent) / 100.0f;
	}

	public float getSurviveFactor(){
		return (float)(survivePercent) / 100.0f;
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
