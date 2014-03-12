using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TetrisBlockHandler : MonoBehaviour {
	private float waitCount;
	public float waitDuration;
	private float incrementY;
	public bool fall;
	public GameObject predictionPrefab;
	//public Material predictionMaterial;
	private GameObject prediction;

	void Start () {
		fall = true;
		incrementY = this.transform.Find("1").gameObject.collider.bounds.size.y;
		waitCount = 0;
		ShowPrediction();
	}
	
	void Update () {
		if (fall) {
			if (waitCount >= waitDuration) {
				this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - incrementY, this.transform.position.z);
				waitCount = 0;
			} else {
				waitCount += Time.deltaTime;
			}
		} else {
			Destroy(prediction);
		}
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

	private void ShowPrediction() {
		print("predict");
		if (prediction != null) {
			Destroy(prediction);
		}

		RaycastHit hit;
		Vector3 bestHitPoint = -1 * Vector3.one;
		foreach (Transform t in this.transform) {
			if (Physics.Raycast(t.position, -Vector3.up, out hit)) {
				if (hit.transform.gameObject.tag == "Cell" || (hit.transform.gameObject.tag == "Block" && hit.transform.parent != this.transform)) {
					if (bestHitPoint == -1 * Vector3.one || hit.point.y > bestHitPoint.y) {
						bestHitPoint = hit.point;
					}
				}
			}
		}
		print("best: " + bestHitPoint.y);
		
		Transform[] predictionTransforms = new Transform[4];
		int j = 0;
		foreach (Transform t in predictionPrefab.transform) {
			if (t.gameObject.tag == "Block") {
				predictionTransforms[j] = t;
				j++;
			}
		}

		Transform[] tetrisTransforms = new Transform[predictionTransforms.Length];
		j = 0;
		foreach (Transform t in this.transform) {
			if (t.gameObject.tag == "Block") {
				tetrisTransforms[j] = t;
				j++;
			}
		}

		print(predictionTransforms.Length + " " +  tetrisTransforms.Length);
		for (int i = 0; i < tetrisTransforms.Length; i++) {
			predictionTransforms[i].localPosition = tetrisTransforms[i].transform.localPosition;
		}

		prediction = Instantiate(predictionPrefab, new Vector3(this.transform.position.x, bestHitPoint.y+/*tetrisTransforms[0].gameObject.renderer.bounds.size.y*/20/2, this.transform.position.z), this.transform.rotation) as GameObject;
		prediction.transform.localScale = this.transform.localScale;

		/*// Duplicate this GameObject for use as the prediction GameObjectm using bestHitPoint as a reference location for Instantiation
		prediction = Instantiate(this.gameObject, bestHitPoint + new Vector3(0,this.renderer.bounds.size.y/2,0), this.transform.rotation) as GameObject;
		prediction.name = "Prediction";

		// Iterate through all children
		foreach (Transform t in prediction.transform) {
			print("1");
			// Disable all components for this child (block) of prediction
			MonoBehaviour[] components = t.gameObject.GetComponents<MonoBehaviour>();
			foreach (MonoBehaviour c in components) {
				print("2");
				// Except for MeshRenderer, which you just change the material of
				if (!(c is MeshRenderer) && !(c is MeshFilter)) {
					print("3");
					Destroy(c);
				} 
			}

			t.gameObject.GetComponent<MeshRenderer>().material = predictionMaterial;
		}*/
	}

	/*public void setFinalY(float posY) {
		finalY = posY;
	}*/

	/*void OnCollisionEnter(Collision collision) {
		print(collision.gameObject.tag);
		if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Block" || collision.gameObject.tag == "Tetris") {
			fall = false;
		}
	}*/
}
