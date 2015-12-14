using UnityEngine;
using System.Collections;

public class BallEffect : MonoBehaviour {


	public ParticleSystem DropSnowParticule;
	public float initialeDropNbParticule = 5;
	public float maxDropNbParticule = 100;
	public ParticleSystem JumpParticule;
	public float initialeJumpNbParticule = 50;
	public float maxJumpNbParticule = 100;
	public ParticleSystem SpeedRightWindParticule;
	public ParticleSystem SpeedLeftWindParticule;
	public float spaceBetweenEffectAndBall=1.0f;
	public float minimumSpeedForEffect=5;

	public ParticleSystem SpeedSnowLeft;
	public ParticleSystem SpeedSnowRight;
	public float initialeSpeedNbParticule = 50;
	public float maxSpeedNbParticule = 100;

	public ParticleSystem snowDegradation;
	public int nbPartForOneDesagration = 1;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}


	public void UpdatePositionAndRadius (Vector3 position, float radius){
		this.transform.position = position;
		JumpParticule.transform.position = position + new Vector3 (0, -radius, 0);
		SpeedRightWindParticule.transform.position = position + new Vector3 (radius + spaceBetweenEffectAndBall, 0, 0);
		SpeedLeftWindParticule.transform.position = position + new Vector3 (-radius - spaceBetweenEffectAndBall, 0, 0);
		SpeedSnowLeft.transform.position = position + new Vector3 (0, -radius, 0);
		SpeedSnowRight.transform.position = position + new Vector3 (0, -radius, 0);
	}

	public void setSpeedEffect(float speed){
		if (speed > minimumSpeedForEffect) {
			//SpeedRightWindParticule.Emit (10);
		}
	}

	public void setSpeedSnowEffect(float speed, float factor,float speedSign){
		Debug.Log ("TOTO " + speed);
		/*if (speed == 0) {
			SpeedSnowLeft.Stop();
			return;
		} else {
			SpeedSnowLeft.Play ();
		}*/
		if (speedSign > 0) {
		//	SpeedSnowLeft.forceOverLifetime.x.constantMax =(SpeedSnowLeft.forceOverLifetime.x.constantMax >0) ? SpeedSnowLeft.forceOverLifetime.x.constantMax : -SpeedSnowLeft.forceOverLifetime.x.constantMax;
		} else {
		//	SpeedSnowLeft.forceOverLifetime.x.constantMax =(SpeedSnowLeft.forceOverLifetime.x.constantMax >0) ? -SpeedSnowLeft.forceOverLifetime.x.constantMax : SpeedSnowLeft.forceOverLifetime.x.constantMax;
		}
	}

	public void DropSnow (float factor){
		DropSnowParticule.Emit ((int)(initialeDropNbParticule + factor *maxDropNbParticule));
	}

	public void Jump(float factor){
		JumpParticule.Emit ((int)(initialeJumpNbParticule + factor*maxJumpNbParticule));
	}

	public void degradationEffect(float snowDegra){
		snowDegradation.Emit ((int)(snowDegra * nbPartForOneDesagration));
	}
}
