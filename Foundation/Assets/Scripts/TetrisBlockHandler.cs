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
