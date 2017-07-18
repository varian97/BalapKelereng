﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	public GameObject gameCam;

	public GameObject pointer;
	private bool endGame, isPaused, isBallProbablyFall;

	public GameObject ground1;
	public GameObject ground2;

	private Vector3 marblesPos;
	private GameObject instMarble;
	private Object[] listOfMarbles;
	private int indexMarble;
	private List<GameObject> activeMarble;
	private GameObject empty;

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
		listOfMarbles = Resources.LoadAll ("Balls", typeof(GameObject));
		activeSpoons = new List<GameObject> ();
		activeMarble = new List<GameObject> ();

		indexMarble = 0;
		indexSendok = 0;

		distance = 0;
		score = 0;
		checkPoint = 0;

		instSpoon = null;
		instMarble = null;

		endGame = false;
		isPaused = true;
		isBallProbablyFall = false;

		empty = new GameObject ();
	}

	void Start() {
		// instantiate spoon
		GameObject temp = (GameObject)listOfSpoons.GetValue (indexSendok);
		instSpoon = Instantiate (temp, temp.transform.position, temp.transform.rotation) as GameObject;
		activeSpoons.Add (instSpoon);

		// instantiate marble
		GameObject temp2 = (GameObject) listOfMarbles.GetValue(indexMarble);
		instMarble = Instantiate (temp2, temp2.transform.transform.position, temp2.transform.rotation) as GameObject;
		activeMarble.Add (instMarble);

		StartCoroutine (StartCountDown());
	}

	void Update () {

		if (endGame) {
			GameOver ();
		} else if (!endGame && !isPaused) {
			// update the distance to finish
			distance += Time.deltaTime;

			// spawn the road if player pos near the end of the prefabs
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

			// check then change spoons and marble
			if ((distance > 10 && distance <= 60 && indexSendok < 1) ||
				(distance > 60 && indexSendok < 2) ) {

				// destroy the current spoon and marble
				Destroy (activeSpoons [0]);
				activeSpoons.RemoveAt (0);
				Destroy (activeMarble[0]);
				activeMarble.RemoveAt (0);

				indexSendok += 1;
				indexMarble += 1;

				//instantiate the new marble and spoon
				GameObject temp = (GameObject)listOfSpoons.GetValue (indexSendok);
				instSpoon = Instantiate (temp, temp.transform.position, temp.transform.rotation) as GameObject;
				activeSpoons.Add (instSpoon);

				GameObject temp2 = (GameObject)listOfMarbles.GetValue (indexMarble);
				instMarble = Instantiate (temp2, temp2.transform.transform.position, temp2.transform.rotation) as GameObject;
				activeMarble.Add (instMarble);

				StartCoroutine (StartCountDown());
			}

			//notification
			if (distance > 4 && indexSendok < 1) {
				notif.enabled = true;
				notif.text = "Distance until checkpoint : " + (int)(10 - distance) + " m";
			} else if (distance > 49 && indexSendok < 2) {
				notif.enabled = true;
				notif.text = "Distance until checkpoint : " + (int)(60 - distance) + " m";
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
		Destroy (notif);
		Destroy (countdownText);
		Destroy (ground1);
		Destroy (ground2);
		Destroy (instSpoon);
		Destroy (instMarble);
	}

	IEnumerator StartCountDown() {
		if (!endGame) {
			// unparent the marble from the spoon
			empty.transform.SetParent (null);
			instMarble.transform.SetParent (transform);

			// set pause and notif
			isPaused = true;
			notif.enabled = false;
			instMarble.GetComponent<MarbleController> ().SetPause (true);
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

			// unpause
			isPaused = false;
			instMarble.GetComponent<MarbleController> ().SetPause (false);
			ground1.GetComponent<GroundController> ().SetPause (false);
			ground2.GetComponent<GroundController> ().SetPause (false);

			// re-parent the marble to the new spoon
			instSpoon.transform.SetParent (gameCam.transform);
			empty.transform.SetParent (instSpoon.transform);
			instMarble.transform.SetParent (empty.transform, true);
		}
	}
}
