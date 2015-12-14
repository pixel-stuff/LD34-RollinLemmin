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

	public GameObject SpeedSnowLeft;
	public GameObject SpeedSnowRight;
	public float nbChangeState =10;
	public float actualChangeState = 0;
	public GameObject target = null;

	public ParticleSystem snowDegradation;
	public int nbPartForOneDesagration = 1;

	public float decalageRadius = 0;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}


	public void UpdatePositionAndRadius (Vector3 position, float radius){
		this.transform.position = position;
		JumpParticule.transform.position = position + new Vector3 (0, -radius+decalageRadius, 0);
		SpeedRightWindParticule.transform.position = position + new Vector3 (radius+decalageRadius + spaceBetweenEffectAndBall, 0, 0);
		SpeedLeftWindParticule.transform.position = position + new Vector3 (-radius+decalageRadius - spaceBetweenEffectAndBall, 0, 0);
		SpeedSnowLeft.transform.position = position + new Vector3 (0, -radius+decalageRadius, 0);
		SpeedSnowRight.transform.position = position + new Vector3 (0, -radius+decalageRadius, 0);
	}

	public void setSpeedEffect(float speed){
		if (speed > minimumSpeedForEffect) {
			//SpeedRightWindParticule.Emit (10);
		}
	}

	public void setSpeedSnowEffect(float speed, float factor,float speedSign){

			if (speedSign > 0) {
				target = SpeedSnowLeft;
			SpeedSnowRight.SetActive (false);
			//actualChangeState = 0;
		} 
		if (speedSign < 0) {
				target = SpeedSnowRight;
			SpeedSnowLeft.SetActive (false);
			//actualChangeState = 0;
			}
		if (target != null) {
			if (speed == 0) {
				actualChangeState++;
				if (actualChangeState > nbChangeState) {
					actualChangeState = 0;
					target.SetActive (false);// = false;
					return;
				}
			} else {
				target.SetActive (true);// = true;
			}
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
