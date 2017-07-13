using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleController : MonoBehaviour {

	private Rigidbody rb;

	public Camera gameCamera;
	public float collisionFactor = 10.0f;

	public GameObject player;

	private bool isPaused;

	void Start () {
		isPaused = true;
		rb = GetComponent<Rigidbody> ();
	}

	void FixedUpdate() {
		if (!isPaused) {
			rb.isKinematic = false;
			rb.WakeUp ();
			//rb.AddForce (new Vector3 (gameCamera.transform.rotation.z * -1, 0, gameCamera.transform.rotation.x) * collisionFactor);
			//rb.AddForce(Input.acceleration * 5.0f);
		} else {
			rb.isKinematic = true;
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
