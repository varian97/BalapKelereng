using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour {

	private CharacterController groundController;
	public float speed = 5.0f;
	private bool endGame;
	private bool isPaused;

	void Start () {
		endGame = false;
		isPaused = true;
		groundController = GetComponent<CharacterController> ();
	}

	void Update () {
		if(!endGame && !isPaused)
			groundController.Move (Vector3.back * speed * Time.deltaTime);
	}

	public void SetEndGame(bool end) {
		endGame = end;
	}

	public void SetPause(bool pause) {
		isPaused = pause;
	}
}
