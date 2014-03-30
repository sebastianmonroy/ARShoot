using UnityEngine;
using System.Collections;

public class build : MonoBehaviour {
	public float waitDuration;				// How long to wait between acknowledging gestures
	private float waitCount;
	public GameObject tetrisSpawnPrefab;
	public GameObject selectedTetris;		// GameObject containing the tetris block that was last spawned

	void Start () {
		waitCount = waitDuration/5;	// Don't feel like waiting as long for the first tetris block to spawn
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
					//print("click");
					RaycastHit hit;
					if (Physics.Raycast(GestureHandler.CurrentRay, out hit, (1 << 8) | (1 << 9))) {
						//print("tag = " + hit.transform.gameObject.tag);
						if (hit.transform.gameObject.tag == "Cell") {
							// Cell clicked on
							//print("hit cell");
							if (selectedTetris.GetComponent<TetrisBlockHandler>().fall) {
								// If there is currently a falling tetris block, move it to be above the selected cell
								selectedTetris.GetComponent<TetrisBlockHandler>().setXZ(hit.transform.gameObject.transform.position.x, hit.transform.gameObject.transform.position.z);
							}
						} else if (hit.transform.gameObject.tag == "Block") {
							// Block clicked on
							/*print("hit block");
							 
							if (selectedTetris.GetComponent<TetrisBlockHandler>().fall && hit.transform.parent.gameObject != selectedTetris) {
								// If there is currently a falling tetris block AND the block clicked on isn't part of the selected tetris, move it to be above the selected block
								selectedTetris.GetComponent<TetrisBlockHandler>().setXZ(hit.transform.gameObject.transform.position.x, hit.transform.gameObject.transform.position.z);
							} else if (hit.transform.parent.gameObject == selectedTetris) {
								// If there is currently a falling tetris block ANND the block clicked IS part of the selected tetris, push it
								Vector3 camRotV3 = GestureHandler.CurrentRay.direction.normalized;
								print("force direction: " + camRotV3);
								selectedTetris.GetComponent<TetrisBlockHandler>().Throw(hit.transform.gameObject, camRotV3 * 3000);
								//selectedTetris.GetComponent<Rigidbody>().AddForce(Vector3.up * 30000);
							}*/
			
						}
					}
					waitCount = 0;
					break;
				case Gesture.SCROLL_LEFT:
					// SCROLL_LEFT gesture detected
					print("scroll left");
					if (selectedTetris.GetComponent<TetrisBlockHandler>().fall) {
						// If there is currently a falling tetris block, rotate it accordingly
						selectedTetris.GetComponent<TetrisBlockHandler>().incrementYRotation(1);
						waitCount = 0;
					}
					break;
				case Gesture.SCROLL_RIGHT:
					// SCROLL_RIGHT gesture detected
					print("scroll right");
					if (selectedTetris.GetComponent<TetrisBlockHandler>().fall) {
						// If there is currently a falling tetris block, rotate it accordingly
						selectedTetris.GetComponent<TetrisBlockHandler>().incrementYRotation(-1);
						waitCount = 0;
					}
					break;
				case Gesture.SCROLL_DOWN:
					// SCROLL_DOWN gesture detected
					print("scroll down");
					if (selectedTetris.GetComponent<TetrisBlockHandler>().fall) {
						// If there is currently a falling tetris block, rotate it accordingly
						selectedTetris.GetComponent<TetrisBlockHandler>().incrementRotation(selectedTetris.transform.position - this.transform.position);
						waitCount = 0;
					}
					break;
				case Gesture.SCROLL_UP:
					// SCROLL_UP gesture detected
					print("scroll up");
					if (selectedTetris.GetComponent<TetrisBlockHandler>().fall) {
						// If there is currently a falling tetris block, rotate it accordingly
						selectedTetris.GetComponent<TetrisBlockHandler>().incrementRotation(selectedTetris.transform.position - this.transform.position);
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
