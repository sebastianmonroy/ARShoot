using UnityEngine;
using System.Collections;

public class ball : MonoBehaviour {
	private GameObject trampoline;
	// Use this for initialization
	void Start () {
		trampoline = GameObject.FindGameObjectWithTag("Trampoline");
	}
	
	// Update is called once per frame
	void Update () {
		if (this.transform.position.y < trampoline.transform.position.y) {
			Destroy(this.gameObject);
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
