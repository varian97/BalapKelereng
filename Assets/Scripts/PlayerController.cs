using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	private bool endGame;

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

	void Awake () {
		listOfSpoons = Resources.LoadAll ("Spoons", typeof(GameObject));
		activeSpoons = new List<GameObject> ();
		indexSendok = 0;
		distance = 0;
		score = 0;
		instSpoon = null;
		endGame = false;
		marblesPos = marbles.transform.position;
	}

	void Start() {
		instSpoon = Instantiate ((GameObject)listOfSpoons.GetValue (indexSendok), parentSpoon.transform.position, parentSpoon.transform.rotation) as GameObject;
		activeSpoons.Add (instSpoon);
	}

	void Update () {

		if (endGame) {
			GameOver ();
		} else {
			distance += Time.deltaTime;
			if (transform.position.z > ground1.transform.position.z) {
				if (Vector3.Distance (transform.position, ground2.transform.position) < 30.0f) {
					float x = ground2.transform.position.x;
					float y = ground2.transform.position.y;
					float z = ground2.transform.position.z + 70.0f;

					ground1.transform.position = new Vector3 (x,y,z);
				}
			}

			if (transform.position.z > ground2.transform.position.z) {
				if (Vector3.Distance (transform.position, ground1.transform.position) < 30.0f) {
					float x = ground1.transform.position.x;
					float y = ground1.transform.position.y;
					float z = ground1.transform.position.z + 70.0f;

					ground2.transform.position = new Vector3 (x,y,z);
				}
			}

			// check and change spoons
			if (distance > 10 && distance <= 20 && indexSendok < 1) {
				Destroy (activeSpoons [0]);
				activeSpoons.RemoveAt (0);
				indexSendok += 1;
				instSpoon = Instantiate ((GameObject)listOfSpoons.GetValue (indexSendok), parentSpoon.transform.position, parentSpoon.transform.rotation) as GameObject;
				activeSpoons.Add (instSpoon);
				marbles.transform.position = marblesPos;
			} else if (distance > 20 && indexSendok < 2) {
				Destroy (activeSpoons [0]);
				activeSpoons.RemoveAt (0);
				indexSendok += 1;
				instSpoon = Instantiate ((GameObject)listOfSpoons.GetValue (indexSendok), parentSpoon.transform.position, parentSpoon.transform.rotation) as GameObject;
				activeSpoons.Add (instSpoon);
				marbles.transform.position = marblesPos;
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

	private void GameOver() {
		gameOverImage.SetActive(true);
		CleanUp ();
	}

	private void FinishGame() {
		distanceText.text = "";
		finishImage.SetActive (true);
		CleanUp ();
	}

	private void CleanUp() {
		Destroy (ground1);
		Destroy (ground2);
		Destroy (marbles);
		Destroy (parentSpoon);
		Destroy (instSpoon);
	}
}
