using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpoonController : MonoBehaviour {

	private Camera gameCamera;
	private Vector3 camTempTransform;

	// Use this for initialization
	void Start () {
		gameCamera = GameObject.Find ("Camera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		//transform.localEulerAngles = new Vector3 (gameCamera.transform.rotation.x, 0, gameCamera.transform.rotation.z);
		transform.RotateAround(gameCamera.transform.forward, new Vector3(1,0,0), gameCamera.transform.rotation.x);
	}
}
