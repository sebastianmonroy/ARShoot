using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class block : MonoBehaviour {
	public float delayDuration;
	private float delayCount;
	private bool removeable;
	private GameObject grid;
	private bool visible;
	private float bottomOfBlock;
	private Color originalColor;
	private Color selectedColor;
	private Color invisibleColor;
	private List<GameObject> connectedBlocks = new List<GameObject>();
	public GameObject jointPrefab;

	// Use this for initialization
	void Start () {
		delayCount = 0;
		removeable = false;
		visible = true;
		grid = GameObject.FindWithTag("Grid");
		bottomOfBlock = Mathf.Round(grid.transform.position.y);
		originalColor = this.renderer.material.color;
		selectedColor = new Color(1.0f, this.renderer.material.color.g, this.renderer.material.color.b, this.renderer.material.color.a);
		invisibleColor = new Color(this.renderer.material.color.r, this.renderer.material.color.g, this.renderer.material.color.b, 0.5f);
		this.renderer.material.color = selectedColor;

		/*RaycastHit[] hits;
		hits = Physics.SphereCastAll(this.transform.position, 20, Vector3.up);

		foreach (RaycastHit h in hits) {
			if (h.collider.gameObject.tag == "Block") {
				if (!alreadyConnected(h.collider.gameObject)) {
					connectedBlocks.Add(h.collider.gameObject);
					GameObject jointSource = Instantiate(jointPrefab, this.transform.position, this.transform.rotation) as GameObject;
					jointSource.transform.parent = this.transform;
					GameObject jointTarget = Instantiate(jointPrefab, h.collider.gameObject.transform.position, h.collider.gameObject.transform.rotation) as GameObject;
					jointTarget.transform.parent = h.collider.gameObject.transform;

					springSource = jointSource.GetComponent<SpringJoint>();
					springSource.maxDistance = 1.2f*(jointSource.transform.position - jointTarget.transform.position);
					springSource.connectedBody = jointTarget;
				}
			}
		}*/

	}

	private bool alreadyConnected(GameObject block) {
		bool found = false;
		foreach (GameObject b in connectedBlocks) {
			if (b == block) {
				found = true;
				break;
			}
		}
		return found;
	}
	
	// Update is called once per frame
	void Update () {
		if (!removeable) {
			delayCount += Time.deltaTime;
			if (delayCount >= delayDuration) {
				removeable = true;
			}
		}
		float centerOfGrid = Mathf.Round(grid.transform.position.y);
		//print("" + centerOfGrid + " " + bottomOfBlock);
		if (visible) {
			if (centerOfGrid > bottomOfBlock) {
				if (this.renderer.material.color != originalColor) {
					this.renderer.material.color = originalColor;
					this.renderer.collider.enabled = false;
				}
			} else if (centerOfGrid == bottomOfBlock) {
				if (this.renderer.material.color != selectedColor) {
					this.renderer.material.color = selectedColor;
					this.renderer.collider.enabled = true;
				}
			} else {
				visible = false;
			}
		} else {
			this.renderer.collider.enabled = false;
			if (this.renderer.material.color != invisibleColor) {
				this.renderer.material.color = invisibleColor;
			}
			if (centerOfGrid >= bottomOfBlock) {
				visible = true;
			}
		}
	}

	public void Remove() {
		if (removeable) {
			Destroy(this.gameObject);
		}
	}
}
