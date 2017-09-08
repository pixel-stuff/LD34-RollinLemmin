using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Obstacle : MonoBehaviour {

	[SerializeField] UnityEvent destructEvent;
	[SerializeField] UnityEvent shakeEvent;
	[SerializeField] UnityEvent lostSnowEvent;
	[SerializeField] UnityEvent BumpEvent;

	[System.Serializable] enum ObscacleState {
		NONE,
		BUMPED,
		SURVIVED,
		DESTROYED
	}

	[SerializeField]
	private ObscacleState _state = ObscacleState.NONE;

	public int destructPercent;
	public Vector3 destructForce;
	public int survivePercent;
	public Vector3 surviceForce;


	public Vector3 destructAndAddForce(){
		if (_state != ObscacleState.DESTROYED) {
			destructEvent.Invoke ();
			_state = ObscacleState.DESTROYED;
			return destructForce;
		}
		return Vector3.zero;
	}

	public Vector3 surviveAndAddForce(){
		shakeEvent.Invoke ();
		if (_state != ObscacleState.SURVIVED && _state != ObscacleState.DESTROYED) {
			_state = ObscacleState.SURVIVED;
			lostSnowEvent.Invoke ();
			return surviceForce;
		}
		return  Vector3.zero;
	}

	public void Bump(){
		if (_state == ObscacleState.NONE) {
			BumpEvent.Invoke ();
			_state = ObscacleState.BUMPED;
		}
	}

	public float getDestrucFactor(){
		return (float)(destructPercent) / 100.0f;
	}

	public float getSurviveFactor(){
		return (float)(survivePercent) / 100.0f;
	}
}
