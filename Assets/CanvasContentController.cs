using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasContentController : MonoBehaviour {
	public Text GOScore;
	public Text GODistance;

	public void TransferScoreData(int score) {
		GOScore.text = "Score : " + score;
		GOScore.color = Color.white;
		gameObject.SetActive (false);
	}

	public void TransferDistanceData(int distance) {
		GODistance.text = "Distance to Finish : " + distance;
		GODistance.color = Color.white;
		gameObject.SetActive (false);
	}
}
