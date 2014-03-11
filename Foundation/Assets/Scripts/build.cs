using UnityEngine;
using System.Collections;

public class build : MonoBehaviour {
	public float waitDuration;
	private float waitCount;
	public GameObject tetrisSpawnPrefab;
	public GameObject selectedTetris;

	void Start () {
		waitCount = waitDuration/2;
		selectedTetris = null;
	}
	
	void Update () {
		if (waitCount >= waitDuration) {
			switch(GestureHandler.CurrentGesture) {
				case Gesture.CLICK:
					print("click");
					RaycastHit hit;
					if (Physics.Raycast(GestureHandler.CurrentRay, out hit)) {
						print("hit");
						if (hit.transform.gameObject.tag == "Cell") {
							print("cell");
							if (selectedTetris.GetComponent<TetrisBlockHandler>().fall) {
								selectedTetris.GetComponent<TetrisBlockHandler>().setX(hit.transform.gameObject.transform.position.x);
								selectedTetris.GetComponent<TetrisBlockHandler>().setZ(hit.transform.gameObject.transform.position.z);
							}
						}
					}
					waitCount = 0;
					break;
				case Gesture.SCROLL_LEFT:
					print("scroll left");
					if (selectedTetris != null) {
						if (selectedTetris.GetComponent<TetrisBlockHandler>().fall) {
							selectedTetris.GetComponent<TetrisBlockHandler>().incrementYRotation(-1);
						}
						waitCount = 0;
					}
					break;
				case Gesture.SCROLL_RIGHT:
					print("scroll right");
					if (selectedTetris != null) {
						if (selectedTetris.GetComponent<TetrisBlockHandler>().fall) {
							selectedTetris.GetComponent<TetrisBlockHandler>().incrementYRotation(1);
						}
						waitCount = 0;
					}
					break;
				case Gesture.SCROLL_UP:
					print("scroll up");
					if (selectedTetris != null) {
						if (selectedTetris.GetComponent<TetrisBlockHandler>().fall) {
							selectedTetris.GetComponent<TetrisBlockHandler>().incrementXRotation(1);
						}
						waitCount = 0;
					}
					waitCount = 0;
					break;
				case Gesture.SCROLL_DOWN:
					print("scroll down");
					if (selectedTetris != null) {
						if (selectedTetris.GetComponent<TetrisBlockHandler>().fall) {
							selectedTetris.GetComponent<TetrisBlockHandler>().incrementXRotation(-1);
						}
						waitCount = 0;
					}
					waitCount = 0;
					break;
			}
		}

		waitCount += Time.deltaTime;
	}


}
