using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleController : MonoBehaviour {

	private Rigidbody rb;

	private Vector3 marblesPos;

	[SerializeField]
	private Camera gameCamera;
	[SerializeField]
	private GameObject player;

	private bool isPaused;

	void Start () {
		gameCamera = GameObject.Find ("Camera").GetComponent<Camera>();
		player = GameObject.Find ("PlayerPos");

		marblesPos = transform.position;
		isPaused = true;
		rb = GetComponent<Rigidbody> ();
	}

	void FixedUpdate() {
		if (!isPaused) {
			rb.isKinematic = false;
			rb.WakeUp ();
		} else {
			rb.isKinematic = true;
			transform.position = marblesPos;
		}
	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject.tag == "Ground") {
			player.GetComponent<PlayerController> ().SetEndGame (true);
		}
	}

	void OnCollisionExit(Collision other){
		if (other.gameObject.tag == "spoon") {
			player.GetComponent<PlayerController> ().SetProbablyFall (true);
		}
	}

	public void SetPause(bool pause) {
		isPaused = pause;
	}
}
