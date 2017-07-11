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
		//transform.localEulerAngles = new Vector3 (gameCamera.transform.rotation.x, gameCamera.transform.rotation.y, gameCamera.transform.rotation.z);
		if(transform.rotation.z <= 45.0f && transform.rotation.z >= -45.0f)
			transform.Rotate(0, 0, gameCamera.transform.rotation.z * 10.0f);
		if (gameCamera.transform.rotation.z <= 1.0f && gameCamera.transform.rotation.z >= -1.0f) {
			if(transform.rotation.z > 0.0f)
				transform.Rotate (0, 0, -1.0f * 10.0f);
			else
				transform.Rotate (0, 0, 1.0f * 10.0f);
		}
			
	}
}
