using UnityEngine;
using System.Collections;

public class build : MonoBehaviour {
	public float waitDuration;				// How long to wait between acknowledging gestures
	private float waitCount;
	public GameObject tetrisSpawnPrefab;
	public GameObject selectedTetris;		// GameObject containing the tetris block that was last spawned

	void Start () {
		waitCount = waitDuration/2;	// Don't feel like waiting as long for the first tetris block to spawn
		selectedTetris = null;
	}
	
	void Update () {
		if (waitCount >= waitDuration && selectedTetris != null) {
			// Prevents multiple gestures from being registered when only one was intended
			// Prevents any actions from being performed if there are no falling tetris blocks
			switch(GestureHandler.CurrentGesture) {
				// Handles performing certain "build" actions based on the user input detected in GestureHandler.cs
				case Gesture.CLICK:
					// CLICK gesture detected
					print("click");
					RaycastHit hit;
					if (Physics.Raycast(GestureHandler.CurrentRay, out hit)) {
						if (hit.transform.gameObject.tag == "Cell") {
							print("hit cell");
							if (selectedTetris.GetComponent<TetrisBlockHandler>().fall) {
								// If there is currently a falling tetris block, move it to be above the selected cell
								selectedTetris.GetComponent<TetrisBlockHandler>().setX(hit.transform.gameObject.transform.position.x);
								selectedTetris.GetComponent<TetrisBlockHandler>().setZ(hit.transform.gameObject.transform.position.z);
							}
						}
					}
					waitCount = 0;
					break;
				case Gesture.SCROLL_LEFT:
					// SCROLL_LEFT gesture detected
					print("scroll left");
					if (selectedTetris.GetComponent<TetrisBlockHandler>().fall) {
						// If there is currently a falling tetris block, rotate it accordingly
						selectedTetris.GetComponent<TetrisBlockHandler>().incrementYRotation(-1);
						waitCount = 0;
					}
					break;
				case Gesture.SCROLL_RIGHT:
					// SCROLL_RIGHT gesture detected
					print("scroll right");
					if (selectedTetris.GetComponent<TetrisBlockHandler>().fall) {
						// If there is currently a falling tetris block, rotate it accordingly
						selectedTetris.GetComponent<TetrisBlockHandler>().incrementYRotation(1);
						waitCount = 0;
					}
					break;
				case Gesture.SCROLL_DOWN:
					// SCROLL_DOWN gesture detected
					print("scroll down");
					if (selectedTetris.GetComponent<TetrisBlockHandler>().fall) {
						// If there is currently a falling tetris block, rotate it accordingly
						selectedTetris.GetComponent<TetrisBlockHandler>().incrementXRotation(-1);
						waitCount = 0;
					}
					break;
				case Gesture.SCROLL_UP:
					// SCROLL_UP gesture detected
					print("scroll up");
					if (selectedTetris.GetComponent<TetrisBlockHandler>().fall) {
						// If there is currently a falling tetris block, rotate it accordingly
						selectedTetris.GetComponent<TetrisBlockHandler>().incrementXRotation(1);
						waitCount = 0;
					}
					break;
				default:
					break;
			}
		}

		waitCount += Time.deltaTime;
	}


}
