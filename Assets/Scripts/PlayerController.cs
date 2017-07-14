using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	public GameObject gameCam;

	public GameObject pointer;
	private bool endGame, isPaused, isBallProbablyFall;

	public GameObject ground1;
	public GameObject ground2;

	public GameObject marbles;
	private Vector3 marblesPos;

	public GameObject parentSpoon;
	private GameObject instSpoon;
	private Object[] listOfSpoons;
	private int indexSendok;
	private List<GameObject> activeSpoons;

	public Text scoreText;
	private float score;
	public GameObject gameOverImage;
	public GameObject finishImage;

	public float distanceToWin = 30f;
	private float distance;
	public Text distanceText;

	public Text countdownText;
	private int checkPoint;
	public GameObject panel;

	public Text notif;

	void Awake () {
		listOfSpoons = Resources.LoadAll ("Spoons", typeof(GameObject));
		activeSpoons = new List<GameObject> ();
		indexSendok = 0;
		distance = 0;
		score = 0;
		checkPoint = 0;
		instSpoon = null;
		endGame = false;
		isPaused = true;
		isBallProbablyFall = false;
		marblesPos = marbles.transform.position;
	}

	void Start() {
		StartCoroutine (StartCountDown());
		GameObject temp = (GameObject)listOfSpoons.GetValue (indexSendok);
		instSpoon = Instantiate (temp, temp.transform.position, temp.transform.rotation) as GameObject;
		activeSpoons.Add (instSpoon);
	}

	void Update () {

		if (endGame) {
			GameOver ();
		} else if (!endGame && !isPaused) {
			distance += Time.deltaTime;
			if (transform.position.z > ground1.transform.position.z) {
				if (Vector3.Distance (transform.position, ground2.transform.position) < 101.6f) {
					float x = ground2.transform.position.x;
					float y = ground2.transform.position.y;
					float z = ground2.transform.position.z + 152.6f;

					ground1.transform.position = new Vector3 (x, y, z);
				}
			}

			if (transform.position.z > ground2.transform.position.z) {
				if (Vector3.Distance (transform.position, ground1.transform.position) < 75f) {
					float x = ground1.transform.position.x;
					float y = ground1.transform.position.y;
					float z = ground1.transform.position.z + 161.1f;

					ground2.transform.position = new Vector3 (x, y, z);
				}
			}

			// check and change spoons
			if ((distance > 30 && distance <= 60 && indexSendok < 1) ||
				(distance > 60 && indexSendok < 2) ) {
				StartCoroutine (StartCountDown());
				Destroy (activeSpoons [0]);
				activeSpoons.RemoveAt (0);
				indexSendok += 1;
				GameObject temp = (GameObject)listOfSpoons.GetValue (indexSendok);
				instSpoon = Instantiate (temp, temp.transform.position, temp.transform.rotation) as GameObject;
				activeSpoons.Add (instSpoon);
				marbles.transform.position = marblesPos;
			}

			//notification
			if((distance > 27 && indexSendok < 1) || (distance > 57 && indexSendok < 2)) {
				notif.enabled = true;
			}

			// Score
			if (distance <= 10) {
				score += Time.deltaTime;
			} else {
				score += distance * Time.deltaTime;
			}
			scoreText.text = "Score : " + (int)score;

			// Distance 
			distanceText.text = "Distance to Finish : " + ((int)(distanceToWin - distance)) + "m";
			if (distance >= distanceToWin) {
				FinishGame ();
			}

		}
	}

	public void SetEndGame(bool end) {
		endGame = end;
	}

	public void SetProbablyFall(bool fall) {
		isBallProbablyFall = fall;
	}

	private void GameOver() {
		gameOverImage.SetActive(true);
		pointer.SetActive (true);
		CleanUp ();
	}

	private void FinishGame() {
		distanceText.text = "";
		pointer.SetActive (true);
		finishImage.SetActive (true);
		CleanUp ();
	}

	private void CleanUp() {
		Destroy (countdownText);
		Destroy (ground1);
		Destroy (ground2);
		Destroy (marbles);
		Destroy (parentSpoon);
		Destroy (instSpoon);
	}

	IEnumerator StartCountDown() {
		if (!endGame && !isBallProbablyFall) {
			isPaused = true;
			marbles.GetComponent<MarbleController> ().SetPause (true);
			ground1.GetComponent<GroundController> ().SetPause (true);
			ground2.GetComponent<GroundController> ().SetPause (true);

			if (distance > 0) {
				scoreText.enabled = false;
				distanceText.enabled = false;

				panel.SetActive (true);
				countdownText.text = "Checkpoint " + checkPoint;
				yield return new WaitForSeconds (2.0f);
				countdownText.text = "Relax and reset your head position";
				yield return new WaitForSeconds (2.0f);
				panel.SetActive (false);

				scoreText.enabled = true;
				distanceText.enabled = true;
			}

			countdownText.text = "Ready !";
			yield return new WaitForSeconds (1.5f);
			countdownText.text = "Go !";
			yield return new WaitForSeconds (0.5f);

			checkPoint++;
			countdownText.text = "";
			isPaused = false;
			marbles.GetComponent<MarbleController> ().SetPause (false);
			ground1.GetComponent<GroundController> ().SetPause (false);
			ground2.GetComponent<GroundController> ().SetPause (false);
			instSpoon.transform.SetParent (gameCam.transform);
		}
	}
}
