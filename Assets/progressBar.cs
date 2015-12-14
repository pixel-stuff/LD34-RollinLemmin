using UnityEngine;
using System.Collections;

public class progressBar : MonoBehaviour {

	public float sizeOfWorld;

	public GameObject fox;
	public GameObject lemming;
	public GameObject anchor;
	public GameObject redProgrression;

	public GameObject Ball;

	public float popFoxDist;
	public float speedFox;

	private bool foxIsPop= false;
	private float actualProgress;

	public float sizeOfBar;
	public float sizeOfRedBar;
	private float startBallX;
	// Use this for initialization
	void Start () {
		foxIsPop = false;
		startBallX = Ball.transform.position.x;
	}
	float calculateProgress(){
		return Ball.transform.position.x - startBallX;
	}
	// Update is called once per frame
	void Update () {
		
		actualProgress = calculateProgress();
		if (!foxIsPop && Ball.transform.position.x > popFoxDist) {
			fox.SetActive (true);
			foxIsPop = true;
		}

		if (foxIsPop) {
			if (fox.transform.position.x >= lemming.transform.position.x) {
				// GAME OVER
				GameStateManager.setGameState (GameState.GameOver);
				Application.LoadLevelAsync ("GameOverScene");
			}
			fox.transform.position = new Vector3 (fox.transform.position.x + speedFox, fox.transform.position.y, fox.transform.position.z);
		}
		float xAnchor = anchor.transform.position.x;
		float percentProgression = actualProgress / sizeOfWorld;

		float lemmingsPosition = sizeOfBar * percentProgression;
		lemming.transform.position = new Vector3 (xAnchor + lemmingsPosition, lemming.transform.position.y, lemming.transform.position.z);

		//adapt red bar 


		float RedTotalSize = lemmingsPosition;
		float centerPosition = xAnchor + RedTotalSize / 2;
		redProgrression.transform.position = new Vector3 (centerPosition, redProgrression.transform.position.y, redProgrression.transform.position.z);

		float Scale = RedTotalSize / sizeOfRedBar;

		redProgrression.transform.localScale = new Vector3 (Scale, redProgrression.transform.localScale.y,redProgrression.transform.localScale.z);

	}
}
