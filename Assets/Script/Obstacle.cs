using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Obstacle : MonoBehaviour {

	[SerializeField] UnityEvent destructEvent;
	[SerializeField] UnityEvent surviveEvent;
	[SerializeField] UnityEvent BumpEvent;

	public int destructPercent;
	public Vector3 destructForce;
	public int survivePercent;
	public Vector3 surviceForce;



	public Vector3 destructAndAddForce(){
		destructEvent.Invoke ();
		return destructForce;
	}

	public Vector3 surviveAndAddForce(){
		surviveEvent.Invoke ();
		return surviceForce;
	}

	public void Bump(){
		BumpEvent.Invoke ();
	}

	public float getDestrucFactor(){
		return (float)(destructPercent) / 100.0f;
	}

	public float getSurviveFactor(){
		return (float)(survivePercent) / 100.0f;
	}
}
