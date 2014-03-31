using UnityEngine;
using System.Collections;

public class build : MonoBehaviour {
	public GameObject TblockPrefab;
	public GameObject LblockPrefab;
	public GameObject ZblockPrefab;
	public GameObject OblockPrefab;
	public GameObject IblockPrefab;

	public GameObject TblockPredictionPrefab;
	public GameObject LblockPredictionPrefab;
	public GameObject ZblockPredictionPrefab;
	public GameObject OblockPredictionPrefab;
	public GameObject IblockPredictionPrefab;

	public float waitDuration;				// How long to wait between acknowledging gestures
	private float waitCount;
	public GameObject nextTetris;
	public GameObject selectedTetris;
	public GameObject selectedBlock;
	public GameObject selectedPreviewBlock;

	void Start () {
		waitCount = waitDuration/5;	// Don't feel like waiting as long for the first tetris block to spawn
		//getNextTetris();
	}
	
	void Update () {
		if (selectedTetris == null) {
			getNextTetris();
		}

		if (waitCount >= waitDuration) {
			// Prevents multiple gestures from being registered when only one was intended
			// Prevents any actions from being performed if there are no falling tetris blocks
			switch(GestureHandler.CurrentGesture) {
				// Handles performing certain "build" actions based on the user input detected in GestureHandler.cs
				case Gesture.CLICK:
					// CLICK gesture detected
					RaycastHit hit;
					if (Physics.Raycast(GestureHandler.CurrentRay, out hit)) {
						print("clicked on " + hit.transform.gameObject.tag);
						if (hit.transform.gameObject.tag == "Block") {
							if (selectedBlock != null) {
								// remove previous preview if necessary
								selectedBlock.GetComponent<block>().destroyPreview();
							}
							// preview available sides for selected block
							selectedBlock = hit.transform.gameObject;
							selectedBlock.GetComponent<block>().previewSides();
							if (selectedTetris.active) {
								selectedTetris.active = false;
							}
							waitCount = waitDuration;
						} else if (hit.transform.gameObject.tag == "Preview") {
							// show tetris prediction when preview block is touched
							selectedPreviewBlock = hit.transform.gameObject;
							selectedTetris.active = true;
							correctPreview();
							waitCount = 0;
						} else if (hit.transform.gameObject.tag == "Prediction") {
							// create tetrimino where the prediction is (if not colliding)
							if (!selectedTetris.GetComponent<TetriminoHandler>().isColliding) {
								selectedTetris.GetComponent<TetriminoHandler>().setPreview(false);
								selectedBlock.GetComponent<block>().destroyPreview();
								getNextTetris();
								waitCount = 0;
							}
						}
					}
					
					break;
				case Gesture.SCROLL_LEFT:
					// SCROLL_LEFT gesture detected
					print("scroll left");
					if (selectedTetris.active && selectedTetris.GetComponent<TetriminoHandler>().isPreview) {
						// If there is currently a selected tetris block, rotate it accordingly
						selectedTetris.GetComponent<TetriminoHandler>().incrementYRotation(1);
						correctPreview();
						waitCount = 0;
					}
					break;
				case Gesture.SCROLL_RIGHT:
					// SCROLL_RIGHT gesture detected
					print("scroll right");
					if (selectedTetris.active && selectedTetris.GetComponent<TetriminoHandler>().isPreview) {
						// If there is currently a selected tetris block, rotate it accordingly
						selectedTetris.GetComponent<TetriminoHandler>().incrementYRotation(-1);
						correctPreview();
						waitCount = 0;
					}
					break;
				case Gesture.SCROLL_DOWN:
					// SCROLL_DOWN gesture detected
					print("scroll down");
					if (selectedTetris.active && selectedTetris.GetComponent<TetriminoHandler>().isPreview) {
						// If there is currently a selected tetris block, rotate it accordingly
						selectedTetris.GetComponent<TetriminoHandler>().incrementRotation(selectedTetris.transform.position - this.transform.position);
						correctPreview();
						waitCount = 0;
					}
					break;
				case Gesture.SCROLL_UP:
					// SCROLL_UP gesture detected
					print("scroll up");
					if (selectedTetris.active && selectedTetris.GetComponent<TetriminoHandler>().isPreview) {
						// If there is currently a selected tetris block, rotate it accordingly
						selectedTetris.GetComponent<TetriminoHandler>().incrementRotation(selectedTetris.transform.position - this.transform.position);
						correctPreview();
						waitCount = 0;
					}
					break;
				default:
					break;
			}
		}

		waitCount += Time.deltaTime;
	}

	public void correctPreview() {
		Vector3 dir = selectedPreviewBlock.GetComponent<block_preview>().direction;
		//print("direction = " + dir);
		Vector3 offset = dir * selectedTetris.GetComponent<TetriminoHandler>().getOffset(dir);
		//print("offset = " + offset);
		selectedTetris.transform.position = selectedPreviewBlock.transform.position + offset;
	}

	public void getNextTetris() {
		print ("get next");
		if (nextTetris == null) {
			switch (Random.Range((int) 0, (int) 5)) {
				case 0:
					selectedTetris = Instantiate(TblockPrefab, Vector3.zero, Quaternion.identity) as GameObject;
					break;
				case 1:
					selectedTetris = Instantiate(LblockPrefab, Vector3.zero, Quaternion.identity) as GameObject;
					break;
				case 2:
					selectedTetris = Instantiate(ZblockPrefab, Vector3.zero, Quaternion.identity) as GameObject;
					break;
				case 3:
					selectedTetris = Instantiate(OblockPrefab, Vector3.zero, Quaternion.identity) as GameObject;
					break;
				case 4:
					selectedTetris = Instantiate(IblockPrefab, Vector3.zero, Quaternion.identity) as GameObject;
					break;
				default:
					selectedTetris = Instantiate(LblockPrefab, Vector3.zero, Quaternion.identity) as GameObject;
					break;
			}
		} else {
			selectedTetris = nextTetris;
		}
		selectedTetris.active = false;

		switch (Random.Range((int) 0, (int) 5)) {
			case 0:
				nextTetris = Instantiate(TblockPrefab, Vector3.zero, Quaternion.identity) as GameObject;
				break;
			case 1:
				nextTetris = Instantiate(LblockPrefab, Vector3.zero, Quaternion.identity) as GameObject;
				break;
			case 2:
				nextTetris = Instantiate(ZblockPrefab, Vector3.zero, Quaternion.identity) as GameObject;
				break;
			case 3:
				nextTetris = Instantiate(OblockPrefab, Vector3.zero, Quaternion.identity) as GameObject;
				break;
			case 4:
				nextTetris = Instantiate(IblockPrefab, Vector3.zero, Quaternion.identity) as GameObject;
				break;
			default:
				nextTetris = Instantiate(LblockPrefab, Vector3.zero, Quaternion.identity) as GameObject;
				break;
		}
		nextTetris.active = false;
	}
}
