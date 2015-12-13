using UnityEngine;
using System.Collections;
using System;

public class lemmings : MonoBehaviour {

	public float rollingSnowAdd;
	public float lostSnow;
	public float snowValue =0.0f;
	public float maxSnow;
	private bool addSnow;

	public float speedTresholdForAddSnow;

	public float initialeSize = 0.32f;
	public float maxSize = 1.1f;

	public float initialeWeight = 100;
	public float maxWeight = 1000;

	public float initialeZoom = 5;
	public float maxZoom = 10;

	public GameObject ball;

	public Camera camera;

	public Vector3 jumpForce;
	//todoSpeed
	public float speed;

	private Vector3 oldPosition;

	private CircleCollider2D m_collider ;
	private Rigidbody2D m_rigideBody;

	private bool canJump =false;


	public BallEffect particuleEffect;


	// Use this for initialization
	void Start () {
		m_collider = this.GetComponent<CircleCollider2D> ();
		m_rigideBody = this.GetComponent<Rigidbody2D> ();
		oldPosition = new Vector3 (0,0,0);
	}
	
	// Update is called once per frame
	void Update () {
		speed = (this.transform.position - oldPosition).magnitude;
		float oldSnowValue = snowValue;
		snowValue -= lostSnow;
		if(snowValue < 0){
			snowValue = 0;
		}
		if (addSnow && snowValue < maxSnow && speed >= speedTresholdForAddSnow) {
			snowValue += rollingSnowAdd;
		}
		updateSize ();
		particuleEffect.UpdatePositionAndRadius (this.transform.position, m_collider.radius);

		particuleEffect.setSpeedEffect (speed);
		if (addSnow) {
			particuleEffect.setSpeedSnowEffect (speed, (snowValue / maxSnow));
		} else {
			particuleEffect.setSpeedSnowEffect (0,0);
		}
		float lostSnowValue = snowValue - oldSnowValue;
		if (lostSnowValue < 0) {
			particuleEffect.degradationEffect (-lostSnowValue);
		}
		if(Input.GetKeyDown("a") || Input.GetKeyDown("a")){
			Jump ();
		}

		if(Input.GetKeyDown("z") || Input.GetKeyDown("z")){
			DropSnow ();
		}
	}

	void updateSize(){
		m_collider.radius = initialeSize + (maxSize * (snowValue / maxSnow));
		ball.transform.localScale = new Vector3(initialeSize + (maxSize * (snowValue / maxSnow)),initialeSize + (maxSize * (snowValue / maxSnow)),1) ;
		m_rigideBody.mass = initialeWeight + (maxWeight * (snowValue / maxSnow));
		camera.orthographicSize =  initialeZoom + (maxZoom * (snowValue / maxSnow));
	}


	void OnCollisionEnter2D(Collision2D other){
		Debug.Log ("here");
		if (other.gameObject.layer == LayerMask.NameToLayer ("Obstacle")) {
			Debug.Log ("COLLIDE");
			Obstacle otherObstacleScript = other.gameObject.GetComponent<Obstacle> ();
			if ((snowValue / maxSnow) >= otherObstacleScript.getDestrucFactor ()) {
				//destruc
				m_rigideBody.AddForce (otherObstacleScript.destructAndAddForce ());
				Debug.Log ("DESTRUC");
			} else if ((snowValue / maxSnow) >= otherObstacleScript.getSurviveFactor()) {
				//survive
				DropSnow();
				m_rigideBody.AddForce (otherObstacleScript.surviveAndAddForce ());
				Debug.Log("Survive");
			} else {
				//death
				Debug.Log("DEATH");
			}
		} else {
			canJump = true;
			if (other.gameObject.layer == LayerMask.NameToLayer ("Neige")) {
				addSnow = true;
			}

		}
	}


	void OnCollisionStay2D(Collision2D other){
		if (other.gameObject.layer == LayerMask.NameToLayer ("Obstacle")) {
			Debug.Log ("COLLIDE");
			Obstacle otherObstacleScript = other.gameObject.GetComponent<Obstacle> ();
			if ((snowValue / maxSnow) >= otherObstacleScript.getDestrucFactor ()) {
				//destruc
				m_rigideBody.AddForce (otherObstacleScript.destructAndAddForce ());
				Debug.Log ("DESTRUC");
			} else if ((snowValue / maxSnow) >= otherObstacleScript.getSurviveFactor ()) {
				//survive
				DropSnow ();
				m_rigideBody.AddForce (otherObstacleScript.surviveAndAddForce ());
				Debug.Log ("Survive");
			} else {
				//death
				Debug.Log ("DEATH");
			}
		} else {
			canJump = true;
			if (other.gameObject.layer == LayerMask.NameToLayer ("Neige")) {
				addSnow = true;
			}
		}
	}


	void OnCollisionExit2D(Collision2D other){
		
		addSnow=false;
	}

	void Jump(){
		if (canJump) {
			canJump = false;
			Debug.Log ("JUMP");
			m_rigideBody.AddForce (jumpForce);
			particuleEffect.Jump (snowValue / maxSnow);
		}
	}

	void DropSnow(){
		//this.GetComponent<ParticleSystem> ().Emit((int)Math.Floor(initialeNbParticule + (maxNbParticule * (snowValue / maxSnow))));
		particuleEffect.DropSnow(snowValue / maxSnow);
		snowValue = 0;
	}
}
