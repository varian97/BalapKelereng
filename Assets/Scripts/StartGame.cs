﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartGame : MonoBehaviour {

	public float gazeTime = 2.0f;
	private bool isgazed;
	private float timer;

	void Start() {
		isgazed = false;
		timer = 0.0f;
	}

	void Update() {
		if (isgazed) {
			timer += Time.deltaTime;
			if (timer >= gazeTime) {
				ExecuteEvents.Execute (gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
				timer = 0.0f;
			}
		}
	}

	public void PointerEnter() {
		isgazed = true;
	}

	public void PointerDown() {
		UnityEngine.SceneManagement.SceneManager.LoadScene (1);
	}

	public void PointerExit() {
		isgazed = false;
		timer = 0.0f;
	}
}
