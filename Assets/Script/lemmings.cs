using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;
using UnityStandardAssets.ImageEffects;

public class lemmings : MonoBehaviour {

	[System.Serializable]
	public class RadiusEvent : UnityEvent<float> { }

	[SerializeField] RadiusEvent radiusEvent;
	[SerializeField] UnityEvent ballDestroyEvent;
	[SerializeField] UnityEvent ballBuildEvent;
	[SerializeField] UnityEvent treeDestroyEvent;
	[SerializeField] UnityEvent rockDestroyEvent;
	public float rollingSnowAdd;
	public float lostSnow;
	public float snowValue =0.0f;
	public float maxSnow;
	private bool _addSnow = false;

	public bool addSnow{
		get {return _addSnow; }
		set {
			_addSnow = value;
			this.GetComponentInChildren<SpeedSizeRotation> ().isOnGround = value;
		}
	}

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
    private float oldSpeed = 0.0f;
	public float speed;

	private Vector3 oldPosition;

	private CircleCollider2D m_collider ;
	private Rigidbody2D m_rigideBody;

	private bool canJump =false;

	private bool isDown = false;

	private bool isBump =true;


	public BallEffect particuleEffect;

	private Vector3 m_snowContactPoint;

	public GameObject DestroyEffectCointainer;

	// Use this for initialization
	void Start () {
		m_collider = this.GetComponent<CircleCollider2D> ();
		m_rigideBody = this.GetComponent<Rigidbody2D> ();
		oldPosition = new Vector3 (0,0,0);
		m_rigideBody.angularVelocity = 5;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	//	if(addSnow)
	//	castRayForStompSnow ();
		if (snowValue > lemmingInSnow) {
			ball.SetActive (true);
			ballBuildEvent.Invoke ();
		} else {
			ball.SetActive (false);
			ballDestroyEvent.Invoke ();
		}

		isDown = ((this.transform.position.y - oldPosition.y) < 0) ? false : true;
		speed = (this.transform.position - oldPosition).magnitude;
		float speedSign = this.transform.position.x - oldPosition.x;

        // update camera component acceleration fx
        // not physically accurate, but we don't care for the moment
        // acceleration should look ahead, not backwards
        Vector2 acceleration = new Vector2((this.transform.position.x - oldPosition.x), (this.transform.position.y - oldPosition.y));
        AccelerationBlur blur = camera.GetComponent<AccelerationBlur>();
        if(blur != null)
        {
            blur.m_Acceleration = acceleration.normalized * Mathf.Max(0.0f, speed - oldSpeed);
            Debug.Log(blur.m_Acceleration);
        }


        float oldSnowValue = snowValue;
		oldPosition = this.transform.position;
        oldSpeed = speed;
        snowValue -= lostSnow;
		if(snowValue < 0){
			snowValue = 0;
		}
		if (addSnow && snowValue < maxSnow && speed >= speedTresholdForAddSnow) {
			snowValue += rollingSnowAdd;
		}


		updateSize ();
		particuleEffect.UpdatePositionAndRadius (this.transform.position, m_collider.radius, addSnow ? m_snowContactPoint : Vector3.zero);

		particuleEffect.setSpeedEffect (speed);
		if (addSnow) {
			particuleEffect.setSpeedSnowEffect (speedSign,Math.Abs(speed));
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

		if(Mathf.Abs(Input.acceleration.y) > 1.5f){

			Debug.Log("Y =  "  + Input.acceleration.y);
			DropSnow ();
		}

	}

	void updateSize(){
		m_collider.radius = initialeSize + (maxSize * (snowValue / maxSnow));
		ball.transform.localScale = new Vector3(initialeSize + (maxSize * (snowValue / maxSnow)),initialeSize + (maxSize * (snowValue / maxSnow)),1) ;
		float multi = 2.3f;
		float particuleBaseSize = 0.6f;
		//ball.GetComponent<TrailRenderer> ().startWidth = initialeSize + (maxSize * (snowValue / maxSnow))* multi;
		//ball.GetComponent<TrailRenderer> ().endWidth = initialeSize + (maxSize * (snowValue / maxSnow))* multi;
		//ball.GetComponent<ParticleSystem>().shape = shapeModule;
		var toto = this.GetComponentInChildren<ParticleSystem>().main;
		toto.startSize = new ParticleSystem.MinMaxCurve (particuleBaseSize + ((maxSize * (snowValue / maxSnow)))* multi);
		m_rigideBody.mass = initialeWeight + (maxWeight * (snowValue / maxSnow));

		//this.GetComponentInChildren<SpeedSizeRotation> ().currentRadius= particuleBaseSize + ((maxSize * (snowValue / maxSnow)))* multi;

		radiusEvent.Invoke (particuleBaseSize + ((maxSize * (snowValue / maxSnow))) * multi);

		var destroysParticuleSys = DestroyEffectCointainer.GetComponentsInChildren<ParticleSystem> ();
		foreach(ParticleSystem part in destroysParticuleSys){
			var dropSnow = part.shape;
			dropSnow.radius = m_collider.radius;
		}

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
		camera.orthographicSize = Mathf.Lerp(camera.orthographicSize,cameraZoom,0.3f);
	}


	void OnCollisionEnter2D(Collision2D other){
		isBump = false;

		if (other.gameObject.layer == LayerMask.NameToLayer ("Obstacle")) {
			Obstacle otherObstacleScript = other.gameObject.GetComponent<Obstacle> ();
			if ((snowValue / maxSnow) >= otherObstacleScript.getDestrucFactor ()) {
			} else if ((snowValue / maxSnow) >= otherObstacleScript.getSurviveFactor ()) {
			} else {
				//death
				DropSnow();
				Debug.Log ("Bump");
				otherObstacleScript.Bump();
			}
		} else if (other.gameObject.layer == LayerMask.NameToLayer ("Neige")) {
			ContactPoint2D contactPoint = other.contacts [0];
			m_snowContactPoint = new Vector3( contactPoint.point.x,contactPoint.point.y,this.gameObject.transform.position.z);
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


	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.layer == LayerMask.NameToLayer ("ObstacleDestroy")) {

			Obstacle otherObstacleScript = other.gameObject.GetComponentInParent<Obstacle>();
			if ((snowValue / maxSnow) >= otherObstacleScript.getDestrucFactor ()) {
				//destruc
				m_rigideBody.AddForce (otherObstacleScript.destructAndAddForce ());
				Debug.Log ("Destruct");
				if (otherObstacleScript.tag == "Tree") {
					treeDestroyEvent.Invoke ();
				} else {
					rockDestroyEvent.Invoke ();
				}
			} else if ((snowValue / maxSnow) >= otherObstacleScript.getSurviveFactor ()) {
				//survive
				//DropSnow ();
				m_rigideBody.AddForce (otherObstacleScript.surviveAndAddForce ());
				Debug.Log ("Survive");
			}
		}
	}


	void OnCollisionStay2D(Collision2D other){
		isBump = false;
        
		if (other.gameObject.layer == LayerMask.NameToLayer ("Neige")) {
			canJump = true;
			ContactPoint2D contactPoint = other.contacts [0];
			m_snowContactPoint = new Vector3( contactPoint.point.x,contactPoint.point.y,this.gameObject.transform.position.z);
			//castRayForStompSnow ();
			addSnow = true;
			if (isDown) {
				m_rigideBody.AddForce (ForceUpInFall);
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
		m_snowContactPoint = Vector3.zero;
	}
		
	void OnDrawGizmos() {
		Gizmos.color = Color.blue;
		Gizmos.DrawLine (transform.position,new Vector3 (m_snowContactPoint.x,m_snowContactPoint.y,0));
	}

	public void Jump(){
		if (canJump) {
			canJump = false;
			m_rigideBody.AddForce (jumpForce);
			particuleEffect.Jump (snowValue / maxSnow);
		}
	}

	public void DropSnow(){
		particuleEffect.DropSnow(snowValue/maxSnow);
		snowValue = 0;
		ballDestroyEvent.Invoke ();
	}
}
