using UnityEngine;
using System.Collections;

public class build : MonoBehaviour {
	public float waitDuration;
	private float waitCount;
	public GameObject tetrisSpawnPrefab;

	void Start () {
		waitCount = waitDuration;
	}
	
	void Update () {
		if (waitCount >= waitDuration) {
			switch(GestureHandler.CurrentGesture) {
				case Gesture.CLICK:
					print("click");
					break;
				case Gesture.SCROLL_LEFT:
					print("scroll left");
					break;
				case Gesture.SCROLL_RIGHT:
					print("scroll right");
					break;
				case Gesture.SCROLL_UP:
					print("scroll up");
					break;
				case Gesture.SCROLL_DOWN:
					print("scroll down");
					break;
			}
			waitCount = waitDuration;
		}

		waitCount += Time.deltaTime;
	}
}
