using UnityEngine;
using System.Collections;

public class ball : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (this.transform.position.y < 0) {
			Destroy(this);
		}
	}

	void OnDestroy() {
		Camera.main.SendMessage("RoundOver");
	}

	void OnCollision(Collision collision) {
		if (collision.gameObject.tag == "Paddle") {
			Camera.main.SendMessage("ResetPaddle");
		}
	}
}
