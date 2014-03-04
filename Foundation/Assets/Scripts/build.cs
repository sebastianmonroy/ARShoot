using UnityEngine;
using System.Collections;

public class build : MonoBehaviour {
	public float waitDuration;
	private float waitCount;
	private Ray inputRay;
	public GameObject grid;
	public int mouseScrollScale;
	public int fingerScrollScale;
	private int dragMode; // 1: build, 0: nothing, -1: remove
	private float delayCount;
	private float initialPinchDistance;
	private float pinchMultiplier;
	// Use this for initialization
	void Start () {
		waitCount = waitDuration;
		delayCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (waitCount <= 0) {
			/*if (Input.GetMouseButtonDown(0)) {
				RaycastHit hit = new RaycastHit();
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);
				if (Physics.Raycast(ray, out hit)) {
					if (hit.collider.gameObject.tag == "Cell") {
						hit.collider.gameObject.SendMessage("SpawnCube");
					}
				}
			}
			waitCount = waitDuration;*/
			
			/*if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled) {
				// reset dragMode if no longer dragging single finger
				dragMode = 0;
			}*/

			string input = getInput();
			if (input[0] == 'c') {	
				RaycastHit hit = new RaycastHit();
				Debug.DrawRay(inputRay.origin, inputRay.direction * 100, Color.yellow);
				if (Physics.Raycast(inputRay, out hit)) {
					if (hit.collider.gameObject.tag == "Cell") {
						hit.collider.gameObject.SendMessage("SpawnBlock");
						dragMode = 1;
					} else if (hit.collider.gameObject.tag == "Block") {
						hit.collider.gameObject.SendMessage("Remove");
						dragMode = -1;
					}
				}
			} else if (input[0] == 'd') {
				RaycastHit hit = new RaycastHit();
				Debug.DrawRay(inputRay.origin, inputRay.direction * 100, Color.yellow);
				if (Physics.Raycast(inputRay, out hit)) {
					if (dragMode == 1) {
						if (hit.collider.gameObject.tag == "Cell") {
							hit.collider.gameObject.SendMessage("SpawnBlock");
						}
					} else if (dragMode == -1) {
						if (hit.collider.gameObject.tag == "Block") {
							hit.collider.gameObject.SendMessage("Remove");
						}
					}
				}
			} else if (input[0] == 's') {
				string amount = input.Substring(1);
				float curCellSize = grid.GetComponent<GridHandler>().cellSize.x;
				float moveAmount = int.Parse(amount) * curCellSize/4;

				grid.transform.position += new Vector3(0, moveAmount, 0);
				print("scroll" + moveAmount);
				grid.transform.position = new Vector3(grid.transform.position.x, Mathf.Clamp(Mathf.Round(grid.transform.position.y*1000)/1000, -50, 200), grid.transform.position.z);
			} else if (input[0] == 'p') {
				string amount = input.Substring(1);
				float scaleAmount = float.Parse(amount);
				grid.SendMessage("SpawnGrid", scaleAmount);
				print("pinch" + scaleAmount);
			}
			waitCount = waitDuration;
		}

		waitCount -= Time.deltaTime;
	}

	private string getInput() {
		if (Input.GetMouseButtonDown(0)) {
			inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			return "c";
		} else if (Input.GetAxis("Mouse ScrollWheel") != 0) {
			float deltaY = Input.GetAxis("Mouse ScrollWheel");
			return "s" + (int) (deltaY * mouseScrollScale);
		} else if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began) {
			initialPinchDistance = -1;
			if (delayCount >= 0.05f && Input.touchCount == 1) {
				inputRay = Camera.main.ScreenPointToRay(Input.touches[0].position);
				dragMode = 0;
				delayCount = 0;
				return "c";
			} else {
				delayCount += Time.deltaTime;
				return "false";
			}			
		} else if (Input.touchCount == 1 && Input.GetTouch(0).phase != TouchPhase.Moved) {
			delayCount = 0;
			initialPinchDistance = -1;
			inputRay = Camera.main.ScreenPointToRay(Input.touches[0].position);
			return "d";
		} else if (Input.touchCount == 2) {
			float currentPinchDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
			float fingerPinchScale;
			//delayCount += Time.deltaTime;
			if (initialPinchDistance == -1) {
				initialPinchDistance = currentPinchDistance;
				pinchMultiplier = 2;
				return "false";
			} else if (currentPinchDistance >= initialPinchDistance * pinchMultiplier) {
				fingerPinchScale = currentPinchDistance / initialPinchDistance - (currentPinchDistance % initialPinchDistance) / initialPinchDistance;
				fingerPinchScale = 1 / fingerPinchScale;
				pinchMultiplier = Mathf.Clamp(pinchMultiplier*2, 2.0f, 4.0f);
				return "p" + fingerPinchScale;
			} else if (currentPinchDistance <= initialPinchDistance / pinchMultiplier) {
				fingerPinchScale = initialPinchDistance / currentPinchDistance - (initialPinchDistance % currentPinchDistance) / currentPinchDistance;
				pinchMultiplier = Mathf.Clamp(pinchMultiplier*2, 2.0f, 4.0f);
				return "p" + fingerPinchScale;
			} else {
				return "false";
			}
		} else if (Input.touchCount == 3) {
			float totalMove = 0;
			foreach (Touch t in Input.touches) {
				totalMove += t.deltaPosition.y * Time.deltaTime/t.deltaTime/5;
			}

			int deltaY = (int) (totalMove / Input.touchCount);
			return "s" + Mathf.Clamp(deltaY, -1, 1);
		} else {
			delayCount = 0;
			initialPinchDistance = -1;
			return "false";
		}
	}
}
