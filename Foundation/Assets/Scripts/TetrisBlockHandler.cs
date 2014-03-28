using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TetrisBlockHandler : MonoBehaviour {
	private float waitCount;
	public float waitDuration;
	private float incrementY;
	public bool fall;
	public GameObject predictionPrefab;
	private GameObject prediction;
	private Ray debugRay = new Ray();
	private float debugRayDistance = 0.0f;
	private bool joined;
	void Start () {
		fall = true;
		joined = false;
		incrementY = 40;//this.transform.Find("1").gameObject.renderer.bounds.size.y;
		waitCount = 0;
		ShowPrediction();
	}
	
	void Update () {
		/*if (this.transform.position.y < 0) {
			Destroy(this.gameObject);
		}*/

		if (fall) {
			if (waitCount >= waitDuration) {
				this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - incrementY, this.transform.position.z);
				// Update Prediction Prefab every time the Tetris moves
				ShowPrediction();
				if (Vector3.Distance(this.transform.position, prediction.transform.position) < 20) {
					this.transform.position = prediction.transform.position;
					fall = false;
				}

				waitCount = 0;
			} else {
				waitCount += Time.deltaTime;
			}
			Debug.DrawRay(debugRay.origin, debugRay.direction * debugRayDistance, Color.red, Time.deltaTime);
		} else if (!joined) {
			Simulate();
		}	
	}

	public void Simulate() {
		// Stop predicting when the Tetris stops falling
		// Create FixedJoints between the blocks
		// Unfreeze blocks, make them use gravity
		Destroy(prediction);
		foreach (Transform t in this.transform) {
			//t.gameObject.GetComponent<block>().Jointify();
		}
		joined = true;
		fall = false;
	}

	public void Throw(GameObject throwBlock, Vector3 throwVector) {
		Destroy(prediction);
		/*foreach (Transform t in this.transform) {
			t.gameObject.GetComponent<block>().Jointify();
		}*/

		foreach (Transform t in this.transform) {
			t.gameObject.GetComponent<block>().PrepareToThrow();
			if (t == throwBlock.transform) {
				t.gameObject.rigidbody.velocity = throwVector;
			}
		}

		joined = true;
		fall = false;
	}

	public void setX(float posX) {
		this.transform.position = new Vector3(Mathf.Clamp(posX, -90, 90), this.transform.position.y, this.transform.position.z);
	}

	public void setZ(float posZ) {
		this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, Mathf.Clamp(posZ, -90, 90));
	}

	public void setXZ(float posX, float posZ) {
		setX(posX);
		setZ(posZ);
		ShowPrediction();
	}

	public void incrementYRotation(int amount) {
		//this.transform.rotation = new Vector3(this.transform.rotation.x, this.transform.rotation.y + 90*amount, this.transform.rotation.z);s
		this.transform.Rotate(0, 90 * amount, 0, Space.World);
		ShowPrediction();
	}

	public void incrementXRotation(int amount) {
		//this.transform.rotation = new Vector3(this.transform.rotation.x + 90*amount, this.transform.rotation.y, this.transform.rotation.z);
		this.transform.Rotate(90 * amount, 0, 0, Space.World);
		ShowPrediction();
	}

	public void incrementZRotation(int amount) {
		//this.transform.rotation = new Vector3(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z + 90*amount);
		this.transform.Rotate(0, 0, 90 * amount, Space.World);
		ShowPrediction();
	}

	public void incrementRotation(Vector3 aimDirection) {
		if (Mathf.Abs(aimDirection.x) >= Mathf.Abs(aimDirection.z)) {
			incrementZRotation((int) (aimDirection.x/Mathf.Abs(aimDirection.x)));
		} else {
			incrementXRotation((int) (aimDirection.z/Mathf.Abs(aimDirection.z)));
		}
	}

	private void ShowPrediction() {
		//print("predict");
		if (prediction != null) {
			// Destroy the previous prediction before updating
			Destroy(prediction);
		}

		RaycastHit hit;
		RaycastHit bestHit;
		Physics.Raycast(this.transform.position, Vector3.up, out bestHit, 0.1f);
		Transform bestTransform = this.transform;
		bool instantiated = false;
		foreach (Transform t in this.transform) {
			// Iterate through the blocks in this Tetris and find which will collide first and gather relevant information
			if (t.gameObject.tag == "Block") {
				Ray currentRay = new Ray(t.position, -Vector3.up);
				if (Physics.Raycast(currentRay, out hit, (1 << 8) | (1 << 9))) {
					if (hit.transform.parent.gameObject != this.gameObject) {
						if (!instantiated || hit.distance < bestHit.distance) {
							bestHit = hit;
							bestTransform = t;
							debugRay = currentRay;
							instantiated = true;
							//bestBlock = t.gameObject;
						}
					}
				}
			}
		}
		debugRayDistance = bestHit.distance;
		//print("best: " + bestHit.point.y);
		//print("t: " + bestTransform.gameObject.tag);


		if (bestTransform.gameObject.tag == "Block") {
			// Evaluate how high to spawn the Prediction Prefab
			float predictionY = this.transform.position.y - bestTransform.gameObject.renderer.bounds.min.y;

			prediction = Instantiate(predictionPrefab, new Vector3(this.transform.position.x, bestHit.point.y + predictionY, this.transform.position.z), this.transform.rotation) as GameObject;
			prediction.transform.parent = this.transform.parent;
		}
	}

	/*void OnCollisionEnter(Collision collision) {
		print(collision.gameObject.tag);
		if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Block" || collision.gameObject.tag == "Tetris") {
			fall = false;
		}
	}*/
}
