using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Gesture { CLICK, SCROLL_LEFT, SCROLL_RIGHT, SCROLL_UP, SCROLL_DOWN, NOTHING };

public class GestureHandler : MonoBehaviour {
	public static Gesture CurrentGesture;
	public static Ray CurrentRay;
	private float delayCount;
	public float scrollThreshold;

	void Start () {
		CurrentGesture = Gesture.NOTHING;
		delayCount = 0;
	}
	
	void Update () {
		getInput();
	}

	private void getInput() {
		if (Input.GetMouseButtonDown(0)) {
			CurrentRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			CurrentGesture = Gesture.CLICK;
		} else if (Input.GetAxis("Mouse ScrollWheel") != 0) {
			float deltaY = Input.GetAxis("Mouse ScrollWheel");
			if (deltaY > 0) {
				CurrentGesture = Gesture.SCROLL_UP;
			} else {
				CurrentGesture = Gesture.SCROLL_DOWN;
			}
		} else if (Input.touchCount == 1) {
			delayCount += Time.deltaTime;
			if (delayCount >= 0.1f) {
				CurrentRay = Camera.main.ScreenPointToRay(Input.touches[0].position);
				CurrentGesture = Gesture.CLICK;
			} else {
				delayCount += Time.deltaTime;
				CurrentGesture = Gesture.NOTHING;
			}			
		} else if (Input.touchCount == 2) {
			delayCount = 0;
			float totalMoveY = 0;
			float totalMoveX = 0;
			foreach (Touch t in Input.touches) {
				totalMoveX += t.deltaPosition.x * Time.deltaTime/t.deltaTime/5;
				totalMoveY += t.deltaPosition.y * Time.deltaTime/t.deltaTime/5;
			}

			int deltaX = (int) (totalMoveX / Input.touchCount);
			int deltaY = (int) (totalMoveY / Input.touchCount);

			if (deltaX >= deltaY) {
				if (deltaX >= scrollThreshold) {
					CurrentGesture = Gesture.SCROLL_RIGHT;
				} else if (deltaX <= -scrollThreshold) {
					CurrentGesture = Gesture.SCROLL_LEFT;
				}
			} else {
				if (deltaY >= scrollThreshold) {
					CurrentGesture = Gesture.SCROLL_UP;
				} else if (deltaY <= -scrollThreshold) {
					CurrentGesture = Gesture.SCROLL_DOWN;
				}
			}
		} else {
			delayCount = 0;
			CurrentGesture = Gesture.NOTHING;
		}
	}

	public Gesture GetGesture() {
		return CurrentGesture;
	}

	public Ray GetRay() {
		return CurrentRay;
	}
}
