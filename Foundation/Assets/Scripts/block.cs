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
	public GameObject jointPrefab;
	private List<GameObject> connectedObjects = new List<GameObject>();

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

		RaycastHit[] hits;
		hits = Physics.SphereCastAll(this.transform.position, 15, Vector3.up);

		foreach (RaycastHit h in hits) {
			GameObject targetObject = h.collider.gameObject;
			if (h.collider.gameObject.tag == "Block") {
				GameObject jointSource = Instantiate(jointPrefab, this.transform.position, this.transform.rotation) as GameObject;
				jointSource.transform.parent = this.transform;
				GameObject jointTarget = Instantiate(jointPrefab, targetObject.transform.position, targetObject.transform.rotation) as GameObject;
				jointTarget.transform.parent = targetObject.transform;

				jointTarget.transform.parent.GetComponent<block>().AddConnection(jointSource);

				SpringJoint springSource = jointSource.GetComponent<SpringJoint>();
				springSource.maxDistance = 1.2f*(jointSource.transform.position - jointTarget.transform.position).magnitude;
				springSource.connectedBody = jointTarget.rigidbody;
			}
		}
	}

	public void RemoveConnection(GameObject jointTarget) {
		foreach (Transform t in this.transform) {
			if (t.gameObject.GetComponent<SpringJoint>().connectedBody == jointTarget.rigidbody) {
				Destroy(t.gameObject);
				connectedObjects.Remove(jointTarget);
				break;
			}
		}
	}

	public void AddConnection(GameObject jointSource) {
		connectedObjects.Add(jointSource);
	}

	/*public void createJoint(GameObject targetObject) {
		createSourceJoint(targetObject);
		target.GetComponent<block>().createTargetJoint(this.gameObject);
	}

	public void createTargetJoint(GameObject sourceObject) {
		if (!alreadyHaveJoint(sourceObject)) {
			targetJoints.Add(sourceObject);
		}
	}

	public void createSourceJoint(GameObject targetObject) {
		if (!alreadyHaveJoint(targetObject)) {
			GameObject jointSource = Instantiate(jointPrefab, this.transform.position, this.transform.rotation) as GameObject;
			jointSource.transform.parent = this.transform;
			GameObject jointTarget = Instantiate(jointPrefab, targetObject.transform.position, targetObject.transform.rotation) as GameObject;
			jointTarget.transform.parent = targetObject.transform;

			springSource = jointSource.GetComponent<SpringJoint>();
			springSource.maxDistance = 1.2f*(jointSource.transform.position - jointTarget.transform.position);
			springSource.connectedBody = jointTarget;

			sourceJoints.Add(targetObject);
		}
	}

	private GameObject getTargetJoint(GameObject joint) {
		GameObject targetJoint = null;
		foreach (GameObject j in targetJoints) {
			if (j == joint) {
				targetJoint = j;
				break;
			}
		}
		return targetJoint;
	}

	public GameObject removeSourceJoint(GameObject targetJoint) {
		sourceJoints.Remove(targetJoint);
		Destroy(targetJoint.gameObject);
	}

	public GameObject removeJoint(GameObject targetJoint) {
		removeSourceJoint(targetJoint);
		targetJoint.GetComponent<block>.removeSourceJoint
	}

	private bool alreadyHaveSourceJoint(GameObject block) {
		bool found = false;
		foreach (GameObject b in sourceJoints) {
			if (b == block) {
				found = true;
				break;
			}
		}
		return found;
	}

	private bool alreadyHaveJoint(GameObject block) {
		return (alreadyHaveTargetJoint(block) || alreadyHaveSourceJoint(block));
	}*/
	
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

	public void OnDestroy() {
		foreach (GameObject c in connectedObjects) {
			c.GetComponent<block>().RemoveConnection(this.gameObject);
		}
	}
}
