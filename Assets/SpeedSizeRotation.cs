using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedSizeRotation : MonoBehaviour {

	public float currentSpeed;
	public float currentRadius;
	public void setCurrentRadius(float value){
		currentRadius = value;
	}

	public float lostDegreePerSecondPerSecond = 90;

	public bool isOnGround;

	public float degreePerSeconde;

	private Vector3 previousPosition;
	// Use this for initialization
	void Start () {
		previousPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		float time = Time.deltaTime;
		if (isOnGround) {
			//Calculate RotationSpeed
			Vector3 speed = (this.transform.position - previousPosition);
			float distance = (this.transform.position - previousPosition).magnitude;
			if (distance > 0) {
				currentSpeed = distance / time * Mathf.Sign (speed.x);
				float perim = currentRadius * 2 * Mathf.PI; 
				degreePerSeconde = (currentSpeed / perim) * 360;
			}
		} else {
			//Remove inertie
			float degreePerSecondAbs = Mathf.Abs(degreePerSeconde);
			if (degreePerSecondAbs < lostDegreePerSecondPerSecond) {
				degreePerSeconde = 0;
			} else 
				degreePerSeconde = Mathf.Sign(degreePerSecondAbs) * (degreePerSecondAbs - (lostDegreePerSecondPerSecond * time));
		}

		previousPosition = this.transform.position;
		ApplyRotationSpeed(time, degreePerSeconde);
	}

	void ApplyRotationSpeed(float deltaTime, float speed){
		this.transform.eulerAngles = new Vector3 (0, 0, this.transform.eulerAngles.z + -speed * deltaTime);
	}
}
