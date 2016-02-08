using UnityEngine;
using System.Collections;
using System;

public class lemmings : MonoBehaviour {

	public float rollingSnowAdd;
	public float lostSnow;
	public float snowValue =0.0f;
	public float maxSnow;
	private bool addSnow;

	public float lemmingInSnow;

	public float speedTresholdForAddSnow;

	public Vector3 ForceUpInFall;

	public float initialeSize = 0.32f;
	public float maxSize = 1.1f;

	public float initialeWeight = 100;
	public float maxWeight = 1000;

	public float initialeZoom = 5;
	public float maxZoom = 10;


	public float BumpZoomPercent =10;
	public float BumpZoomStep =0.5f;
	private float actualBumpZoom =0.0f;
	public float SpeedZoomPercent = 5;
	public float actualSpeedZoom =0.0f;
	public float SpeedZoomMax = 2.0f;
	public GameObject ball;

	public Camera camera;

	public Vector3 jumpForce;
	//todoSpeed
	public float speed;

	private Vector3 oldPosition;

	private CircleCollider2D m_collider ;
	private Rigidbody2D m_rigideBody;

	private bool canJump =false;

	private bool isDown = false;

	private bool isBump =true;


	public BallEffect particuleEffect;


	// Use this for initialization
	void Start () {
		m_collider = this.GetComponent<CircleCollider2D> ();
		m_rigideBody = this.GetComponent<Rigidbody2D> ();
		oldPosition = new Vector3 (0,0,0);
		m_rigideBody.angularVelocity = 5;
	}
	
	// Update is called once per frame
	void Update () {
		if (snowValue > lemmingInSnow) {
			ball.SetActive (true);
		} else {
			ball.SetActive (false);
		}

		isDown = ((this.transform.position.y - oldPosition.y) < 0) ? false : true;
		speed = (this.transform.position - oldPosition).magnitude;
		float speedSign = this.transform.position.x - oldPosition.x;
		float oldSnowValue = snowValue;
		oldPosition = this.transform.position;
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
			Debug.Log ("ADDSNOWEFFECT");
			particuleEffect.setSpeedSnowEffect (speed, (snowValue / maxSnow),speedSign);
		} else {
			Debug.Log ("ADDSNOWEFFECT");
			particuleEffect.setSpeedSnowEffect (0,0,0);
		}
		float lostSnowValue = snowValue - oldSnowValue;
		if (lostSnowValue < 0) {
			particuleEffect.degradationEffect (-lostSnowValue);
		}
		if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)){
			Jump ();
		}

		if(Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.DownArrow)){
			DropSnow ();
		}
		if (Input.GetKeyDown (KeyCode.Escape)) {
			GameStateManager.setGameState (GameState.Playing);
			Application.LoadLevel ("MenuScene");
		}
		if (Input.touchCount == 1) {    
			// touch on screen
			if (Input.GetTouch (0).phase == TouchPhase.Began) {
				Jump ();
			}
		}

	}

	void updateSize(){
		m_collider.radius = initialeSize + (maxSize * (snowValue / maxSnow));
		ball.transform.localScale = new Vector3(initialeSize + (maxSize * (snowValue / maxSnow)),initialeSize + (maxSize * (snowValue / maxSnow)),1) ;
		m_rigideBody.mass = initialeWeight + (maxWeight * (snowValue / maxSnow));
		updateZoomOnCamera ();
	}

	void updateZoomOnCamera(){
		//actualSpeedZoom = (speed / SpeedZoomMax) * SpeedZoomPercent;
		if (isBump) {
			actualBumpZoom += BumpZoomStep;
		} else {
			actualBumpZoom -= BumpZoomStep;
		}
		if (actualBumpZoom < 0 ) {
			actualBumpZoom = 0;
		}
		if (actualBumpZoom > BumpZoomPercent) {
			actualBumpZoom = BumpZoomPercent;
		}
		float initialCameraZoom = initialeZoom + (maxZoom * (snowValue / maxSnow));
		float cameraZoom = initialCameraZoom *(1 + (actualBumpZoom/100));
		//cameraZoom += initialCameraZoom *(1 + (actualSpeedZoom/100));
		camera.orthographicSize = cameraZoom;
	}


	void OnCollisionEnter2D(Collision2D other){
		isBump = false;
		if (other.gameObject.layer == LayerMask.NameToLayer ("Obstacle")) {
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
		} else if (other.gameObject.layer == LayerMask.NameToLayer ("Neige")) {
			canJump = true;
			addSnow = true;
			if (isDown) {
				m_rigideBody.AddForce (ForceUpInFall);
			}
		}
		 else if (other.gameObject.layer == LayerMask.NameToLayer ("Death")){
			GameStateManager.setGameState (GameState.GameOver);
			Application.LoadLevelAsync ("GameOverScene");
		} else if (other.gameObject.layer == LayerMask.NameToLayer ("End")){
			GameStateManager.setGameState (GameState.GameOver);
			Application.LoadLevelAsync ("SuccessGameOver");
		}

	}



	void OnCollisionStay2D(Collision2D other){
		isBump = false;
		if (other.gameObject.layer == LayerMask.NameToLayer ("Obstacle")) {
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
				if (isDown) {
					m_rigideBody.AddForce (ForceUpInFall);
				}
			}
		}

		if (other.gameObject.layer == LayerMask.NameToLayer ("Death")){
			GameStateManager.setGameState (GameState.GameOver);
			Application.LoadLevelAsync ("GameOverScene");
		} else if (other.gameObject.layer == LayerMask.NameToLayer ("End")){
			Debug.Log ("END");
			GameStateManager.setGameState (GameState.GameOver);
			Application.LoadLevelAsync ("SuccessGameOver");
		}

	}


	void OnCollisionExit2D(Collision2D other){
		m_rigideBody.AddRelativeForce (new Vector3(0,-10000,0));
		addSnow=false;
		isBump = true;
	}

	public void Jump(){
		if (canJump) {
			canJump = false;
			m_rigideBody.AddForce (jumpForce);
			particuleEffect.Jump (snowValue / maxSnow);
		}
	}

	public void DropSnow(){
		particuleEffect.DropSnow(snowValue / maxSnow);
		snowValue = 0;
	}
}
