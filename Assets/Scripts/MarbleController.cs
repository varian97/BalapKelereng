using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleController : MonoBehaviour {

	private Rigidbody rb;
	public Camera gameCamera;
	public float collisionFactor = 10.0f;

	public GameObject player;
	public GameObject ground;

	private bool isPaused;

	void Start () {
		isPaused = true;
		rb = GetComponent<Rigidbody> ();
	}

	void Update () {
		if (!isPaused) {
			rb.isKinematic = false;
			rb.AddForce (new Vector3 (gameCamera.transform.rotation.z, 0, gameCamera.transform.rotation.x) * collisionFactor);
		} else {
			rb.isKinematic = true;
		}
	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject.tag == "Ground") {
			player.GetComponent<PlayerController> ().SetEndGame (true);
			ground.GetComponent<GroundController> ().SetEndGame (true);
			Debug.Log ("GameOver");
		}
	}

	public void SetPause(bool pause) {
		isPaused = pause;
	}
}
