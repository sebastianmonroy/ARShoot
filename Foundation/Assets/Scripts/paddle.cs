using UnityEngine;
using System.Collections;

public class paddle : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Ball") {
				collision.gameObject.rigidbody.velocity =  (this.transform.forward * this.transform.parent.GetComponent<serve>().serveSpeed);
		}
	}
}