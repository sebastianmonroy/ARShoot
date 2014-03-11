using UnityEngine;
using System.Collections;

public class TetrisBlockHandler : MonoBehaviour {
	private float waitCount;
	public float waitDuration;
	private float incrementY;
	public bool fall;
	//private float finalY;

	void Start () {
		fall = true;
		incrementY = this.transform.Find("1").gameObject.collider.bounds.size.y;
		waitCount = 0;
	}
	
	void Update () {
		if (fall) {
			if (waitCount >= waitDuration) {
				this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - incrementY, this.transform.position.z);
				waitCount = 0;
			} else {
				waitCount += Time.deltaTime;
			}

			/*if (this.transform.position.y == finalY) {
				fall = false;
			}*/
		}
	}

	public void setX(float posX) {
		this.transform.position = new Vector3(Mathf.Clamp(posX, -90, 90), this.transform.position.y, this.transform.position.z);
	}

	public void setZ(float posZ) {
		this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, Mathf.Clamp(posZ, -90, 90));
	}

	public void incrementYRotation(int amount) {
		//this.transform.rotation = new Vector3(this.transform.rotation.x, this.transform.rotation.y + 90*amount, this.transform.rotation.z);s
		this.transform.Rotate(0, 90 * amount, 0, Space.World);
	}

	public void incrementXRotation(int amount) {
		//this.transform.rotation = new Vector3(this.transform.rotation.x + 90*amount, this.transform.rotation.y, this.transform.rotation.z);
		this.transform.Rotate(90 * amount, 0, 0, Space.World);
	}

	public void incrementZRotation(int amount) {
		//this.transform.rotation = new Vector3(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z + 90*amount);
		this.transform.Rotate(0, 0, 90 * amount, Space.World);
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
