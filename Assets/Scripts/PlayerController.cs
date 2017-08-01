﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	public GameObject gameCam;

	public GameObject pointer;
	private bool endGame, isPaused, isBallProbablyFall;

	public GameObject ground1;
	public GameObject finish;

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

	//public float distanceToWin = 30f;
	private float distance;
	private float initialDistance;
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

		initialDistance =  Vector3.Distance(transform.position, new Vector3(transform.position.x, transform.position.y, finish.transform.position.z));

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
		Debug.Log (initialDistance);

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
			distance = Vector3.Distance(transform.position, new Vector3(transform.position.x, transform.position.y, finish.transform.position.z));

			// check then change spoons and marble
			if ((distance < ((initialDistance*8)/9) && distance >= (initialDistance/3) && indexSendok < 1) ||
				(distance < (initialDistance*7/9) && indexSendok < 2) || 
				(distance < (initialDistance*6/9) && indexSendok < 3) ||
				(distance < (initialDistance*5/9) && indexSendok < 4) ||
				(distance < (initialDistance*4/9) && indexSendok < 5) ||
				(distance < (initialDistance*3/9) && indexSendok < 6) ||
				(distance < (initialDistance*2/9) && indexSendok < 7) ||
				(distance < (initialDistance/9) && indexSendok < 8) ) {

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
			if (distance <= ((initialDistance*8)/9) + 5 && indexSendok < 1) {
				notif.enabled = true;
				notif.text = "Distance until checkpoint : " + (int)(distance - ((initialDistance*8)/9)) + " m";
			} else if (distance < (initialDistance*7/9) + 5 && indexSendok < 2) {
				notif.enabled = true;
				notif.text = "Distance until checkpoint : " + (int)(distance - (initialDistance*7/9)) + " m";
			} else if (distance < (initialDistance*6/9) + 5 && indexSendok < 3) {
				notif.enabled = true;
				notif.text = "Distance until checkpoint : " + (int)(distance - (initialDistance*6/9)) + " m";
			} else if (distance < (initialDistance*5/9) + 5 && indexSendok < 4) {
				notif.enabled = true;
				notif.text = "Distance until checkpoint : " + (int)(distance - (initialDistance*5/9)) + " m";
			} else if (distance < (initialDistance*4/9) + 5 && indexSendok < 5) {
				notif.enabled = true;
				notif.text = "Distance until checkpoint : " + (int)(distance - (initialDistance*4/9)) + " m";
			} else if (distance < (initialDistance*3/9) + 5 && indexSendok < 6) {
				notif.enabled = true;
				notif.text = "Distance until checkpoint : " + (int)(distance - (initialDistance*3/9)) + " m";
			} else if (distance < (initialDistance*2/9) + 5 && indexSendok < 7) {
				notif.enabled = true;
				notif.text = "Distance until checkpoint : " + (int)(distance - (initialDistance*2/9)) + " m";
			} else if (distance < (initialDistance/9) + 5 && indexSendok < 8) {
				notif.enabled = true;
				notif.text = "Distance until checkpoint : " + (int)(distance - (initialDistance/9)) + " m";
			}

			// Score
			score += (indexSendok + 1) * Time.deltaTime;
			scoreText.text = "Score : " + (int)score;

			// Distance 
			distanceText.text = "Distance to Finish : " + ((int)distance/10) + "m";
			if ((int)distance <= 0) {
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
		distanceText.color = Color.white;
		scoreText.color = Color.white;
		gameOverImage.SetActive(true);
		pointer.SetActive (true);
		CleanUp ();
	}

	private void FinishGame() {
		distanceText.text = "";
		pointer.SetActive (true);
		distanceText.color = Color.white;
		scoreText.color = Color.white;
		finishImage.SetActive (true);
		CleanUp ();
	}

	private void CleanUp() {
		Destroy (notif);
		Destroy (countdownText);
		Destroy (ground1);
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

			// re-parent the marble to the new spoon
			instSpoon.transform.SetParent (gameCam.transform);
			instMarble.transform.SetParent (gameCam.transform, true);
			empty.transform.SetParent (instSpoon.transform, true);

		}
	}
}
